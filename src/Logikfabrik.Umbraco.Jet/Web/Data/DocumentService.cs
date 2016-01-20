// <copyright file="DocumentService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DocumentService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to document models.
    /// </summary>
    public class DocumentService : ContentService
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        public DocumentService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="umbracoHelperWrapper" /> is <c>null</c>.</exception>
        public DocumentService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            if (umbracoHelperWrapper == null)
            {
                throw new ArgumentNullException(nameof(umbracoHelperWrapper));
            }

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <summary>
        /// Gets a model for the document with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The document model type.</typeparam>
        /// <param name="id">The document identifier.</param>
        /// <returns>A model for the document with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a document model type.</exception>
        public T GetDocument<T>(int id)
            where T : class, new()
        {
            if (!typeof(T).IsModelType<DocumentTypeAttribute>())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a document model type.");
            }

            return GetDocument<T>(_umbracoHelperWrapper.TypedDocument(id));
        }

        /// <summary>
        /// Gets a model for the document.
        /// </summary>
        /// <typeparam name="T">The document model type.</typeparam>
        /// <param name="content">The document content.</param>
        /// <returns>A model for the document.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a document model type.</exception>
        public T GetDocument<T>(IPublishedContent content)
            where T : class, new()
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!typeof(T).IsModelType<DocumentTypeAttribute>())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a document model type.");
            }

            return (T)GetDocument(content, typeof(T));
        }

        /// <summary>
        /// Gets a model for the document.
        /// </summary>
        /// <param name="content">The document content.</param>
        /// <param name="documentModelType">The document model type.</param>
        /// <returns>A model for the document.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="documentModelType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="documentModelType" /> is not a document model type.</exception>
        public object GetDocument(IPublishedContent content, Type documentModelType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (documentModelType == null)
            {
                throw new ArgumentNullException(nameof(documentModelType));
            }

            if (!documentModelType.IsModelType<DocumentTypeAttribute>())
            {
                throw new ArgumentException($"Type {documentModelType} is not a document model type.", nameof(documentModelType));
            }

            return GetMappedContent(content, documentModelType);
        }

        /// <summary>
        /// Maps content by convention.
        /// </summary>
        /// <param name="content">The content to map.</param>
        /// <param name="model">The model to map to.</param>
        protected override void MapByConvention(IPublishedContent content, object model)
        {
            base.MapByConvention(content, model);

            MapProperty(model, GetPropertyName(() => content.DocumentTypeId), content.DocumentTypeId);
            MapProperty(model, GetPropertyName(() => content.DocumentTypeAlias), content.DocumentTypeAlias);
        }
    }
}