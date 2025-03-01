using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MindHaven
{
    public partial class LoginPage : ContentPage
    {
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
                await DisplayAlert("Error", "Please enter both email and password", "OK");
                return;
            }

            var loginData = new { email, password };
            string json = JsonConvert.SerializeObject(loginData);

            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost/mindhaven/login.php", content);


                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    if (responseData.Contains("success"))
                    {
                        await DisplayAlert("Success", "Login successful!", "OK");
                        await Navigation.PushAsync(new MindHaven.MainPage()); // Redirect to MainPage
                    }
                    else
                    {
                        await DisplayAlert("Error", "Invalid login credentials", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Server error, please try again", "OK");
                }
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage()); // Redirect to RegisterPage
        }
    }
}
