// <copyright file="DataType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Models
{
    using System.Collections.Generic;

    [DataType(
        "443c08bc-e314-4e2d-ba26-66c6a565ad60",
        typeof(object),
        "Editor")]
    public class DataType
    {
        public Dictionary<string, string> PreValues => new Dictionary<string, string>
        {
            { "PreValue0", "Value0" },
            { "PreValue1", "Value1" },
            { "PreValue2", "Value2" }
        };
    }
}
