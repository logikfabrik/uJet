// <copyright file="DocumentService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using System.Linq;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DocumentService" /> class.
    /// </summary>
    public class DocumentService : ContentService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        public DocumentService()
            : this(new UmbracoHelperWrapper(), Jet.TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        /// <param name="typeService">The type service.</param>
        public DocumentService(IUmbracoHelperWrapper umbracoHelperWrapper, ITypeService typeService)
            : base(umbracoHelperWrapper, typeService)
        {
        }

        /// <summary>
        /// Gets a document.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <param name="id">The document identifier.</param>
        /// <returns>A document.</returns>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a document type.</exception>
        public T GetDocument<T>(int id) where T : class, new()
        {
            if (!typeof(T).IsDocumentType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a document type.");
            }

            return GetDocument<T>(UmbracoHelper.TypedDocument(id));
        }

        /// <summary>
        /// Gets a document.
        /// </summary>
        /// <typeparam name="T">The document type.</typeparam>
        /// <param name="content">The document content.</param>
        /// <returns>A document.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a document type.</exception>
        public T GetDocument<T>(IPublishedContent content) where T : class, new()
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!typeof(T).IsDocumentType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a document type.");
            }

            return (T)GetDocument(content, typeof(T));
        }

        /// <summary>
        /// Gets a document.
        /// </summary>
        /// <param name="id">The document identifier.</param>
        /// <param name="documentTypeAlias">The document type alias.</param>
        /// <returns>A document.</returns>
        /// <exception cref="ArgumentException">Thrown if media type with alias <paramref name="documentTypeAlias" /> can not be found.</exception>
        public object GetDocument(int id, string documentTypeAlias)
        {
            var documentType = TypeService.DocumentTypes.FirstOrDefault(t => t.Name.Alias() == documentTypeAlias);

            if (documentType == null)
            {
                throw new ArgumentException(
                    $"Document type with alias {documentTypeAlias} could not be found.",
                    nameof(documentTypeAlias));
            }

            return GetDocument(UmbracoHelper.TypedDocument(id), documentType);
        }

        /// <summary>
        /// Gets a document.
        /// </summary>
        /// <param name="content">The document content.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <returns>A document.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> or <paramref name="documentType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="documentType" /> is not a document type.</exception>
        public object GetDocument(IPublishedContent content, Type documentType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (!documentType.IsDocumentType())
            {
                throw new ArgumentException($"Type {documentType} is not a document type.", nameof(documentType));
            }

            return GetContent(content, documentType);
        }
    }
}