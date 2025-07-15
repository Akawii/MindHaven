using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

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
                await DisplayAlert("Erro", "Email e senha não podem estar vazios.", "OK");
                return;
            }

            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("email", email),
        new KeyValuePair<string, string>("password", password)
    });

            try
            {
                var response = await client.PostAsync("https://mindhaven.pt/login.php", content);
                string result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Resposta do servidor: {result}"); // Log raw response
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erro", "Falha ao conectar ao servidor.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(result) || result.Trim().StartsWith("<"))
                {
                    await DisplayAlert("Erro", "Resposta inesperada do servidor. Verifique o PHP.", "OK");
                    return;
                }

                var jsonResponse = JsonSerializer.Deserialize<LoginResponse>(result);
                if (jsonResponse == null)
                {
                    Console.WriteLine("Deserialization failed. Raw response: " + result);
                    await DisplayAlert("Erro", "Falha ao processar a resposta JSON.", "OK");
                    return;
                }

                if (jsonResponse.status == "success")
                {
                    Preferences.Set("UserId", jsonResponse.user_id);
                    await SecureStorage.SetAsync("UserId", jsonResponse.user_id.ToString());
                    await DisplayAlert("Sucesso", "Login realizado com sucesso!", "OK");
                    Application.Current.MainPage = new MainMenuPage();
                }
                else
                {
                    await DisplayAlert("Falha no Login", jsonResponse.message ?? "Erro desconhecido.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex.Message}");
                await DisplayAlert("Erro", $"Ocorreu um erro ao fazer login: {ex.Message}", "OK");
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
