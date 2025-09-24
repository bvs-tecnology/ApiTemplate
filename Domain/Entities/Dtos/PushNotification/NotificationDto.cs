using System.Text.Json.Serialization;

namespace Domain.Entities.Dtos.PushNotification;

public class NotificationDto(
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
    [JsonPropertyName("title")]
    public string Title { get; set; } = title;
    
    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Body { get; set; } = body;
    
    [JsonPropertyName("badge")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Badge { get; set; } = badge;
    
    [JsonPropertyName("tag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Tag { get; set; } = tag;

    [JsonPropertyName("actions")]
    public List<PushActionDto> Actions { get; set; } = buttons.Select(button => new PushActionDto(button)).ToList();

    [JsonPropertyName("data")]
    public PushDataDto Data { get; set; } = new(buttons);
    
    [JsonPropertyName("icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Icon { get; set; } = icon;
    
    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Image { get; set; } = image;
    
    [JsonPropertyName("lang")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Language { get; set; } = language;
    
    [JsonPropertyName("renotify")]
    public bool Renotify { get; set; } = renotify;
    
    [JsonPropertyName("requireInteraction")]
    public bool RequireInteraction { get; set; } = requireInteraction;
    
    [JsonPropertyName("silent")]
    public bool Silent { get; set; } = silent;
}