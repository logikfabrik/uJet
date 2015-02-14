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
using Umbraco.Core.Models;

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    /// <summary>
    /// Interface for data type definition mappings. Use this interface to customize the way document type properties are mapped by Jet.
    /// </summary>
    public interface IDataTypeDefinitionMapping
    {
        /// <summary>
        /// Gets whether or not this definition mapping can map the given type to a data type definition.
        /// </summary>
        /// <param name="fromType">The type to map to a data type definition.</param>
        /// <returns>True if the type can be mapped using this mapping; otherwise false.</returns>
        bool CanMapToDefinition(Type fromType);

        /// <summary>
        /// Maps the given type to a data type definition.
        /// </summary>
        /// <param name="fromType">The type to map to a data type definition.</param>
        /// <returns>A mapped data type definition.</returns>
        IDataTypeDefinition GetMappedDefinition(Type fromType);
    }
}
