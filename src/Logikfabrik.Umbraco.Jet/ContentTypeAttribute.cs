﻿// The MIT License (MIT)

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
    public abstract class ContentTypeAttribute : Attribute
    {
        private readonly string _name;

        /// <summary>
        /// Gets the name of this content type attribute.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets or sets the description of this content type attribute.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the icon for this content type attribute.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail for this content type attribute.
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Gets whether or not content of this content type can be created at the root of the content tree.
        /// </summary>
        public bool AllowedAsRoot { get; set; }

        /// <summary>
        /// Gets or sets the allowed child node types of this content type attribute.
        /// </summary>
        public Type[] AllowedChildNodeTypes { get; set; }

        /// <summary>
        /// Instantiates a new content type attribute.
        /// </summary>
        /// <param name="name">The name to use for the new content type attribute.</param>
        protected ContentTypeAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or white space.", "name");

            _name = name;
        }
    }
}