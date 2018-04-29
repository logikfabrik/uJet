// <copyright file="IntegerDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="IntegerDataTypeDefinitionMapping" /> class for types <see cref="short" /> and <see cref="int" />, signed and unsigned.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class IntegerDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <inheritdoc />
        protected override Type[] SupportedTypes => new[]
        {
            typeof(short),
            typeof(short?),
            typeof(int),
            typeof(int?),
            typeof(ushort),
            typeof(ushort?),
            typeof(uint),
            typeof(uint?)
        };
    }
}
