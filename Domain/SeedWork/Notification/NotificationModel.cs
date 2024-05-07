namespace Domain.SeedWork.Notification;

public class NotificationModel (
    string key,
    string message,
    NotificationType type
)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Key { get; private set; } = key;
    public string Message { get; private set; } = message;
    public NotificationType Type { get; set; } = type;
}