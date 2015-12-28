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
        protected ContentTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository)
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
        /// Creates a new content type using the uJet content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeConstructor">The content type constructor.</param>
        /// <param name="jetContentType">The uJet content type.</param>
        /// <returns>The created content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeConstructor" />, or <paramref name="jetContentType" /> are <c>null</c>.</exception>
        protected IContentTypeBase CreateContentType<T>(
            Func<IContentTypeBase> contentTypeConstructor,
            ContentType<T> jetContentType)
            where T : ContentTypeAttribute
        {
            if (contentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeConstructor));
            }

            if (jetContentType == null)
            {
                throw new ArgumentNullException(nameof(jetContentType));
            }

            var contentType = CreateBaseContentType(contentTypeConstructor, jetContentType);

            contentType.AllowedAsRoot = jetContentType.AllowedAsRoot;

            if (!string.IsNullOrWhiteSpace(jetContentType.Thumbnail))
            {
                contentType.Thumbnail = jetContentType.Thumbnail;
            }

            SynchronizePropertyTypes(contentType, jetContentType.Properties);

            return contentType;
        }

        /// <summary>
        /// Updates the content type to match the uJet content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentTypeConstructor">The content type constructor.</param>
        /// <param name="jetContentType">The uJet content type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, <paramref name="contentTypeConstructor" />, or <paramref name="jetContentType" /> are <c>null</c>.</exception>
        protected void UpdateContentType<T>(
            IContentTypeBase contentType,
            Func<IContentTypeBase> contentTypeConstructor,
            ContentType<T> jetContentType)
            where T : ContentTypeAttribute
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeConstructor));
            }

            if (jetContentType == null)
            {
                throw new ArgumentNullException(nameof(jetContentType));
            }

            UpdateBaseContentType(contentType, contentTypeConstructor, jetContentType);

            var defaultContentType = contentTypeConstructor();

            contentType.AllowedAsRoot = jetContentType.AllowedAsRoot;
            contentType.Thumbnail = !string.IsNullOrWhiteSpace(jetContentType.Thumbnail) ? jetContentType.Thumbnail : defaultContentType.Thumbnail;

            SynchronizePropertyTypes(contentType, jetContentType.Properties);
        }

        /// <summary>
        /// Sets the content type composition.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="jetContentTypes">The uJet content types.</param>
        /// <param name="jetContentTypeConstructor">The uJet content type constructor.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, <paramref name="jetContentTypes" />, or <paramref name="jetContentTypeConstructor" /> are <c>null</c>.</exception>
        protected void SetComposition<T>(
            IContentTypeBase[] contentTypes,
            ContentType<T>[] jetContentTypes,
            Func<Type, ContentType<T>> jetContentTypeConstructor)
            where T : ContentTypeAttribute
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (jetContentTypes == null)
            {
                throw new ArgumentNullException(nameof(jetContentTypes));
            }

            if (jetContentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(jetContentTypeConstructor));
            }

            foreach (var jetContentType in jetContentTypes.Where(jct => jct.Composition.Count > 1))
            {
                SetComposition(contentTypes, jetContentType, jetContentTypeConstructor);
            }
        }

        /// <summary>
        /// Sets the allowed content types.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="jetContentTypes">The uJet content types.</param>
        /// <param name="jetContentTypeConstructor">The uJet content type constructor.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, <paramref name="jetContentTypes" />, or <paramref name="jetContentTypeConstructor" /> are <c>null</c>.</exception>
        protected void SetAllowedContentTypes<T>(
            IContentTypeBase[] contentTypes,
            ContentType<T>[] jetContentTypes,
            Func<Type, ContentType<T>> jetContentTypeConstructor)
            where T : ContentTypeAttribute
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (jetContentTypes == null)
            {
                throw new ArgumentNullException(nameof(jetContentTypes));
            }

            if (jetContentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(jetContentTypeConstructor));
            }

            foreach (var jetContentType in jetContentTypes.Where(jct => jct.AllowedChildNodeTypes.Any()))
            {
                var contentType = GetBaseContentType(contentTypes, jetContentType);

                if (contentType == null)
                {
                    continue;
                }

                contentType.AllowedContentTypes = GetAllowedChildNodeContentTypes(contentTypes, jetContentType.AllowedChildNodeTypes, jetContentTypeConstructor);

                if (jetContentType.Type.IsDocumentType())
                {
                    ContentTypeService.Save((IContentType)contentType);

                    continue;
                }

                if (jetContentType.Type.IsMediaType())
                {
                    ContentTypeService.Save((IMediaType)contentType);
                }
            }
        }

        /// <summary>
        /// Gets the allowed child node content types.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="allowedChildNodeTypes">The allowed child node types.</param>
        /// <param name="jetContentTypeConstructor">The uJet content type constructor.</param>
        /// <returns>The allowed child node content types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, <paramref name="allowedChildNodeTypes" />, or <paramref name="jetContentTypeConstructor" /> are <c>null</c>.</exception>
        private IEnumerable<ContentTypeSort> GetAllowedChildNodeContentTypes<T>(
            IContentTypeBase[] contentTypes,
            IEnumerable<Type> allowedChildNodeTypes,
            Func<Type, ContentType<T>> jetContentTypeConstructor)
            where T : ContentTypeAttribute
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (allowedChildNodeTypes == null)
            {
                throw new ArgumentNullException(nameof(allowedChildNodeTypes));
            }

            if (jetContentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(jetContentTypeConstructor));
            }

            var childNodeContentTypes = new List<ContentTypeSort>();

            foreach (var allowedChildNodeType in allowedChildNodeTypes)
            {
                var contentType = GetBaseContentType(contentTypes, jetContentTypeConstructor(allowedChildNodeType));

                if (contentType == null)
                {
                    continue;
                }

                childNodeContentTypes.Add(new ContentTypeSort(new Lazy<int>(() => contentType.Id), childNodeContentTypes.Count, contentType.Alias));
            }

            return childNodeContentTypes;
        }

        private void SetComposition<T>(
            IContentTypeBase[] contentTypes,
            ContentType<T> jetContentType,
            Func<Type, ContentType<T>> jetContentTypeConstructor)
            where T : ContentTypeAttribute
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (jetContentType == null)
            {
                throw new ArgumentNullException(nameof(jetContentType));
            }

            if (jetContentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(jetContentTypeConstructor));
            }

            var contentType = GetBaseContentType(contentTypes, jetContentType) as IContentTypeComposition;

            if (contentType == null)
            {
                return;
            }

            var composition = new List<Type>(jetContentType.Composition.Keys);

            // We remove the current type from the composition as it's not a composition of itself.
            composition.Remove(jetContentType.Type);

            var compositionContentTypes = composition.Select(type => GetBaseContentType(contentTypes, jetContentTypeConstructor(type)) as IContentTypeComposition).Where(ct => ct != null).ToArray();

            contentType = SetComposition(contentType, compositionContentTypes);

            if (jetContentType.Type.IsDocumentType())
            {
                ContentTypeService.Save((IContentType)contentType);

                return;
            }

            if (jetContentType.Type.IsMediaType())
            {
                ContentTypeService.Save((IMediaType)contentType);
            }
        }

        private IContentTypeComposition SetComposition(
            IContentTypeComposition contentType,
            IContentTypeComposition[] compositionContentTypes)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (compositionContentTypes == null)
            {
                throw new ArgumentNullException(nameof(compositionContentTypes));
            }

            var types = new List<IContentTypeComposition>(compositionContentTypes);

            foreach (var compositionContentType in contentType.ContentTypeComposition)
            {
                var type = compositionContentTypes.SingleOrDefault(cct => cct.Id == compositionContentType.Id);

                if (type != null)
                {
                    // The content type is already set as a composition content type.
                    types.Remove(type);

                    continue;
                }

                contentType.RemoveContentType(compositionContentType.Alias);
            }

            foreach (var compositionContentType in types)
            {
                // Add content types not already set as composition content types.
                contentType.AddContentType(compositionContentType);
            }

            return contentType;
        }
    }
}