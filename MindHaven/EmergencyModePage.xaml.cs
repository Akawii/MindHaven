using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace MindHaven;

public partial class EmergencyModePage : ContentPage
{
    public EmergencyModePage()
    {
        InitializeComponent();
        StartBreathingAnimation();
    }

    private void StartBreathingAnimation()
    {
        var scaleIn = new Animation(v => BreathingBubble.Scale = v, 1, 1.5);
        var scaleOut = new Animation(v => BreathingBubble.Scale = v, 1.5, 1);

        var breathingAnimation = new Animation();

        // Fase: Inspire
        breathingAnimation.Add(0.0, 4.0 / 16.0, scaleIn);
        breathingAnimation.Add(0.0, 4.0 / 16.0, new Animation(v => UpdateRobotFace("robot_inhale.png")));

        // Fase: Segure
        breathingAnimation.Add(4.0 / 16.0, 8.0 / 16.0, new Animation(v => UpdateRobotFace("robot_hold.png")));

        // Fase: Expire
        breathingAnimation.Add(8.0 / 16.0, 1.0, scaleOut);
        breathingAnimation.Add(8.0 / 16.0, 1.0, new Animation(v => UpdateRobotFace("robot_exhale.png")));

        breathingAnimation.Commit(this, "BreathingInOut", length: 16000, repeat: () => true);
    }

    private void UpdateRobotFace(string imageFile)
    {
       
    }

    private async void OnMenuButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new MainMenuPage();
    }

    private async void OnSendEmergencyMessage(object sender, EventArgs e)
    {
        int userId = Preferences.Get("UserId", 0);

        var payload = new
        {
            user_id = userId,
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            using var client = new HttpClient();
            var response = await client.PostAsync("http://172.20.10.2/mindhaven/send_emergency_message.php", content);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Enviado", "Mensagem de emergência enviada com sucesso.", "OK");
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível enviar a mensagem.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha na conexão: {ex.Message}", "OK");
        }
    }
}
