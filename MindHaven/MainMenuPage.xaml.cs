using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json; // Added for PostAsJsonAsync
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Collections.Generic;

namespace MindHaven
{
    public partial class MainMenuPage : ContentPage
    {
        private bool isMenuOpen = false;
        private bool isProfileOpen = false;
        private static readonly HttpClient client = new();

        public MainMenuPage()
        {
            InitializeComponent();
            LoadProfileData(); // Load profile data (including image) when the page initializes
        }

        private async void LoadProfileData()
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
                var response = await client.PostAsJsonAsync("http://172.20.10.2/mindhaven/get_user.php", requestData);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var userData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse, options);

                if (userData != null)
                {
                    FirstName.Text = userData.ContainsKey("first_name") ? userData["first_name"] : "";

                    if (userData.ContainsKey("profile_picture_base64") && !string.IsNullOrEmpty(userData["profile_picture_base64"]))
                    {
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(userData["profile_picture_base64"]);
                            ProfileButton.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                            ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        }
                        catch (Exception)
                        {
                            await DisplayAlert("Error", "Failed to load profile image.", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load profile data: " + ex.Message, "OK");
            }
        }
        private async void OnEmergencyModeClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new EmergencyModePage();
        }
        private void OnInfoClicked(object sender, EventArgs e)
        {
            LogoutPopupOverlay.IsVisible = false;
            LogoutPopup.IsVisible = false;

            // Reset MainPage properly
            Application.Current.MainPage = new NavigationPage(new UserProfilePage());

        }
       
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (answer)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }


        private async void OnEmergencyClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new DataUser();
        }
        

        private void OnConfirmLogout(object sender, EventArgs e)
        {
            LogoutPopupOverlay.IsVisible = false;
            LogoutPopup.IsVisible = false;

            // Reset MainPage properly
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private void OnCancelLogout(object sender, EventArgs e)
        {
            LogoutPopupOverlay.IsVisible = false;  // Hide overlay
            LogoutPopup.IsVisible = false;  // Hide popup
        }
        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            if (isProfileOpen)
            {
                await CloseProfile();
            }
            else
            {
                await CloseMenu(); // Close the menu if open
                ProfilePopup.IsVisible = true;
                await ProfilePopup.TranslateTo(0, 0, 250, Easing.CubicIn);
                isProfileOpen = true;
            }
        }

        private async void OnEmotionalDiaryClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new EmotionalDiaryPage();
        }

        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new ReportsPage();
        }

        private async void OnMenuButtonClicked(object sender, EventArgs e)
        {
            if (isMenuOpen)
            {
                await CloseMenu();
            }
            else
            {
                await CloseProfile(); // Close the profile panel if open
                MenuPopup.IsVisible = true;
                await MenuPopup.TranslateTo(0, 0, 250, Easing.CubicIn);
                isMenuOpen = true;
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

        private async Task CloseProfile()
        {
            if (isProfileOpen)
            {
                await ProfilePopup.TranslateTo(250, 0, 250, Easing.CubicOut);
                ProfilePopup.IsVisible = false;
                isProfileOpen = false;
            }
        }
    }
}