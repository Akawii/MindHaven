using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

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
        // Definir a animação de respiração, que alterna entre as fases.
        var scaleIn = new Animation(v => BreathingBubble.Scale = v, 1, 1.5);
        var scaleOut = new Animation(v => BreathingBubble.Scale = v, 1.5, 1);

        // Atualizar o texto conforme a fase da respiração
        var breathingTextAnimation = new Animation(v =>
        {
            if (v <= 4.0 / 14.0)
                BreathingText.Text = "Segure...";
            else if (v <= 8.0 / 14.0)
                BreathingText.Text = "Expire...";
            else
                BreathingText.Text = "Inspire...";
        }, 0, 1);

        // Combina a animação da bolha com o texto de respiração.
        var breathingAnimation = new Animation();
        breathingAnimation.Add(0.0, 4.0 / 14.0, scaleIn);     // Inspire (0s–4s)
        breathingAnimation.Add(4.0 / 14.0, 8.0 / 14.0, scaleOut); // Segure (4s–8s)
        breathingAnimation.Add(8.0 / 14.0, 1.0, scaleIn);     // Expire (8s–14s)
        breathingAnimation.Add(0, 1, breathingTextAnimation);  // Texto dinâmico

        // Duração total: 14 segundos (14000ms)
        breathingAnimation.Commit(this, "BreathingInOut", length: 14000, repeat: () => true);
    }

    private async void OnMenuButtonClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new MainMenuPage();
    }

    private async void OnSendEmergencyMessage(object sender, EventArgs e)
    {
        // Lógica de envio da mensagem de emergência.
    }
}
