// <copyright file="ComposableContentTypeSynchronizer{TModel,TModelAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ComposableContentTypeSynchronizer{TModel, TModelAttribute, TContentType}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelAttribute">The model attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    public abstract class ComposableContentTypeSynchronizer<TModel, TModelAttribute, TContentType> : ContentTypeSynchronizer<TModel, TModelAttribute, TContentType>
        where TModel : ComposableContentTypeModel<TModelAttribute>
        where TModelAttribute : ComposableContentTypeAttribute
        where TContentType : IContentTypeBase
    {
        private readonly ContentTypeFinder<TModel, TModelAttribute, TContentType> _contentTypeFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComposableContentTypeSynchronizer{TModel, TModelAttribute, TContentType}" /> class.
        /// </summary>
        /// <param name="typeRepository">The type repository.</param>
        protected ComposableContentTypeSynchronizer(ITypeRepository typeRepository)
            : base(typeRepository)
        {
            _contentTypeFinder = new ContentTypeFinder<TModel, TModelAttribute, TContentType>(typeRepository);
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public override void Run()
        {
            base.Run();

            var contentTypes = GetContentTypes();

            SetParentNodeType(contentTypes);
            SetAllowedContentTypes(contentTypes);
            SetCompositionNodeTypes(contentTypes);
        }

        /// <summary>
        /// Creates a content type.
        /// </summary>
        /// <param name="model">The model to use when creating the content type.</param>
        /// <returns>The created content type.</returns>
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
        /// Updates the specified content type.
        /// </summary>
        /// <param name="contentType">The content type to update.</param>
        /// <param name="model">The model to use when updating the content type.</param>
        /// <returns>The updated content type.</returns>
        internal override TContentType UpdateContentType(TContentType contentType, TModel model)
        {
            contentType = base.UpdateContentType(contentType, model);

            contentType.AllowedAsRoot = model.AllowedAsRoot;
            contentType.Thumbnail = !string.IsNullOrWhiteSpace(model.Thumbnail) ? model.Thumbnail : CreateContentType().Thumbnail;

            return contentType;
        }

        private void SetParentNodeType(TContentType[] contentTypes)
        {
            Func<TModel, bool> hasParent = model => model.ParentNodeType != null;

            foreach (var model in Models.Where(hasParent))
            {
                var contentType = _contentTypeFinder.Find(model, contentTypes).SingleOrDefault() as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                var parentContentType = _contentTypeFinder.Find(model.ParentNodeType, Models, contentTypes).SingleOrDefault() as IContentTypeComposition;

                if (parentContentType == null)
                {
                    continue;
                }

                var aliases = contentType.CompositionAliases().ToArray();

                // Inheritance and composition cannot be mixed. Remove all compositions.
                foreach (var alias in aliases)
                {
                    contentType.RemoveContentType(alias);
                }

                contentType.ParentId = parentContentType.Id;
                contentType.AddContentType(parentContentType);

                SaveContentType((TContentType)contentType);
            }
        }

        private void SetCompositionNodeTypes(TContentType[] contentTypes)
        {
            Func<TModel, bool> hasComposition = model => model.ParentNodeType == null && model.CompositionNodeTypes.Any();

            foreach (var model in Models.Where(hasComposition))
            {
                var contentType = _contentTypeFinder.Find(model, contentTypes).SingleOrDefault() as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                var compositionContentTypes = _contentTypeFinder.Find(model.CompositionNodeTypes, Models, contentTypes).Cast<IContentTypeComposition>().ToList();

                var aliases = contentType.CompositionAliases().ToArray();

                foreach (var alias in aliases)
                {
                    var type = compositionContentTypes.SingleOrDefault(compositionContentType => compositionContentType.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));

                    if (type != null)
                    {
                        // The content type is already part of the composition.
                        compositionContentTypes.Remove(type);

                        continue;
                    }

                    // The content type should no longer be part of the composition.
                    contentType.RemoveContentType(alias);
                }

                foreach (var compositionContentType in compositionContentTypes)
                {
                    // Add content types not already part of the composition.
                    contentType.AddContentType(compositionContentType);
                }

                SaveContentType((TContentType)contentType);
            }
        }

        private void SetAllowedContentTypes(TContentType[] contentTypes)
        {
            Func<TModel, bool> hasAllowedChildNodeTypes = model => model.AllowedChildNodeTypes.Any();

            foreach (var model in Models.Where(hasAllowedChildNodeTypes))
            {
                var contentType = _contentTypeFinder.Find(model, contentTypes).SingleOrDefault();

                if (contentType == null)
                {
                    continue;
                }

                contentType.AllowedContentTypes = GetAllowedChildNodeContentTypes(model.AllowedChildNodeTypes, contentTypes);

                SaveContentType(contentType);
            }
        }

        private IEnumerable<ContentTypeSort> GetAllowedChildNodeContentTypes(Type[] allowedChildNodeTypes, TContentType[] contentTypes)
        {
            var types = _contentTypeFinder.FindAll(allowedChildNodeTypes, Models, contentTypes);

            var allowedChildNodeContentTypes = new List<ContentTypeSort>();

            foreach (var type in types)
            {
                allowedChildNodeContentTypes.Add(new ContentTypeSort(new Lazy<int>(() => type.Id), allowedChildNodeContentTypes.Count, type.Alias));
            }

            return allowedChildNodeContentTypes;
        }
    }
}