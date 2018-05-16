// <copyright file="ComposableContentTypeModelSynchronizer{TModel,TModelTypeAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;
    using Mappings;

    /// <summary>
    /// The <see cref="ComposableContentTypeModelSynchronizer{TModel,TModelTypeAttribute,TContentType}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ComposableContentTypeModelSynchronizer<TModel, TModelTypeAttribute, TContentType> : ContentTypeModelSynchronizer<TModel, TModelTypeAttribute, TContentType>
        where TModel : ComposableContentTypeModel<TModelTypeAttribute>
        where TModelTypeAttribute : ComposableContentTypeModelTypeAttribute
        where TContentType : class, IContentTypeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComposableContentTypeModelSynchronizer{TModel,TModelTypeAttribute,TContentType}" /> class.
        /// </summary>
        /// <param name="contentTypeFinder">The content type finder.</param>
        /// <param name="propertyTypeFinder">The property type finder.</param>
        /// <param name="dataTypeDefinitionService">The data type definition service.</param>
        /// <param name="typeRepository">The type repository.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ComposableContentTypeModelSynchronizer(
            IContentTypeFinder<TModel, TModelTypeAttribute, TContentType> contentTypeFinder,
            IPropertyTypeFinder propertyTypeFinder,
            IDataTypeDefinitionService dataTypeDefinitionService,
            ITypeRepository typeRepository)
            : base(contentTypeFinder, propertyTypeFinder, dataTypeDefinitionService, typeRepository)
        {
        }

        /// <inheritdoc />
        public override void Run()
        {
            base.Run();

            var contentTypes = GetContentTypes();

            SetParentNodeType(contentTypes);
            SetAllowedContentTypes(contentTypes);
            SetCompositionNodeTypes(contentTypes);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        internal override TContentType UpdateContentType(TContentType contentType, TModel model)
        {
            contentType = base.UpdateContentType(contentType, model);

            contentType.AllowedAsRoot = model.AllowedAsRoot;
            contentType.Thumbnail = !string.IsNullOrWhiteSpace(model.Thumbnail) ? model.Thumbnail : CreateContentType().Thumbnail;

            return contentType;
        }

        private void SetParentNodeType(TContentType[] contentTypes)
        {
            bool HasParent(TModel model) => model.ParentNodeType != null;

            foreach (var model in Models.Where(HasParent))
            {
                // ReSharper disable once UsePatternMatching
                var contentType = ContentTypeFinder.Find(model, contentTypes).SingleOrDefault() as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                // ReSharper disable once UsePatternMatching
                var parentContentType = ContentTypeFinder.Find(model.ParentNodeType, Models, contentTypes).SingleOrDefault() as IContentTypeComposition;

                if (parentContentType == null)
                {
                    continue;
                }

                var aliases = contentType.CompositionAliases().ToArray();

                // Inheritance and composition can not be mixed. Remove all compositions.
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
            bool HasComposition(TModel model) => model.ParentNodeType == null && model.CompositionNodeTypes.Any();

            foreach (var model in Models.Where(HasComposition))
            {
                // ReSharper disable once UsePatternMatching
                var contentType = ContentTypeFinder.Find(model, contentTypes).SingleOrDefault() as IContentTypeComposition;

                if (contentType == null)
                {
                    continue;
                }

                var compositionContentTypes = ContentTypeFinder.Find(model.CompositionNodeTypes, Models, contentTypes).Cast<IContentTypeComposition>().ToList();

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
            bool HasAllowedChildNodeTypes(TModel model) => model.AllowedChildNodeTypes.Any();

            foreach (var model in Models.Where(HasAllowedChildNodeTypes))
            {
                var contentType = ContentTypeFinder.Find(model, contentTypes).SingleOrDefault();

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
            var types = ContentTypeFinder.FindAll(allowedChildNodeTypes, Models, contentTypes);

            var allowedChildNodeContentTypes = new List<ContentTypeSort>();

            foreach (var type in types)
            {
                allowedChildNodeContentTypes.Add(new ContentTypeSort(new Lazy<int>(() => type.Id), allowedChildNodeContentTypes.Count, type.Alias));
            }

            return allowedChildNodeContentTypes;
        }
    }
}