using Domain.SeedWork.Constants;

namespace Domain.SeedWork.Notification
{
    public static class NotificationsWrapper
    {
        private static INotification GetContainer()
            => ServiceLocator.Container.GetService<INotification>();

        public static void AddNotification(string key, string message, NotificationType type)
            => GetContainer().AddNotification(key, message, type);

        public static void AddNotification(string message)
            => AddNotification(NotificationConstant.InvalidPropertyKey, message, NotificationType.BadRequest);

        public static bool HasNotification()
            => GetContainer().HasNotification;
    }
}
