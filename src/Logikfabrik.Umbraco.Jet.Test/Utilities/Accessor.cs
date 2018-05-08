namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;

    [Flags]
    public enum Accessor
    {
        Get = 1,

        Set = 2,

        GetSet = Get | Set
    }
}
