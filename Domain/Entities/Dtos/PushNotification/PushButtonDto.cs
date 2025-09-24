using Domain.Entities.Enums;

namespace Domain.Entities.Dtos.PushNotification;

public class PushButtonDto(string title, EPushAction action, EPushOperation operation, string? url = null)
{
    public string Title { get; init; } = title;
    public EPushAction Action { get; init; } = action;
    public EPushOperation Operation { get; init; } = operation;
    public string? Url { get; init; } = url;
}