using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MindHaven
{
    public partial class RegisterPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler());

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            string firstName = firstNameEntry.Text?.Trim();
            string lastName = lastNameEntry.Text?.Trim();
            string birthday = birthdayEntry.Text?.Trim();
            string username = usernameEntry.Text?.Trim();
            string email = emailEntry.Text?.Trim();
            string password = passwordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(birthday) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            var registerData = new { firstName, lastName, birthday, username, email, password };
            string json = JsonConvert.SerializeObject(registerData);

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://172.20.10.2/mindhaven/register.php", content);
                string responseData = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Server Error", $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}\n{responseData}", "OK");
                    return;
                }

                var result = JsonConvert.DeserializeObject<dynamic>(responseData);
                if (result != null && result.status == "success")
                {
                    await DisplayAlert("Success", "Account created successfully!", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    string errorMessage = result?.message ?? "Unknown error occurred";
                    await DisplayAlert("Error", errorMessage, "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                await DisplayAlert("Connection Error", "Could not connect to the server. Please check your internet connection and try again.\n" + httpEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Unexpected Error", "An unexpected error occurred: " + ex.Message, "OK");
            }
        }
    }
}