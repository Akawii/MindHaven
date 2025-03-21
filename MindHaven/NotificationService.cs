using System;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace MindHaven
{
    public class NotificationService
    {
        public void SendNotification(string title, string message)
        {
            var notification = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = title,
                Description = message,
                Android = new AndroidOptions
                {
                    ChannelId = "mindhaven_channel"
                }
            };

            LocalNotificationCenter.Current.Show(notification);
        }

        public void ScheduleDailyNotification(string title, string message)
        {
            TimeSpan time = new TimeSpan(12, 10, 0); // Horário fixo: 12:10
            var notifyTime = DateTime.Now.Date.Add(time);

            // Se a hora já passou hoje, agendar para amanhã
            if (notifyTime <= DateTime.Now)
            {
                notifyTime = notifyTime.AddDays(1);
            }

            var request = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = title,
                Description = message,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime,
                    RepeatType = NotificationRepeat.Daily
                },
                Android = new AndroidOptions
                {
                    ChannelId = "mindhaven_channel"
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }

        public void ScheduleTestNotification(string title, string message)
        {
            var notifyTime = DateTime.Now.AddMinutes(1); // Define para 1 minuto no futuro

            var request = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = title,
                Description = message,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime,
                    RepeatType = NotificationRepeat.No // Apenas para testar uma vez
                },
                Android = new AndroidOptions
                {
                    ChannelId = "mindhaven_channel"
                }
            };

            LocalNotificationCenter.Current.Show(request);
        }
    }
}
