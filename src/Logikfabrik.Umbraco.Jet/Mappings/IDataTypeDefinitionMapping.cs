// <copyright file="IDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IDataTypeDefinitionMapping" /> interface. Implement this interface to customize definition mappings.
    /// </summary>
    public interface IDataTypeDefinitionMapping
    {
        /// <summary>
        /// Determines whether this instance can map the specified type to a definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can map to definition; otherwise, <c>false</c>.
        /// </returns>
        bool CanMapToDefinition(Type fromType);

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>The mapped definition.</returns>
        IDataTypeDefinition GetMappedDefinition(Type fromType);
    }
}
