// <copyright file="DocumentService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using EnsureThat;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DocumentService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to document models.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DocumentService : ContentService
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public DocumentService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        public DocumentService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            EnsureArg.IsNotNull(umbracoHelperWrapper);

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <summary>
        /// Gets a model for the document with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The document model type.</typeparam>
        /// <param name="id">The document identifier.</param>
        /// <returns>A model for the document with the specified identifier.</returns>
        public T GetDocument<T>(int id)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<DocumentTypeAttribute>(), nameof(T));

            return GetDocument<T>(_umbracoHelperWrapper.TypedDocument(id));
        }

        /// <summary>
        /// Gets a model for the document.
        /// </summary>
        /// <typeparam name="T">The document model type.</typeparam>
        /// <param name="content">The document content.</param>
        /// <returns>A model for the document.</returns>
        public T GetDocument<T>(IPublishedContent content)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<DocumentTypeAttribute>(), nameof(T));
            EnsureArg.IsNotNull(content);

            return (T)GetDocument(content, typeof(T));
        }

        /// <summary>
        /// Gets a model for the document.
        /// </summary>
        /// <param name="content">The document content.</param>
        /// <param name="documentModelType">The document model type.</param>
        /// <returns>A model for the document.</returns>
        public object GetDocument(IPublishedContent content, Type documentModelType)
        {
            EnsureArg.IsNotNull(content);
            EnsureArg.IsNotNull(documentModelType);
            EnsureArg.IsTrue(documentModelType.IsModelType<DocumentTypeAttribute>(), nameof(documentModelType));

            return GetMappedContent(content, documentModelType);
        }

        /// <inheritdoc />
        protected override void MapByConvention(IPublishedContent content, object model)
        {
            base.MapByConvention(content, model);

            MapProperty(model, GetPropertyName(() => content.DocumentTypeId), content.DocumentTypeId);
            MapProperty(model, GetPropertyName(() => content.DocumentTypeAlias), content.DocumentTypeAlias);
        }
    }
}