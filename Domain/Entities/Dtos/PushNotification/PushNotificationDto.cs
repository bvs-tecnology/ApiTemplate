using System.Text.Json.Serialization;
using Domain.Common.InputModel;
using Domain.Entities.Constants;

namespace Domain.Entities.Dtos.PushNotification;

public class PushNotificationDto(
    string title,
    List<PushButtonDto> buttons,
    string? body = null,
    string? badge = null,
    string? tag = null,
    string? icon = null,
    string? image = null,
    string? language = null,
    bool renotify = false,
    bool requireInteraction = false,
    bool silent = false
)
{
    public PushNotificationDto(SendPushInputModel inputModel) : this(
        inputModel.Title,
        inputModel.Buttons,
        inputModel.Body,
        icon: PushConstants.Icon
    ) {}
    [JsonPropertyName("notification")]
    public NotificationDto Notification { get; set; } = new(title, buttons, body, badge, tag, icon, image, language, renotify, requireInteraction, silent);
}