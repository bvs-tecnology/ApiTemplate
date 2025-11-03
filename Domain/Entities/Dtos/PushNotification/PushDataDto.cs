using System.Text.Json.Serialization;

namespace Domain.Entities.Dtos.PushNotification;

public class PushDataDto(List<PushButtonDto> buttons)
{
    [JsonPropertyName("onActionClick")]
    public PushOnActionClickDto OnActionClick { get; set; } = new(buttons);
}