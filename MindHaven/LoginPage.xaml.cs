using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage; // Needed for Preferences

namespace MindHaven
{
    public partial class LoginPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler());

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Email and Password cannot be empty.", "OK");
                return;
            }

            var loginData = new { email, password };
            string jsonData = JsonSerializer.Serialize(loginData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://172.20.10.2/mindhaven/login.php", content);
                string result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Server Response: {result}"); // Debugging Output

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", "Failed to connect to server.", "OK");
                    return;
                }

                // Ensure the response is valid JSON
                if (result.Trim().StartsWith("<"))
                {
                    await DisplayAlert("Error", "Unexpected server response. Please check the API.", "OK");
                    return;
                }

                var jsonResponse = JsonSerializer.Deserialize<LoginResponse>(result);

                if (jsonResponse != null && jsonResponse.status == "success")
                {
                    Preferences.Set("UserId", jsonResponse.user_id);
                    await SecureStorage.SetAsync("UserId", jsonResponse.user_id.ToString());
                    await DisplayAlert("Success", "Login successful!", "OK");
                    Application.Current.MainPage = new MainMenuPage();
                }
                else
                {
                    await DisplayAlert("Login Failed", jsonResponse?.message ?? "Unknown error occurred.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while logging in.", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private class LoginResponse
        {
            public string status { get; set; }
            public int user_id { get; set; }
            public string message { get; set; }
        }
    }
}
