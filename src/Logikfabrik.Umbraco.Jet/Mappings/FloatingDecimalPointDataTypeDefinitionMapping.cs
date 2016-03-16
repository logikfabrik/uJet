// <copyright file="FloatingDecimalPointDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="FloatingDecimalPointDataTypeDefinitionMapping" /> class for type <see cref="decimal" />.
    /// </summary>
    public class FloatingDecimalPointDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        protected override Type[] SupportedTypes => new[] { typeof(decimal), typeof(decimal?) };

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        /// The mapped definition.
        /// </returns>
        public override IDataTypeDefinition GetMappedDefinition(Type fromType)
        {
            // The Umbraco data model has no explicit support for floating decimal point types.
            return !CanMapToDefinition(fromType)
                ? null
                : GetDefinition(ApplicationContext.Current.Services.DataTypeService, (int)DataTypeDefinition.Textstring);
        }
    }
}
