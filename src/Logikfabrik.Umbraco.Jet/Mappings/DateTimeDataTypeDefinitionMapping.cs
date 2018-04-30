// <copyright file="DateTimeDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="DateTimeDataTypeDefinitionMapping" /> class for type <see cref="DateTime" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DateTimeDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <inheritdoc />
        public override DefaultDataTypeDefinition DefaultDataTypeDefinition => DefaultDataTypeDefinition.DatePicker;

        /// <inheritdoc />
        public override Type[] SupportedTypes => new[]
        {
            typeof(DateTime),
            typeof(DateTime?)
        };
    }
}
