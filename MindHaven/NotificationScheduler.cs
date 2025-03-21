using System;

namespace MindHaven
{
    public class NotificationScheduler
    {
        private static NotificationService _notificationService = new NotificationService();

        public static void ScheduleDailyNotification(string title, string message)
        {
            _notificationService.ScheduleDailyNotification(title, message);
        }

        public static void ScheduleTestNotification(string title, string message)
        {
            _notificationService.ScheduleTestNotification(title, message);
        }
    }
}
