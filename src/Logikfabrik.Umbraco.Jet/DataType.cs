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

using Logikfabrik.Umbraco.Jet.Extensions;
using System;
using System.Reflection;

namespace Logikfabrik.Umbraco.Jet
{
    public class DataType
    {
        private readonly string _name;
        private readonly string _editor;
        private readonly Type _type;

        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the editor for this data type.
        /// </summary>
        public string Editor { get { return _editor; } }

        /// <summary>
        /// Gets the database type for this data type.
        /// </summary>
        public Type Type { get { return _type; } }

        public DataType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsDataType())
                throw new ArgumentException("Type is not a data type.", "type");

            _name = GetName(type);

            var attribute = type.GetCustomAttribute<DataTypeAttribute>();

            _editor = GetEditor(attribute);
            _type = GetType(attribute);
        }

        /// <summary>
        /// Gets the data type name from the given type.
        /// </summary>
        /// <param name="type">The underlying type.</param>
        /// <returns>A data type name.</returns>
        private static string GetName(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.Name;
        }

        /// <summary>
        /// Gets the data type editor from the given type.
        /// </summary>
        /// <param name="attribute">The data type attribute of the underlying type.</param>
        /// <returns>A data type editor.</returns>
        private static string GetEditor(DataTypeAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            return attribute.Editor;
        }

        /// <summary>
        /// Gets the data type type from the given type.
        /// </summary>
        /// <param name="attribute">The data type attribute of the underlying type.</param>
        /// <returns>A data type type.</returns>
        private static Type GetType(DataTypeAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            return attribute.Type;
        }
    }
}