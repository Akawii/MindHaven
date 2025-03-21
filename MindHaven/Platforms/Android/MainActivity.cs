using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace MindHaven;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Criar o canal de notificações para Android 8+ (Oreo e superior)
        var notificationChannelRequest = new NotificationChannelRequest
        {
            Id = "mindhaven_channel",
            Name = "MindHaven Alerts",
            Importance = (AndroidImportance)NotificationImportance.High // Conversão explícita de NotificationImportance para AndroidImportance
        };

        // Agora, passa uma lista contendo o canal de notificações
        LocalNotificationCenter.CreateNotificationChannels(new List<NotificationChannelRequest> { notificationChannelRequest });

        // Testar uma notificação instantânea ao iniciar o app
        var notificationService = new NotificationService();
        notificationService.SendNotification("Teste", "Isso é uma notificação de teste!");

        // Testar uma notificação agendada para 1 minuto depois
        NotificationScheduler.ScheduleTestNotification("Lembrete de Bem-Estar", "Hora de relaxar!");
    }
}
