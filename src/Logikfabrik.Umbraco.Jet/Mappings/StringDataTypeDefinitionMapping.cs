// <copyright file="StringDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="StringDataTypeDefinitionMapping" /> class for type <see cref="string" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class StringDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <inheritdoc />
        public override DefaultDataTypeDefinition DefaultDataTypeDefinition => DefaultDataTypeDefinition.Textstring;

        /// <inheritdoc />
        public override Type[] SupportedTypes => new[] { typeof(string) };
    }
}
