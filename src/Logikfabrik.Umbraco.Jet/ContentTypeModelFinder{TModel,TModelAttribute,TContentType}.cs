// <copyright file="ContentTypeModelFinder{TModel,TModelAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;
    using Logging;

    /// <summary>
    /// The <see cref="ContentTypeModelFinder{TModel, TModelAttribute, TContentType}"/> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelAttribute">The model attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    public class ContentTypeModelFinder<TModel, TModelAttribute, TContentType> : TypeModelFinder<TModel, TModelAttribute>
        where TModel : ContentTypeModel<TModelAttribute>
        where TModelAttribute : ContentTypeModelAttribute
        where TContentType : IContentTypeBase
    {
        private readonly ILogService _logService;
        private readonly ITypeRepository _typeRepository;
        private readonly TypeModelComparer<TModel, TModelAttribute> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeModelFinder{TModel, TModelAttribute, TContentType}"/> class.
        /// </summary>
        /// <param name="logService">The log service,</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logService" />, or <paramref name="typeRepository" /> are <c>null</c>.</exception>
        public ContentTypeModelFinder(ILogService logService, ITypeRepository typeRepository)
            : base(logService)
        {
            if (logService == null)
            {
                throw new ArgumentNullException(nameof(logService));
            }

            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            _logService = logService;
            _typeRepository = typeRepository;
            _comparer = new TypeModelComparer<TModel, TModelAttribute>();
        }

        /// <summary>
        /// Finds the models matching the specified content type.
        /// </summary>
        /// <param name="contentTypeNeedle">The content type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeNeedle" />, or <paramref name="modelsHaystack" /> are <c>null</c>.</exception>
        public TModel[] Find(TContentType contentTypeNeedle, TModel[] modelsHaystack)
        {
            if (contentTypeNeedle == null)
            {
                throw new ArgumentNullException(nameof(contentTypeNeedle));
            }

            if (modelsHaystack == null)
            {
                throw new ArgumentNullException(nameof(modelsHaystack));
            }

            _logService.Log<ContentTypeModelFinder<TModel, TModelAttribute, TContentType>>(new LogEntry(LogEntryType.Debug, $"Find content type models matching {contentTypeNeedle.Name} ({contentTypeNeedle.Alias})."));

            TModel[] models;

            var id = _typeRepository.GetContentTypeModelId(contentTypeNeedle.Id);

            if (id.HasValue)
            {
                models = modelsHaystack.Where(model => model.Id == id.Value).Distinct(_comparer).ToArray();

                if (models.Any())
                {
                    _logService.Log<ContentTypeModelFinder<TModel, TModelAttribute, TContentType>>(new LogEntry(LogEntryType.Debug, $"Found {models.Length} content type models matching {contentTypeNeedle.Name} ({contentTypeNeedle.Alias})."));

                    return models;
                }
            }

            models = modelsHaystack.Where(model => model.Alias.Equals(contentTypeNeedle.Alias, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            _logService.Log<ContentTypeModelFinder<TModel, TModelAttribute, TContentType>>(new LogEntry(LogEntryType.Debug, $"Found {models.Length} content type models matching {contentTypeNeedle.Name} ({contentTypeNeedle.Alias})."));

            return models;
        }
    }
}
