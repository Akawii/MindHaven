using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
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
            LoadProfileData(); // Carregar dados e imagem do perfil ao iniciar a página
        }

        private async void LoadProfileData()
        {
            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                await DisplayAlert("Erro", "Usuário não está logado.", "OK");
                return;
            }

            try
            {
                var requestData = new { user_id = userId };
                var response = await client.PostAsJsonAsync("http://172.20.10.2/mindhaven/get_user.php", requestData);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var userData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse, options);

                if (userData != null && userData.TryGetValue("status", out var status) && status == "success")
                {
                    FirstName.Text = userData.ContainsKey("first_name") ? userData["first_name"] : "";

                    if (userData.TryGetValue("profile_picture_base64", out string base64Image) && !string.IsNullOrEmpty(base64Image))
                    {
                        try
                        {
                            Console.WriteLine($"Base64 Image Length: {base64Image.Length}");

                            byte[] imageBytes = Convert.FromBase64String(base64Image);

                            if (imageBytes != null && imageBytes.Length > 0)
                            {
                                Stream imageStream1 = new MemoryStream(imageBytes);
                                Stream imageStream2 = new MemoryStream(imageBytes);

                                // Use a clone of the stream for each Image control
                                ProfileButton.Source = ImageSource.FromStream(() => imageStream1);
                                ProfileImage.Source = ImageSource.FromStream(() => imageStream2);
                            }
                            else
                            {
                                Console.WriteLine("Decoded image is empty");
                                SetDefaultProfileImage();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Image decoding error: " + ex.Message);
                            await DisplayAlert("Erro", "Falha ao carregar imagem do perfil: " + ex.Message, "OK");
                            SetDefaultProfileImage();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Base64 image string missing or empty");
                        SetDefaultProfileImage();
                    }
                }
                else
                {
                    string errorMsg = userData != null && userData.ContainsKey("message") ? userData["message"] : "Erro ao carregar dados do usuário.";
                    await DisplayAlert("Erro", errorMsg, "OK");
                    SetDefaultProfileImage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                await DisplayAlert("Erro", "Falha ao carregar dados do perfil: " + ex.Message, "OK");
                SetDefaultProfileImage();
            }
        }


        private void SetDefaultProfileImage()
        {
            ProfileButton.Source = "default_profile.png";
            ProfileImage.Source = "default_profile.png";
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

            Application.Current.MainPage = new NavigationPage(new UserProfilePage());
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Logout", "Tem certeza que deseja sair?", "Sim", "Não");
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
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private void OnCancelLogout(object sender, EventArgs e)
        {
            LogoutPopupOverlay.IsVisible = false;
            LogoutPopup.IsVisible = false;
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            if (isProfileOpen)
            {
                await CloseProfile();
            }
            else
            {
                await CloseMenu();
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
                await CloseProfile();
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
