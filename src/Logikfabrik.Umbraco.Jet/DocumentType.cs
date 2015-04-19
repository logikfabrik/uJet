//----------------------------------------------------------------------------------
// <copyright file="DocumentType.cs" company="Logikfabrik">
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
    using System.Collections.Generic;
    using Extensions;
    
    public class DocumentType : ContentType<DocumentTypeAttribute>
    {
        private readonly IEnumerable<string> templates;
        private readonly string defaultTemplate;

        public DocumentType(Type type)
            : base(type)
        {
            if (!type.IsDocumentType())
            {
                throw new ArgumentException(string.Format("Type {0} is not a document type.", type), "type");
            }

            var attribute = GetAttribute();

            this.templates = GetTemplates(attribute);
            this.defaultTemplate = GetDefaultTemplate(attribute);
        }

        /// <summary>
        /// Gets the default template for this document type.
        /// </summary>
        public string DefaultTemplate
        {
            get { return this.defaultTemplate; }
        }

        /// <summary>
        /// Gets the available templates for this document type.
        /// </summary>
        public IEnumerable<string> Templates
        {
            get { return this.templates; }
        }

        /// <summary>
        /// Gets the document type default template from the given type.
        /// </summary>
        /// <param name="attribute">The document type attribute of the underlying type.</param>
        /// <returns>A document type default template (alias).</returns>
        private static string GetDefaultTemplate(DocumentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            return attribute.DefaultTemplate;
        }

        /// <summary>
        /// Gets the document type available templates (aliases) from the given type.
        /// </summary>
        /// <param name="attribute">The document type attribute of the underlying type.</param>
        /// <returns>Document type available templates (aliases).</returns>
        private static IEnumerable<string> GetTemplates(DocumentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            return attribute.Templates;
        }
    }
}