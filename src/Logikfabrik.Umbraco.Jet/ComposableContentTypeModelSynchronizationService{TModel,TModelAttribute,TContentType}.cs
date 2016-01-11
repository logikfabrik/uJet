// <copyright file="ComposableContentTypeModelSynchronizationService{TModel,TModelAttribute,TContentType}.cs" company="Logikfabrik">
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
    /// The <see cref="ComposableContentTypeModelSynchronizationService{TModel,TModelAttribute,TContentType}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The <see cref="ComposableContentTypeModel{T}" /> type.</typeparam>
    /// <typeparam name="TModelAttribute">The <see cref="ComposableContentTypeAttribute" /> type.</typeparam>
    /// <typeparam name="TContentType">The <see cref="IContentTypeBase" /> type.</typeparam>
    public abstract class ComposableContentTypeModelSynchronizationService<TModel, TModelAttribute, TContentType> : ContentTypeModelSynchronizationService<TModel, TModelAttribute, TContentType>
        where TModel : ComposableContentTypeModel<TModelAttribute>
        where TModelAttribute : ComposableContentTypeAttribute
        where TContentType : IContentTypeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComposableContentTypeModelSynchronizationService{TModel,TModelAttribute,TContentType}" /> class.
        /// </summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        protected ComposableContentTypeModelSynchronizationService(ITypeResolver typeResolver, ITypeRepository typeRepository)
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
        /// <param name="model">The content type model.</param>
        /// <returns>
        /// The created content type.
        /// </returns>
        internal override TContentType CreateContentType(TModel model)
        {
            var contentType = base.CreateContentType(model);

            contentType.AllowedAsRoot = model.AllowedAsRoot;

            if (!string.IsNullOrWhiteSpace(model.Thumbnail))
            {
                contentType.Thumbnail = model.Thumbnail;
            }

            return contentType;
        }

        /// <summary>
        /// Updates the content type for the specified content type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="model">The content type model.</param>
        /// <returns>
        /// The updated content type.
        /// </returns>
        internal override TContentType UpdateContentType(TContentType contentType, TModel model)
        {
            contentType = base.UpdateContentType(contentType, model);

            var defaultContentType = CreateContentType();

            contentType.AllowedAsRoot = model.AllowedAsRoot;
            contentType.Thumbnail = !string.IsNullOrWhiteSpace(model.Thumbnail) ? model.Thumbnail : defaultContentType.Thumbnail;

            return contentType;
        }

        /// <summary>
        /// Gets a model for the specified model type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>A model.</returns>
        protected abstract TModel GetModel(Type modelType);

        private void SetInheritance(TContentType[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            Func<TModel, bool> hasParent = type => type.ParentNodeType != null;

            foreach (var model in Models.Where(hasParent))
            {
                var contentType = Resolver.ResolveType<TModel, TModelAttribute, TContentType>(model, contentTypes) as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                var parentContentTypeModel = GetModel(model.ParentNodeType);

                if (parentContentTypeModel == null)
                {
                    continue;
                }

                var parentContentType = Resolver.ResolveType<TModel, TModelAttribute, TContentType>(parentContentTypeModel, contentTypes) as IContentTypeComposition;

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

                SaveContentType((TContentType)contentType);
            }
        }

        /// <summary>
        /// Sets the composition node types.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        private void SetCompositionNodeTypes(TContentType[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            foreach (var contentTypeModel in Models.Where(ctm => ctm.CompositionNodeTypes.Any()))
            {
                if (contentTypeModel.ParentNodeType != null)
                {
                    // Composition and inheritance don't mix.
                    continue;
                }

                var contentType = Resolver.ResolveType<TModel, TModelAttribute, TContentType>(contentTypeModel, contentTypes) as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                var compositionContentTypes = contentTypeModel.CompositionNodeTypes.Select(type => Resolver.ResolveType<TModel, TModelAttribute, TContentType>(GetModel(type), contentTypes) as IContentTypeComposition).Where(ct => ct != null).ToArray();

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

                SaveContentType((TContentType)contentType);
            }
        }

        /// <summary>
        /// Sets the allowed content types.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        private void SetAllowedContentTypes(TContentType[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            foreach (var contentTypeModel in Models.Where(ctm => ctm.AllowedChildNodeTypes.Any()))
            {
                var contentType = Resolver.ResolveType<TModel, TModelAttribute, TContentType>(contentTypeModel, contentTypes);

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
        private IEnumerable<ContentTypeSort> GetAllowedChildNodeContentTypes(
            TContentType[] contentTypes,
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

            var typeModels = new TypeModelResolver<TModel, TModelAttribute>().ResolveByModelTypes(allowedChildNodeTypes.ToArray(), Models);

            foreach (var typeModel in typeModels)
            {
                var contentType = Resolver.ResolveType<TModel, TModelAttribute, TContentType>(typeModel, contentTypes);

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