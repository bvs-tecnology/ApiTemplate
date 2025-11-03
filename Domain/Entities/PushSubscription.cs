using Domain.Common.InputModel;

namespace Domain.Entities;

public class PushSubscription : BaseEntity
{
    public PushSubscription() {}
    public PushSubscription(PushSubscriptionInputModel inputModel, Guid userId) : base(userId)
    {
        Endpoint = inputModel.Endpoint;
        Auth = inputModel.Auth;
        P256dh = inputModel.P256dh;
    }
    public string Endpoint { get; init; }
    public string Auth { get; private set; }
    public string P256dh { get; private set; }

    public void Update(PushSubscriptionInputModel inputModel)
    {
        Auth = inputModel.Auth;
        P256dh = inputModel.P256dh;
        base.Update();   
    }
}