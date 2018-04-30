// <copyright file="BooleanDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="BooleanDataTypeDefinitionMapping" /> class for type <see cref="bool" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class BooleanDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <inheritdoc />
        public override DefaultDataTypeDefinition DefaultDataTypeDefinition => DefaultDataTypeDefinition.TrueFalse;

        /// <inheritdoc />
        public override Type[] SupportedTypes => new[]
        {
            typeof(bool),
            typeof(bool?)
        };
    }
}
