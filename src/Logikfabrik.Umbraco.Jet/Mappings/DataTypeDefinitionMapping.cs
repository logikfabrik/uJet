// <copyright file="DataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Linq;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DataTypeDefinitionMapping" /> class.
    /// </summary>
    public abstract class DataTypeDefinitionMapping : IDataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        protected abstract Type[] SupportedTypes { get; }

        /// <summary>
        /// Determines whether this instance can map the specified from type to a definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can map to definition; otherwise, <c>false</c>.
        /// </returns>
        public bool CanMapToDefinition(Type fromType)
        {
            return SupportedTypes.Contains(fromType);
        }

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        /// The mapped definition.
        /// </returns>
        public abstract IDataTypeDefinition GetMappedDefinition(Type fromType);

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The definition.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeService" /> is <c>null</c>.</exception>
        protected IDataTypeDefinition GetDefinition(IDataTypeService dataTypeService, int id)
        {
            if (dataTypeService == null)
            {
                throw new ArgumentNullException(nameof(dataTypeService));
            }

            return dataTypeService.GetDataTypeDefinitionById(id);
        }
    }
}
