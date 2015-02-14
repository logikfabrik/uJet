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

namespace Logikfabrik.Umbraco.Jet
{
    [AttributeUsage(
        AttributeTargets.Class,
        AllowMultiple = false,
        Inherited = false)]
    public class DataTypeAttribute : Attribute
    {
        private readonly Type _type;
        private readonly string _editor;

        /// <summary>
        /// Gets the name of this data type attribute.
        /// </summary>
        public string Editor { get { return _editor; } }

        /// <summary>
        /// Gets the type of this data type attribute.
        /// </summary>
        public Type Type { get { return _type; } }

        public DataTypeAttribute(Type type, string editor)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (string.IsNullOrWhiteSpace(editor))
                throw new ArgumentException("Editor cannot be null or white space.", "editor");

            _type = type;
            _editor = editor;
        }
    }
}
