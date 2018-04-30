// <copyright file="FloatingBinaryPointDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="FloatingBinaryPointDataTypeDefinitionMapping" /> class for types <see cref="float" /> and <see cref="double" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class FloatingBinaryPointDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <inheritdoc />
        public override DefaultDataTypeDefinition DefaultDataTypeDefinition => DefaultDataTypeDefinition.Textstring;

        /// <inheritdoc />
        public override Type[] SupportedTypes => new[]
        {
            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?)
        };
    }
}
