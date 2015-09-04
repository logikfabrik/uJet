//----------------------------------------------------------------------------------
// <copyright file="DocumentTypeAttribute.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class DocumentTypeAttribute : ContentTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name to use for the new document type attribute.</param>
        public DocumentTypeAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The ID to use for the new document type attribute.</param>
        /// <param name="name">The name to use for the new document type attribute.</param>
        public DocumentTypeAttribute(string id, string name)
            : base(id, name)
        {
        }

        /// <summary>
        /// Gets or sets the available templates (aliases) of this document type attribute.
        /// </summary>
        public string[] Templates { get; set; }

        /// <summary>
        /// Gets or sets the default template (alias) of this document type attribute.
        /// </summary>
        public string DefaultTemplate { get; set; }
    }
}
