// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    /// <summary>
    /// Base class for all default data type definition mappings.
    /// </summary>
    /// <remarks>You can create your own data type definition mapping by implementing the IDataTypeDefinitionMapping interface.</remarks>
    public abstract class DataTypeDefinitionMapping : IDataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets a data type definition with the given ID.
        /// </summary>
        /// <param name="dataTypeService">The data type servie to use.</param>
        /// <param name="id">ID of the data type definition to get.</param>
        /// <returns>A data type definition.</returns>
        protected IDataTypeDefinition GetDefinition(IDataTypeService dataTypeService, int id)
        {
            if (dataTypeService == null)
                throw new ArgumentNullException("dataTypeService");

            return dataTypeService.GetDataTypeDefinitionById(id);
        }

        /// <summary>
        /// Gets the types supported by this data type definition mapping.
        /// </summary>
        protected abstract Type[] SupportedTypes { get; }

        /// <summary>
        /// Gets whether or not this definition mapping can map the given type to a data type definition.
        /// </summary>
        /// <param name="fromType">The type to map to a data type definition.</param>
        /// <returns>True if the type can be mapped using this mapping; otherwise false.</returns>
        public bool CanMapToDefinition(Type fromType)
        {
            return SupportedTypes.Contains(fromType);
        }

        /// <summary>
        /// Maps the given type to a data type definition.
        /// </summary>
        /// <param name="fromType">The type to map to a data type definition.</param>
        /// <returns>A mapped data type definition.</returns>
        public abstract IDataTypeDefinition GetMappedDefinition(Type fromType);
    }
}
