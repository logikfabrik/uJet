// <copyright file="FloatingDecimalPointDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="FloatingDecimalPointDataTypeDefinitionMapping" /> class for type <see cref="decimal" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class FloatingDecimalPointDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <inheritdoc />
        public override DefaultDataTypeDefinition DefaultDataTypeDefinition => DefaultDataTypeDefinition.Textstring;

        /// <inheritdoc />
        public override Type[] SupportedTypes => new[]
        {
            typeof(decimal),
            typeof(decimal?)
        };
    }
}
