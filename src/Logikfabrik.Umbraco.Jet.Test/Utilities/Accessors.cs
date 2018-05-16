// <copyright file="Accessors.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;

    [Flags]
    public enum Accessors
    {
        Get = 1,

        Set = 2,

        GetSet = Get | Set
    }
}
