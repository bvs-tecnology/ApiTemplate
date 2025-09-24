namespace Domain.Common.InputModel;

public class PushSubscriptionInputModel
{
    public required string Endpoint { get; init; }
    public required string Auth { get; init; }
    public required string P256dh { get; init; }
}