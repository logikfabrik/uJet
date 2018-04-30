// <copyright file="IDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="IDataTypeDefinitionMapping" /> interface. Implement this interface to customize definition mappings.
    /// </summary>
    public interface IDataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the default data type definition.
        /// </summary>
        /// <value>
        /// The default data type definition.
        /// </value>
        DefaultDataTypeDefinition DefaultDataTypeDefinition { get; }

        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        Type[] SupportedTypes { get; }

        /// <summary>
        /// Determines whether this instance can map the specified from type to a definition.
        /// </summary>
        /// <param name="fromType">The from type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can map to definition; otherwise, <c>false</c>.
        /// </returns>
        bool CanMapToDefinition(Type fromType);
    }
}
