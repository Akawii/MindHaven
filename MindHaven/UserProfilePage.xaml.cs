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
        private static readonly HttpClient client = new();

        public UserProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private async void LoadUserData()
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
                var userData = JsonSerializer.Deserialize<UserData>(jsonResponse, options);

                if (userData != null)
                {
                    FirstName.Text = userData.FirstName;
                    LastName.Text = userData.LastName;
                    UserName.Text = userData.Username;
                    UserEmail.Text = userData.Email;
                    UserBirthday.Text = userData.Birthday;

                    if (!string.IsNullOrEmpty(userData.ProfileImageBase64))
                    {
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(userData.ProfileImageBase64);
                            ProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        }
                        catch (Exception)
                        {
                            await DisplayAlert("Erro", "Falha ao carregar a imagem do perfil.", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Erro", "Dados do usuário não encontrados.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Falha ao carregar os dados do usuário: " + ex.Message, "OK");
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

        [JsonPropertyName("profile_picture_base64")]
        public string ProfileImageBase64 { get; set; }
    }
}
