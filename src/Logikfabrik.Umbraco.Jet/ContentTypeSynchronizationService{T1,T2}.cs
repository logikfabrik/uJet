// <copyright file="ContentTypeSynchronizationService{T1,T2}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ContentTypeSynchronizationService{T1, T2}" /> class.
    /// </summary>
    /// <typeparam name="T1">The type type.</typeparam>
    /// <typeparam name="T2">The type attribute type.</typeparam>
    public abstract class ContentTypeSynchronizationService<T1, T2> : BaseTypeSynchronizationService<T1, T2>
        where T1 : ContentType<T2>
        where T2 : ContentTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeSynchronizationService{T1, T2}" /> class.
        /// </summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        protected ContentTypeSynchronizationService(ITypeResolver typeResolver, ITypeRepository typeRepository)
            : base(typeResolver, typeRepository)
        {
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public override void Synchronize()
        {
            base.Synchronize();

            var contentTypes = GetContentTypes();

            SetAllowedContentTypes(contentTypes);
            SetComposition(contentTypes);
        }

        /// <summary>
        /// Creates a content type for the specified content type model.
        /// </summary>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <returns>
        /// The created content type.
        /// </returns>
        internal override IContentTypeBase CreateContentType(T1 contentTypeModel)
        {
            var contentType = base.CreateContentType(contentTypeModel);

            contentType.AllowedAsRoot = contentTypeModel.AllowedAsRoot;

            if (!string.IsNullOrWhiteSpace(contentTypeModel.Thumbnail))
            {
                contentType.Thumbnail = contentTypeModel.Thumbnail;
            }

            return contentType;
        }

        /// <summary>
        /// Updates the content type for the specified content type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <returns>
        /// The updated content type.
        /// </returns>
        internal override IContentTypeBase UpdateContentType(IContentTypeBase contentType, T1 contentTypeModel)
        {
            contentType = base.UpdateContentType(contentType, contentTypeModel);

            var defaultContentType = GetContentType();

            contentType.AllowedAsRoot = contentTypeModel.AllowedAsRoot;
            contentType.Thumbnail = !string.IsNullOrWhiteSpace(contentTypeModel.Thumbnail) ? contentTypeModel.Thumbnail : defaultContentType.Thumbnail;

            return contentType;
        }

        /// <summary>
        /// Gets a content type model for the specified model type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>A content type model for the specified model type.</returns>
        protected abstract T1 GetContentTypeModel(Type modelType);

        /// <summary>
        /// Sets the content type composition.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" /> is <c>null</c>.</exception>
        private void SetComposition(IContentTypeBase[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            foreach (var contentTypeModel in ContentTypeModels.Where(ctm => ctm.Composition.Count > 1))
            {
                SetComposition(contentTypes, contentTypeModel);
            }
        }

        private void SetComposition(IContentTypeBase[] contentTypes, T1 contentTypeModel)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            var contentType = Resolver.ResolveType<T1, T2>(contentTypeModel, contentTypes) as IContentTypeComposition;

            if (contentType == null)
            {
                return;
            }

            var composition = new List<Type>(contentTypeModel.Composition.Keys);

            // We remove the current type from the composition as it's not a composition of itself.
            composition.Remove(contentTypeModel.Type);

            var compositionContentTypes = composition.Select(type => Resolver.ResolveType<T1, T2>(GetContentTypeModel(type), contentTypes) as IContentTypeComposition).Where(ct => ct != null).ToArray();

            SetComposition(contentType, compositionContentTypes);

            SaveContentType(contentType);
        }

        /// <summary>
        /// Sets the composition for the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="compositionContentTypes">The composition content types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, <paramref name="compositionContentTypes" /> are <c>null</c>.</exception>
        private void SetComposition(
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
        }

        /// <summary>
        /// Sets the allowed content types.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" /> is <c>null</c>.</exception>
        private void SetAllowedContentTypes(IContentTypeBase[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            foreach (var contentTypeModel in ContentTypeModels.Where(ctm => ctm.AllowedChildNodeTypes.Any()))
            {
                var contentType = Resolver.ResolveType<T1, T2>(contentTypeModel, contentTypes);

                if (contentType == null)
                {
                    continue;
                }

                contentType.AllowedContentTypes = GetAllowedChildNodeContentTypes(contentTypes, contentTypeModel.AllowedChildNodeTypes);

                SaveContentType(contentType);
            }
        }

        /// <summary>
        /// Gets the allowed child node content types.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="allowedChildNodeTypes">The allowed child node types.</param>
        /// <returns>The allowed child node content types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, <paramref name="allowedChildNodeTypes" /> are <c>null</c>.</exception>
        private IEnumerable<ContentTypeSort> GetAllowedChildNodeContentTypes(
            IContentTypeBase[] contentTypes,
            IEnumerable<Type> allowedChildNodeTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (allowedChildNodeTypes == null)
            {
                throw new ArgumentNullException(nameof(allowedChildNodeTypes));
            }

            var childNodeContentTypes = new List<ContentTypeSort>();

            foreach (var allowedChildNodeType in allowedChildNodeTypes)
            {
                var contentType = Resolver.ResolveType<T1, T2>(GetContentTypeModel(allowedChildNodeType), contentTypes);

                if (contentType == null)
                {
                    continue;
                }

                childNodeContentTypes.Add(new ContentTypeSort(new Lazy<int>(() => contentType.Id), childNodeContentTypes.Count, contentType.Alias));
            }

            return childNodeContentTypes;
        }
    }
}