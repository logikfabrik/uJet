//----------------------------------------------------------------------------------
// <copyright file="DataTypeDefinitionMapping.cs" company="Logikfabrik">
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
    using System.Linq;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DataTypeDefinitionMapping" /> class.
    /// </summary>
    public abstract class DataTypeDefinitionMapping : IDataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        protected abstract Type[] SupportedTypes { get; }

        /// <summary>
        /// Determines whether this instance can map the specified from type to a definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can map to definition; otherwise, <c>false</c>.
        /// </returns>
        public bool CanMapToDefinition(Type fromType)
        {
            return this.SupportedTypes.Contains(fromType);
        }

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>
        /// The mapped definition.
        /// </returns>
        public abstract IDataTypeDefinition GetMappedDefinition(Type fromType);

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The definition.</returns>
        /// <exception cref="ArgumentNullException">Thrown if dataTypeService is null.</exception>
        protected IDataTypeDefinition GetDefinition(IDataTypeService dataTypeService, int id)
        {
            if (dataTypeService == null)
            {
                throw new ArgumentNullException("dataTypeService");
            }

            return dataTypeService.GetDataTypeDefinitionById(id);
        }
    }
}
