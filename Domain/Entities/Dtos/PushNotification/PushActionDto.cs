using System.Text.Json.Serialization;
using Domain.Entities.Constants;

namespace Domain.Entities.Dtos.PushNotification;

public class PushActionDto(string action, string title)
{
    public PushActionDto(PushButtonDto button) : this(PushActions.Get(button.Action), button.Title) { }
    
    [JsonPropertyName("action")]
    public string Action { get; set; } = action;

    [JsonPropertyName("title")]
    public string Title { get; set; } = title;
}