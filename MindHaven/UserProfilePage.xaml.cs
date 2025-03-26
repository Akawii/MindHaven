    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Microsoft.Maui.Storage;
    using Microsoft.Maui.Controls;
    using System.IO;
    using System.Net.Http.Json;

    namespace MindHaven
    {
        public partial class UserProfilePage : ContentPage
        {
            private bool isMenuOpen = false;
            private static readonly HttpClient client = new();
            private string originalProfileImageBase64;
            private string newProfileImageBase64;

            public UserProfilePage()
            {
                InitializeComponent();
                LoadProfileData();
            
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
                    LastName.Text = userData.ContainsKey("last_name") ? userData["last_name"] : "";
                    UserName.Text = userData.ContainsKey("username") ? userData["username"] : "";
                    UserEmail.Text = userData.ContainsKey("email") ? userData["email"] : "";
                    UserBirthday.Text = userData.ContainsKey("birthday") ? userData["birthday"] : "";

                    if (userData.ContainsKey("profile_picture_base64") && !string.IsNullOrEmpty(userData["profile_picture_base64"]))
                    {
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(userData["profile_picture_base64"]);
                            ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
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

        private async void OnMenuButtonClicked(object sender, EventArgs e)
            {
                if (isMenuOpen)
                {
                    await CloseMenu();
                }
                else
                {
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

            private async void OnChoosePictureClicked(object sender, EventArgs e)
            {
                try
                {
                    var catApiResponse = await client.GetStringAsync("https://api.thecatapi.com/v1/images/search");
                    var catImages = JsonSerializer.Deserialize<CatApiResponse[]>(catApiResponse);
                    if (catImages?.Length > 0)
                    {
                        var imageUrl = catImages[0].Url;
                        var imageBytes = await client.GetByteArrayAsync(imageUrl);
                        newProfileImageBase64 = Convert.ToBase64String(imageBytes);
                        ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", "Erro ao carregar imagem: " + ex.Message, "OK");
                }
            }
        


        private async void OnSaveClicked(object sender, EventArgs e)
        {
            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                await DisplayAlert("Erro", "Usuário não está logado.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(newProfileImageBase64))
            {
                newProfileImageBase64 = originalProfileImageBase64;
            }

            var updatedData = new
            {
                user_id = userId,
                first_name = FirstName.Text,
                last_name = LastName.Text,
                username = UserName.Text,
                email = UserEmail.Text,
                birthday = UserBirthday.Text,
                profile_picture = newProfileImageBase64  // Update this line to match PHP's expectation
            };

            try
            {
                var response = await client.PostAsJsonAsync("http://172.20.10.2/mindhaven/update_user.php", updatedData);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", "Request failed: {response.StatusCode}\n{jsonResponse}", "OK");
                    return;
                }

                var result = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

                if (result != null && result.Success)
                {
                    await DisplayAlert("Sucesso", "Informações atualizadas com sucesso!", "OK");
                    originalProfileImageBase64 = newProfileImageBase64;
                }
                else
                {
                    await DisplayAlert("Erro", result?.Message ?? "Falha ao atualizar as informações.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Erro ao atualizar os dados: " + ex.Message, "OK");
            }
        }
    }

        public class UserData
        {
            [JsonPropertyName("first_name")]
            public string FirstName { get; set; }

            [JsonPropertyName("last_name")]
            public string LastName { get; set; }

            [JsonPropertyName("birthday")]
            public string Birthday { get; set; }

            [JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("profile_picture")]
            public string ProfileImageBase64 { get; set; }
        }
        public class CatApiResponse
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }
        }

        public class ApiResponse
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }