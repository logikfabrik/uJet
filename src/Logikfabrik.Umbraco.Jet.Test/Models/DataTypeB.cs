// <copyright file="DataTypeB.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Models
{
    using System.Collections.Generic;

    [DataType(
        "3df862fa-2f69-4ed0-a28c-8b513dced7fc",
        typeof(object),
        "Editor")]
    public class DataTypeB
    {
        public Dictionary<string, string> PreValues
        {
            // ReSharper disable once ValueParameterNotUsed
            set
            {
            }
        }
    }
}