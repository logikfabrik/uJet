// <copyright file="ContentTypeFinder{TModel,TModelTypeAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using Logging;

    /// <summary>
    /// The <see cref="ContentTypeFinder{TModel, TModelTypeAttribute, TContentType}" /> class. Class for finding Umbraco content types.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public class ContentTypeFinder<TModel, TModelTypeAttribute, TContentType> : IContentTypeFinder<TModel, TModelTypeAttribute, TContentType>
        where TModel : ContentTypeModel<TModelTypeAttribute>
        where TModelTypeAttribute : ContentTypeModelTypeAttribute
        where TContentType : class, IContentTypeBase
    {
        private readonly ILogService _logService;
        private readonly ITypeRepository _typeRepository;
        private readonly IContentTypeModelFinder<TModel, TModelTypeAttribute, TContentType> _contentTypeModelFinder;
        private readonly EntityTypeComparer<TContentType> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeFinder{TModel, TModelTypeAttribute, TContentType}" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="typeRepository">The type repository.</param>
        public ContentTypeFinder(ILogService logService, ITypeRepository typeRepository)
        {
            Ensure.That(logService).IsNotNull();
            Ensure.That(typeRepository).IsNotNull();

            _logService = logService;
            _typeRepository = typeRepository;
            // TODO: DI
            _contentTypeModelFinder = new ContentTypeModelFinder<TModel, TModelTypeAttribute, TContentType>(logService, typeRepository);
            _comparer = new EntityTypeComparer<TContentType>();
        }

        /// <inheritdoc />
        public TContentType[] Find(Type[] modelTypeNeedles, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelTypeNeedles).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            return modelTypeNeedles.SelectMany(needle => Find(needle, modelsHaystack, contentTypesHaystack)).Distinct(_comparer).ToArray();
        }

        /// <inheritdoc />
        public TContentType[] Find(Type modelTypeNeedle, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelTypeNeedle).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            var modelNeedles = _contentTypeModelFinder.Find(modelTypeNeedle, modelsHaystack);

            return Find(modelNeedles, contentTypesHaystack).Distinct(_comparer).ToArray();
        }

        /// <inheritdoc />
        public TContentType[] FindAll(Type[] modelTypeNeedles, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            Ensure.That(modelTypeNeedles).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();
            Ensure.That(contentTypesHaystack).IsNotNull();

            return modelTypeNeedles.SelectMany(needle => FindAll(needle, modelsHaystack, contentTypesHaystack)).Distinct(_comparer).ToArray();
        }

        /// <inheritdoc />
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

        private IEnumerable<TContentType> FindAll(Type modelTypeNeedle, TModel[] modelsHaystack, TContentType[] contentTypesHaystack)
        {
            var modelNeedles = _contentTypeModelFinder.FindAll(modelTypeNeedle, modelsHaystack);

            return Find(modelNeedles, contentTypesHaystack).Distinct(_comparer).ToArray();
        }

        private IEnumerable<TContentType> Find(IEnumerable<TModel> modelNeedles, TContentType[] contentTypesHaystack)
        {
            return modelNeedles.SelectMany(needle => Find(needle, contentTypesHaystack)).Distinct(_comparer).ToArray();
        }
    }
}