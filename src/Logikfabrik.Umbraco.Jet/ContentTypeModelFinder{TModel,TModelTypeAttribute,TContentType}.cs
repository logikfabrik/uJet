// <copyright file="ContentTypeModelFinder{TModel,TModelTypeAttribute,TContentType}.cs" company="Logikfabrik">
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
    /// The <see cref="ContentTypeModelFinder{TModel, TModelTypeAttribute, TContentType}" /> class. Class for finding content type models.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public class ContentTypeModelFinder<TModel, TModelTypeAttribute, TContentType> : ModelFinder<TModel, TModelTypeAttribute>, IContentTypeModelFinder<TModel, TModelTypeAttribute, TContentType>
        where TModel : ContentTypeModel<TModelTypeAttribute>
        where TModelTypeAttribute : ContentTypeModelTypeAttribute
        where TContentType : class, IContentTypeBase
    {
        private readonly ILogService _logService;
        private readonly ITypeRepository _typeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeModelFinder{TModel, TModelTypeAttribute, TContentType}" /> class.
        /// </summary>
        /// <param name="logService">The log service,</param>
        /// <param name="typeRepository">The type repository.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public ContentTypeModelFinder(ILogService logService, ITypeRepository typeRepository)
        {
            Ensure.That(logService).IsNotNull();
            Ensure.That(typeRepository).IsNotNull();

            _logService = logService;
            _typeRepository = typeRepository;
        }

        /// <inheritdoc />
        public TModel[] Find(TContentType contentTypeNeedle, TModel[] modelsHaystack)
        {
            Ensure.That(contentTypeNeedle).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();

            _logService.Log<ContentTypeModelFinder<TModel, TModelTypeAttribute, TContentType>>(new LogEntry(LogEntryType.Debug, $"Find content type models matching '{contentTypeNeedle.Name}' ('{contentTypeNeedle.Alias}')."));

            TModel[] models;

            var id = _typeRepository.GetContentTypeModelId(contentTypeNeedle.Id);

            if (id.HasValue)
            {
                models = modelsHaystack.Where(model => model.Id == id.Value).Distinct(Comparer).ToArray();

                if (models.Any())
                {
                    _logService.Log<ContentTypeModelFinder<TModel, TModelTypeAttribute, TContentType>>(new LogEntry(LogEntryType.Debug, $"Found '{models.Length}' content type models matching '{contentTypeNeedle.Name}' ('{contentTypeNeedle.Alias}')."));

                    return models;
                }
            }

            models = modelsHaystack.Where(model => model.Alias.Equals(contentTypeNeedle.Alias, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            _logService.Log<ContentTypeModelFinder<TModel, TModelTypeAttribute, TContentType>>(new LogEntry(LogEntryType.Debug, $"Found '{models.Length}' content type models matching '{contentTypeNeedle.Name}' ('{contentTypeNeedle.Alias}')."));

            return models;
        }
    }
}
