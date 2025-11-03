namespace Infra.Utils.Configuration;

public class PushNotificationConfigs
{
    public string PublicKey { get; init; } = string.Empty;
    public string PrivateKey { get; init; } = string.Empty;
    public string MailTo { get; init; } = string.Empty;
}