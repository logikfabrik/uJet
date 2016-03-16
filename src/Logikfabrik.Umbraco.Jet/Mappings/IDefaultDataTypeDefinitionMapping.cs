// <copyright file="IDefaultDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IDefaultDataTypeDefinitionMapping" /> interface.
    /// </summary>
    public interface IDefaultDataTypeDefinitionMapping
    {
        /// <summary>
        /// Determines whether this instance can map to definition.
        /// </summary>
        /// <param name="uiHint">A UI hint.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can map to definition; otherwise, <c>false</c>.
        /// </returns>
        bool CanMapToDefinition(string uiHint, Type fromType);

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="uiHint">A UI hint.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>The mapped definition.</returns>
        IDataTypeDefinition GetMappedDefinition(string uiHint, Type fromType);
    }
}
