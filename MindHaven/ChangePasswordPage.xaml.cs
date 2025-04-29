using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MindHaven;

public partial class ChangePasswordPage : ContentPage
{
    private static readonly HttpClient client = new();

    public ChangePasswordPage()
    {
        InitializeComponent();
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        int userId = Preferences.Get("UserId", 0);
        if (userId == 0)
        {
            await DisplayAlert("Erro", "Usuário não está logado.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CurrentPassword.Text) ||
            string.IsNullOrWhiteSpace(NewPassword.Text) ||
            string.IsNullOrWhiteSpace(ConfirmNewPassword.Text))
        {
            await DisplayAlert("Erro", "Todos os campos devem ser preenchidos.", "OK");
            return;
        }

        var updatedData = new
        {
            user_id = userId,
            CurrentPassword = CurrentPassword.Text,
            NewPassword = NewPassword.Text,
            ConfirmNewPassword = ConfirmNewPassword.Text
        };

        try
        {
            Console.WriteLine("Sending: " + JsonSerializer.Serialize(updatedData));
            var response = await client.PostAsJsonAsync("http://172.20.10.2/mindhaven/update_pass.php", updatedData);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", $"Request failed: {response.StatusCode}\n{jsonResponse}", "OK");
                return;
            }

            var result = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

            if (result != null && result.Success)
            {
                await DisplayAlert("Sucesso", "Senha alterada com sucesso!", "OK");
                CurrentPassword.Text = string.Empty;
                NewPassword.Text = string.Empty;
                ConfirmNewPassword.Text = string.Empty;
                Application.Current.MainPage = new UserProfilePage();
            }
            else
            {
                await DisplayAlert("Erro", result?.Message ?? "Falha ao atualizar a senha.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Erro ao atualizar a senha: " + ex.Message, "OK");
        }
    }
    private async void OnMenuButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new UserProfilePage();
    }

    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}