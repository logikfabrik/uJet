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

            SetInheritance(contentTypes);
            SetAllowedContentTypes(contentTypes);
            SetCompositionNodeTypes(contentTypes);
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

        private void SetInheritance(IContentTypeBase[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            Func<T1, bool> hasParent = type => type.ParentNodeType != null;

            foreach (var contentTypeModel in ContentTypeModels.Where(hasParent))
            {
                var contentType = Resolver.ResolveType<T1, T2>(contentTypeModel, contentTypes) as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                var parentContentTypeModel = GetContentTypeModel(contentTypeModel.ParentNodeType);

                if (parentContentTypeModel == null)
                {
                    continue;
                }

                var parentContentType = Resolver.ResolveType<T1, T2>(parentContentTypeModel, contentTypes) as IContentTypeComposition;

                if (parentContentType == null)
                {
                    continue;
                }

                var aliases = contentType.CompositionAliases().ToArray();

                foreach (var alias in aliases)
                {
                    contentType.RemoveContentType(alias);
                }

                contentType.ParentId = parentContentType.Id;
                contentType.AddContentType(parentContentType);

                SaveContentType(contentType);
            }
        }

        /// <summary>
        /// Sets the composition node types.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" /> is <c>null</c>.</exception>
        private void SetCompositionNodeTypes(IContentTypeBase[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            foreach (var contentTypeModel in ContentTypeModels.Where(ctm => ctm.CompositionNodeTypes.Any()))
            {
                if (contentTypeModel.ParentNodeType != null)
                {
                    // Composition and inheritance don't mix.
                    continue;
                }

                var contentType = Resolver.ResolveType<T1, T2>(contentTypeModel, contentTypes) as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                var compositionContentTypes = contentTypeModel.CompositionNodeTypes.Select(type => Resolver.ResolveType<T1, T2>(GetContentTypeModel(type), contentTypes) as IContentTypeComposition).Where(ct => ct != null).ToArray();

                var types = new List<IContentTypeComposition>(compositionContentTypes);

                var aliases = contentType.CompositionAliases().ToArray();

                foreach (var alias in aliases)
                {
                    var type = compositionContentTypes.SingleOrDefault(ct => ct.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));

                    if (type != null)
                    {
                        // The content type is already set as a composition content type.
                        types.Remove(type);

                        continue;
                    }

                    contentType.RemoveContentType(alias);
                }

                foreach (var compositionContentType in types)
                {
                    // Add content types not already set as composition content types.
                    contentType.AddContentType(compositionContentType);
                }

                SaveContentType(contentType);
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