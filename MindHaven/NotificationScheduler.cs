using System;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace MindHaven
{
    public class NotificationScheduler
    {
        public static void ScheduleDailyNotification(string title, string message, TimeSpan time)
        {
            var request = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = title,
                Description = message,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.Date.Add(time), // Define horário fixo
                    RepeatType = NotificationRepeat.Daily
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