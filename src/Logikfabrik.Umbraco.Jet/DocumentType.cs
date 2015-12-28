// <copyright file="DocumentType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// The <see cref="DocumentType" /> class.
    /// </summary>
    public class DocumentType : ContentType<DocumentTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="type" /> is not a document type.</exception>
        public DocumentType(Type type)
            : base(type)
        {
            if (!type.IsDocumentType())
            {
                throw new ArgumentException($"Type {type} is not a document type.", nameof(type));
            }

            var attribute = type.GetCustomAttribute<DocumentTypeAttribute>();

            Templates = GetTemplates(attribute);
            DefaultTemplate = GetDefaultTemplate(attribute);
        }

        /// <summary>
        /// Gets the default template.
        /// </summary>
        /// <value>
        /// The default template.
        /// </value>
        public string DefaultTemplate { get; }

        /// <summary>
        /// Gets the templates.
        /// </summary>
        /// <value>
        /// The templates.
        /// </value>
        public IEnumerable<string> Templates { get; }

        /// <summary>
        /// Gets the composition.
        /// </summary>
        /// <returns>
        /// The composition.
        /// </returns>
        protected override IDictionary<Type, IEnumerable<Type>> GetComposition()
        {
            return GetComposition(TypeExtensions.IsDocumentType);
        }

        /// <summary>
        /// Gets the default template.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The default template.</returns>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static string GetDefaultTemplate(DocumentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.DefaultTemplate;
        }

        /// <summary>
        /// Gets the templates.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The templates.</returns>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static IEnumerable<string> GetTemplates(DocumentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Templates ?? new string[] { };
        }
    }
}