// <copyright file="IDataType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IDataType" /> interface. Optional interface for defining pre-values.
    /// </summary>
    [PublicAPI]
    public interface IDataType
    {
        /// <summary>
        /// Gets the pre-values.
        /// </summary>
        /// <value>
        /// The pre-values.
        /// </value>
        Dictionary<string, string> PreValues { get; }
    }
}
