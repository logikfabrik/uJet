// <copyright file="ContentTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Extensions;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="ContentTypeSynchronizationService" /> class.
    /// </summary>
    public abstract class ContentTypeSynchronizationService : BaseTypeSynchronizationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" /> is <c>null</c>.</exception>
        protected ContentTypeSynchronizationService(IContentTypeService contentTypeService, IContentTypeRepository contentTypeRepository)
            : base(contentTypeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            ContentTypeService = contentTypeService;
        }

        /// <summary>
        /// Gets the content type service.
        /// </summary>
        /// <value>
        /// The content type service.
        /// </value>
        protected IContentTypeService ContentTypeService { get; }

        /// <summary>
        /// Creates the content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBaseConstructor">The content type base constructor.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBaseConstructor" />, or <paramref name="contentType" /> are <c>null</c>.</exception>
        protected IContentTypeBase CreateContentType<T>(
            Func<IContentTypeBase> contentTypeBaseConstructor,
            ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (contentTypeBaseConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBaseConstructor));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var t = CreateBaseType(contentTypeBaseConstructor, contentType);

            t.AllowedAsRoot = contentType.AllowedAsRoot;

            if (contentType.Thumbnail != null)
            {
                t.Thumbnail = contentType.Thumbnail;
            }

            SynchronizePropertyTypes(t, contentType.Properties);

            return t;
        }

        /// <summary>
        /// Updates the content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="contentTypeBaseConstructor">The content type base constructor.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, <paramref name="contentTypeBaseConstructor" />, or <paramref name="contentType" /> are <c>null</c>.</exception>
        protected void UpdateContentType<T>(
            IContentTypeBase contentTypeBase,
            Func<IContentTypeBase> contentTypeBaseConstructor,
            ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (contentTypeBaseConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBaseConstructor));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            UpdateBaseType(contentTypeBase, contentTypeBaseConstructor, contentType);

            var defaultContentType = contentTypeBaseConstructor();

            contentTypeBase.AllowedAsRoot = contentType.AllowedAsRoot;
            contentTypeBase.Thumbnail = contentType.Thumbnail ?? defaultContentType.Thumbnail;

            SynchronizePropertyTypes(contentTypeBase, contentType.Properties);
        }

        /// <summary>
        /// Sets the allowed content types.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBases">The content type bases.</param>
        /// <param name="contentTypes">The content types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBases" />, or <paramref name="contentTypes" /> are <c>null</c>.</exception>
        protected void SetAllowedContentTypes<T>(
            IContentTypeBase[] contentTypeBases,
            IEnumerable<ContentType<T>> contentTypes)
            where T : ContentTypeAttribute
        {
            if (contentTypeBases == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBases));
            }

            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            foreach (var contentType in contentTypes.Where(dt => dt.AllowedChildNodeTypes.Any()))
            {
                var contentTypeBase = contentTypeBases.FirstOrDefault(ct => ct.Alias == contentType.Alias);

                if (contentTypeBase == null)
                {
                    continue;
                }

                contentTypeBase.AllowedContentTypes = GetAllowedChildNodeContentTypes(contentType.AllowedChildNodeTypes);

                if (contentType.Type.IsDocumentType())
                {
                    ContentTypeService.Save((IContentType)contentTypeBase);
                }
                else if (contentType.Type.IsMediaType())
                {
                    ContentTypeService.Save((IMediaType)contentTypeBase);
                }
            }
        }

        /// <summary>
        /// Gets the allowed child node content types.
        /// </summary>
        /// <param name="allowedChildNodeTypes">The allowed child node types.</param>
        /// <returns>The allowed child node content types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="allowedChildNodeTypes" /> is <c>null</c>.</exception>
        private IEnumerable<ContentTypeSort> GetAllowedChildNodeContentTypes(IEnumerable<Type> allowedChildNodeTypes)
        {
            if (allowedChildNodeTypes == null)
            {
                throw new ArgumentNullException(nameof(allowedChildNodeTypes));
            }

            var nodeTypes = new List<ContentTypeSort>();

            foreach (var allowedChildNodeType in allowedChildNodeTypes)
            {
                var contentType = ContentTypeService.GetContentType(allowedChildNodeType.Name.Alias());

                if (contentType == null)
                {
                    continue;
                }

                nodeTypes.Add(
                    new ContentTypeSort(
                        new Lazy<int>(() => contentType.Id), nodeTypes.Count, contentType.Alias));
            }

            return nodeTypes;
        }
    }
}