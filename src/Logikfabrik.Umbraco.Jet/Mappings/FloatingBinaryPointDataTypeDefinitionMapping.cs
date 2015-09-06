// <copyright file="FloatingBinaryPointDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="FloatingBinaryPointDataTypeDefinitionMapping" /> class for types <see cref="float" /> and <see cref="double" />.
    /// </summary>
    public class FloatingBinaryPointDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        protected override Type[] SupportedTypes => new[] { typeof(float), typeof(float?), typeof(double), typeof(double?) };

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        /// The mapped definition.
        /// </returns>
        public override IDataTypeDefinition GetMappedDefinition(Type fromType)
        {
            // The Umbraco data model has no explicit support for floating binary point types.
            return !CanMapToDefinition(fromType)
                ? null
                : GetDefinition(ApplicationContext.Current.Services.DataTypeService, (int)DataTypeDefinition.Textstring);
        }
    }
}
