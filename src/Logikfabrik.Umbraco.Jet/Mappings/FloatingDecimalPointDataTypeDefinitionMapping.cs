﻿//----------------------------------------------------------------------------------
// <copyright file="FloatingDecimalPointDataTypeDefinitionMapping.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// Default data type definition mapping for floating decimal point types.
    /// </summary>
    public class FloatingDecimalPointDataTypeDefinitionMapping : DataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the types supported by this data type definition mapping.
        /// </summary>
        protected override Type[] SupportedTypes
        {
            get { return new[] { typeof(decimal), typeof(decimal?) }; }
        }

        /// <summary>
        /// Maps the given type to a data type definition.
        /// </summary>
        /// <param name="fromType">The type to map to a data type definition.</param>
        /// <returns>A mapped data type definition.</returns>
        public override IDataTypeDefinition GetMappedDefinition(Type fromType)
        {
            // The Umbraco data model has no explicit support for floating decimal point types.
            return !this.CanMapToDefinition(fromType)
                ? null
                : this.GetDefinition(ApplicationContext.Current.Services.DataTypeService, (int)DataTypeDefinition.Textstring);
        }
    }
}
