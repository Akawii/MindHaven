using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace MindHaven
{
    public partial class DataUser : ContentPage
    {
        private static readonly HttpClient client = new();
        private bool isMenuOpen = false;
        public DataUser()
        {
            InitializeComponent();
            LoadUserData();
        }

        private async void OnAddContactClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AddContactoPage();
        }

        private async void LoadUserData()
        {
            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            try
            {
                var requestData = new { user_id = userId };
                var response = await client.PostAsJsonAsync("http://172.20.10.2/mindhaven/get_user_data.php", requestData);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var userData = JsonSerializer.Deserialize<UserProfileData>(jsonResponse, options);

                if (userData != null)
                {
                    FirstNameLabel.Text = $"{userData.FirstName}";
                    AgeLabel.Text = $"{userData.Age} anos";

                    ContactsStack.Children.Clear();
                    foreach (var contact in userData.EmergencyContacts)
                    {
                        ContactsStack.Children.Add(new Label
                        {
                            Text = $"{contact.Name} ({contact.Relationship}) - {contact.Phone}",
                            FontSize = 16,
                            TextColor = Colors.Black
                        });
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Failed to load user data.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }
        private async void OnMenuButtonClicked(object sender, EventArgs e)
        {
            if (MenuPopup.IsVisible)
            {
                await MenuPopup.TranslateTo(-250, 0, 250, Easing.Linear);
                MenuPopup.IsVisible = false;
            }
            else
            {
                MenuPopup.IsVisible = true;
                await MenuPopup.TranslateTo(0, 0, 250, Easing.Linear);
            }
        }


        private async Task CloseMenu()
        {
            if (isMenuOpen)
            {
                await MenuPopup.TranslateTo(-250, 0, 250, Easing.CubicOut);
                MenuPopup.IsVisible = false;
                isMenuOpen = false;
            }
        }
        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new ReportsPage();
        }

        private async void OnMainMenuClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new MainMenuPage();
        }

        private async void OnPasswordClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new ChangePasswordPage();
        }
    }

    public class UserProfileData
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("emergency_contacts")]
        public List<Contact> EmergencyContacts { get; set; }
    }

    public class Contact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("relationship")]
        public string Relationship { get; set; }
    }
}
