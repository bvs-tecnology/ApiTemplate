namespace Domain.Entities.Dtos;

public class PushSubscriptionDto
{
    public PushSubscriptionDto() {}
    private PushSubscriptionDto(PushSubscription subscription)
    {
        Endpoint = subscription.Endpoint;
        Auth = subscription.Auth;
        P256dh = subscription.P256dh;
    }
    public string Endpoint { get; init; }
    public string Auth { get; init; }
    public string P256dh { get; init; }

    public static implicit operator PushSubscriptionDto(PushSubscription dto) => new(dto);
}