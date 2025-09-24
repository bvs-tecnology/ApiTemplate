using Domain.Entities.Enums;

namespace Domain.Entities.Constants;

public static class PushActions
{
    public const string Default = "default";
    public const string Foo = "foo";
    public const string Bar = "bar";

    public static string Get(EPushAction action)
    {
        return action switch
        {
            EPushAction.Default => Default,
            EPushAction.Foo => Foo,
            EPushAction.Bar => Bar,
            _ => string.Empty
        };
    }
}