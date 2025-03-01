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
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            string firstName = firstNameEntry.Text;
            string lastName = lastNameEntry.Text;
            string birthday = birthdayEntry.Text;
            string username = usernameEntry.Text;
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(birthday) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }


            var registerData = new { firstName, lastName, birthday, username, email, password };
            string json = JsonConvert.SerializeObject(registerData);

            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://192.168.1.214/mindhaven/register.php", content);


                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    if (responseData.Contains("success"))
                    {
                        await DisplayAlert("Success", "Account created successfully!", "OK");
                        await Navigation.PushAsync(new LoginPage()); // Redirect to LoginPage
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to register", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Server error, please try again", "OK");
                }
            }
        }
    }
}
