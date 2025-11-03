using Domain.Entities.Dtos.PushNotification;

namespace Domain.Common.InputModel;

public class SendPushInputModel
{
    public string Title { get; init; }
    public string Body { get; init; }
    public List<PushButtonDto> Buttons { get; init; } = [];
}