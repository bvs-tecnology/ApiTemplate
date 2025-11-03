using Domain.Common.InputModel;
using Domain.Entities.Dtos;
using Domain.Entities.Dtos.PushNotification;

namespace Domain.Interfaces.Services;

public interface IPushService
{
    Task Subscribe(PushSubscriptionInputModel inputModel, Guid userId);
    Task Unsubscribe(Guid userId);
    Task SendPush(SendPushInputModel inputModel);
}