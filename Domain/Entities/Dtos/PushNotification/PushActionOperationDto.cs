using System.Text.Json.Serialization;
using Domain.Entities.Constants;

namespace Domain.Entities.Dtos.PushNotification;

public class PushActionOperationDto(string operation, string? url = null)
{
    public PushActionOperationDto(PushButtonDto button) : this(PushOperations.Get(button.Operation), button.Url) {}
    
    [JsonPropertyName("operation")]
    public string Operation { get; set; } = operation;

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; } = url;
}