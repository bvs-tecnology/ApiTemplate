namespace Domain.SeedWork.Notification;

public interface INotification
{
    List<NotificationModel> Notifications { get; }
    bool HasNotification { get; }
    void AddNotification(string key, string message, NotificationType type);
}