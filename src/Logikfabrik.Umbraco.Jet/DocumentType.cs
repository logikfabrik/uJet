// <copyright file="DocumentType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class DocumentType : ContentType<DocumentTypeAttribute>
    {
        public DocumentType(Type type)
            : base(type)
        {
            if (!type.IsDocumentType())
            {
                throw new ArgumentException($"Type {type} is not a document type.", nameof(type));
            }

            var attribute = GetAttribute();

            Templates = GetTemplates(attribute);
            DefaultTemplate = GetDefaultTemplate(attribute);
        }

        /// <summary>
        /// Gets the default template for this document type.
        /// </summary>
        public string DefaultTemplate { get; }

        /// <summary>
        /// Gets the available templates for this document type.
        /// </summary>
        public IEnumerable<string> Templates { get; }

        /// <summary>
        /// Gets the document type default template from the given type.
        /// </summary>
        /// <param name="attribute">The document type attribute of the underlying type.</param>
        /// <returns>A document type default template (alias).</returns>
        private static string GetDefaultTemplate(DocumentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
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
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Templates;
        }
    }
}