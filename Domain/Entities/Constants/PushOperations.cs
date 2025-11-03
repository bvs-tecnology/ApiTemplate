using Domain.Entities.Enums;

namespace Domain.Entities.Constants;

public static class PushOperations
{
    public const string OpenWindow = "openWindow";
    public const string FocusLastFocusedOrOpen = "focusLastFocusedOrOpen";
    public const string NavigateLastFocusedOrOpen = "navigateLastFocusedOrOpen";
    public const string SendRequest = "sendRequest";

    public static string Get(EPushOperation operation)
    {
        return operation switch
        {
            EPushOperation.OpenWindow => OpenWindow,
            EPushOperation.FocusLastFocusedOrOpen => FocusLastFocusedOrOpen,
            EPushOperation.NavigateLastFocusedOrOpen => NavigateLastFocusedOrOpen,
            EPushOperation.SendRequest => SendRequest,
            _ => string.Empty
        };
    }
}