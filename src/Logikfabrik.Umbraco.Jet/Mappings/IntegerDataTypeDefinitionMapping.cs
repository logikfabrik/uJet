// <copyright file="IntegerDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IntegerDataTypeDefinitionMapping" /> class for type <see cref="int" />.
    /// </summary>
    public class IntegerDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        protected override Type[] SupportedTypes => new[]
        {
            typeof(short), typeof(short?), typeof(int), typeof(int?), typeof(ushort), typeof(ushort?),
            typeof(uint), typeof(uint?)
        };

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        /// The mapped definition.
        /// </returns>
        public override IDataTypeDefinition GetMappedDefinition(Type fromType)
        {
            return !CanMapToDefinition(fromType)
                ? null
                : GetDefinition(ApplicationContext.Current.Services.DataTypeService, (int)DataTypeDefinition.Numeric);
        }
    }
}
