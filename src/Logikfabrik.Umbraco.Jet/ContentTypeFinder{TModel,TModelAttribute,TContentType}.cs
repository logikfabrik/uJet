// <copyright file="ContentTypeFinder{TModel,TModelAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using Logging;

    /// <summary>
    /// The <see cref="ContentTypeFinder{TModel, TModelAttribute, TContentType}" /> class. Class for finding content types by model or by model type.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelAttribute">The model attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    public class ContentTypeFinder<TModel, TModelAttribute, TContentType>
        where TModel : ContentTypeModel<TModelAttribute>
        where TModelAttribute : ContentTypeModelAttribute
        where TContentType : class, IContentTypeBase
    {
        private readonly ILogService _logService;
        private readonly ITypeRepository _typeRepository;
        private readonly EntityTypeComparer<TContentType> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeFinder{TModel, TModelAttribute, TContentType}" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="typeRepository">The type repository.</param>
        public ContentTypeFinder(ILogService logService, ITypeRepository typeRepository)
        {
            Ensure.That(logService).IsNotNull();
            Ensure.That(typeRepository).IsNotNull();

            _logService = logService;
            _typeRepository = typeRepository;
            _comparer = new EntityTypeComparer<TContentType>();
        }

        /// <summary>
        /// Finds the models with a model type matching any of the specified model types, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        public TContentType[] Find(Type[] modelTypeNeedles, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelTypeNeedles).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            return modelTypeNeedles.SelectMany(needle => Find(needle, modelsHaystack, contentTypesHaystack)).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds the models with a model type matching the specified model type, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        public TContentType[] Find(Type modelTypeNeedle, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelTypeNeedle).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            var modelNeedles = new ContentTypeModelFinder<TModel, TModelAttribute, TContentType>(_logService, _typeRepository).Find(modelTypeNeedle, modelsHaystack);

            return Find(modelNeedles, contentTypesHaystack).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds all models with a model type matching any of the specified model types, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        public TContentType[] FindAll(Type[] modelTypeNeedles, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelTypeNeedles).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            return modelTypeNeedles.SelectMany(needle => FindAll(needle, modelsHaystack, contentTypesHaystack)).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds all models with a model type matching the specified model type, and then the content types matching any of those models.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the content types for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        public TContentType[] FindAll(Type modelTypeNeedle, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelTypeNeedle).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            var modelNeedles = new ContentTypeModelFinder<TModel, TModelAttribute, TContentType>(_logService, _typeRepository).FindAll(modelTypeNeedle, modelsHaystack);

            return Find(modelNeedles, contentTypesHaystack).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds the content types matching any of the specified models.
        /// </summary>
        /// <param name="modelNeedles">The models to find the content types for.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        public TContentType[] Find(TModel[] modelNeedles, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelNeedles).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            return modelNeedles.SelectMany(needle => Find(needle, contentTypesHaystack)).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds the content types matching the specified model.
        /// </summary>
        /// <param name="modelNeedle">The model to find the content types for.</param>
        /// <param name="contentTypesHaystack">The haystack of content types.</param>
        /// <returns>The content types found.</returns>
        /// <remarks>A content type is a match if a model needle's ID can be mapped to the ID of the content type, or if a model needle and the content type has the same alias.</remarks>
        public TContentType[] Find(TModel modelNeedle, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelNeedle).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            // ReSharper disable once InvertIf
            if (modelNeedle.Id.HasValue)
            {
                var id = _typeRepository.GetContentTypeId(modelNeedle.Id.Value);

                // ReSharper disable once InvertIf
                if (id.HasValue)
                {
                    var contentTypes = contentTypesHaystack.Where(contentType => contentType.Id == id.Value).Distinct(_comparer).ToArray();

                    if (contentTypes.Any())
                    {
                        return contentTypes;
                    }
                }
            }

            return contentTypesHaystack.Where(contentType => contentType.Alias != null && contentType.Alias.Equals(modelNeedle.Alias, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }
    }
}