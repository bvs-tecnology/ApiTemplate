using System.Text.Json.Serialization;
using Domain.Entities.Constants;
using Domain.Entities.Enums;

namespace Domain.Entities.Dtos.PushNotification;

public class PushOnActionClickDto
{
    public PushOnActionClickDto(List<PushButtonDto> buttons)
    {
        Default = GetActionOperation(buttons, EPushAction.Default) ?? new PushActionOperationDto(PushOperations.FocusLastFocusedOrOpen);
        Foo = GetActionOperation(buttons, EPushAction.Foo);
        Bar = GetActionOperation(buttons, EPushAction.Bar);
    }
    [JsonPropertyName(PushActions.Default)]
    public PushActionOperationDto Default { get; set; }

    [JsonPropertyName(PushActions.Foo)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PushActionOperationDto? Foo { get; set; }

    [JsonPropertyName(PushActions.Bar)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PushActionOperationDto? Bar { get; set; }

    private PushActionOperationDto? GetActionOperation(List<PushButtonDto> buttons, EPushAction action)
    {
        var actionOperation = buttons.FirstOrDefault(x => x.Action == action);
        return actionOperation != null ? new PushActionOperationDto(actionOperation) : null;
    }
}