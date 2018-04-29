// <copyright file="DataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Linq;

    /// <summary>
    /// The <see cref="DataTypeDefinitionMapping" /> class. Base class for data type definition mappings.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class DataTypeDefinitionMapping : IDataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        protected abstract Type[] SupportedTypes { get; }

        /// <inheritdoc />
        public bool CanMapToDefinition(Type fromType)
        {
            return SupportedTypes.Contains(fromType);
        }
    }
}
