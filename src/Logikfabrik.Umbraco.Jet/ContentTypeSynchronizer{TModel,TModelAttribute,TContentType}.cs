// <copyright file="ContentTypeSynchronizer{TModel,TModelAttribute,TContentType}.cs" company="Logikfabrik">
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
    using Mappings;

    /// <summary>
    /// The <see cref="ContentTypeSynchronizer{TModel, TModelAttribute, TContentType}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelAttribute">The attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ContentTypeSynchronizer<TModel, TModelAttribute, TContentType> : ISynchronizer
        where TModel : ContentTypeModel<TModelAttribute>
        where TModelAttribute : ContentTypeModelAttribute
        where TContentType : IContentTypeBase
    {
        private readonly ITypeRepository _typeRepository;
        private readonly IDataTypeDefinitionService _dataTypeDefinitionService;
        private readonly ContentTypeFinder<TModel, TModelAttribute, TContentType> _contentTypeFinder;
        private readonly PropertyTypeFinder _propertyTypeFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeSynchronizer{TModel, TModelAttribute, TContentType}" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <param name="dataTypeDefinitionService">The data type definition service.</param>
        protected ContentTypeSynchronizer(ILogService logService, ITypeRepository typeRepository, IDataTypeDefinitionService dataTypeDefinitionService)
        {
            EnsureArg.IsNotNull(logService);
            EnsureArg.IsNotNull(typeRepository);
            EnsureArg.IsNotNull(dataTypeDefinitionService);

            _typeRepository = typeRepository;
            _dataTypeDefinitionService = dataTypeDefinitionService;
            _contentTypeFinder = new ContentTypeFinder<TModel, TModelAttribute, TContentType>(logService, typeRepository);
            _propertyTypeFinder = new PropertyTypeFinder(logService, typeRepository);
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>
        /// The models.
        /// </value>
        protected abstract TModel[] Models { get; }

        /// <inheritdoc />
        public virtual void Run()
        {
            if (!Models.Any())
            {
                return;
            }

            new ContentTypeModelValidator<TModel, TModelAttribute>().Validate(Models);

            // TODO: Optimize. Whe get all contenttypes here, but also in the Synchronize method.
            var contentTypes = GetContentTypes();

            foreach (var model in Models)
            {
                Synchronize(model, contentTypes);
            }
        }

        /// <summary>
        /// Creates a content type.
        /// </summary>
        /// <param name="model">The model to use when creating the content type.</param>
        /// <returns>The created content type.</returns>
        internal virtual TContentType CreateContentType(TModel model)
        {
            var contentType = CreateContentType();

            contentType.Name = model.Name;
            contentType.Alias = model.Alias;
            contentType.Description = model.Description;
            contentType.IsContainer = model.IsContainer;

            if (!string.IsNullOrWhiteSpace(model.Icon))
            {
                contentType.Icon = model.Icon;
            }

            return contentType;
        }

        /// <summary>
        /// Updates the specified content type.
        /// </summary>
        /// <param name="contentType">The content type to update.</param>
        /// <param name="model">The model to use when updating the content type.</param>
        /// <returns>The updated content type.</returns>
        internal virtual TContentType UpdateContentType(TContentType contentType, TModel model)
        {
            contentType.Name = model.Name;
            contentType.Alias = model.Alias;
            contentType.Description = model.Description;
            contentType.Icon = model.Icon ?? CreateContentType().Icon;
            contentType.IsContainer = model.IsContainer;

            return contentType;
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>
        /// The content types.
        /// </returns>
        protected abstract TContentType[] GetContentTypes();

        /// <summary>
        /// Creates a content type.
        /// </summary>
        /// <returns>
        /// The created content type.
        /// </returns>
        protected abstract TContentType CreateContentType();

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type to save.</param>
        protected abstract void SaveContentType(TContentType contentType);

        private void CreatePropertyType(PropertyType model, TContentType contentType)
        {
            var definition = GetDataTypeDefinition(model);

            var propertyType = new global::Umbraco.Core.Models.PropertyType(definition)
            {
                Name = model.Name,
                Alias = model.Alias,
                Mandatory = model.Mandatory,
                Description = model.Description,
                ValidationRegExp = model.RegularExpression
            };

            if (model.SortOrder.HasValue)
            {
                propertyType.SortOrder = model.SortOrder.Value;
            }

            if (!string.IsNullOrWhiteSpace(model.PropertyGroup))
            {
                contentType.AddPropertyType(propertyType, model.PropertyGroup);
            }
            else
            {
                contentType.AddPropertyType(propertyType);
            }
        }

        private void UpdatePropertyType(TContentType contentType, global::Umbraco.Core.Models.PropertyType propertyType, PropertyType model)
        {
            if (!contentType.PropertyGroups.Contains(model.PropertyGroup) || (contentType.PropertyGroups.Contains(model.PropertyGroup) && !contentType.PropertyGroups[model.PropertyGroup].PropertyTypes.Contains(model.Alias)))
            {
                contentType.MovePropertyType(model.Alias, model.PropertyGroup);
            }

            propertyType.Name = model.Name;
            propertyType.Alias = model.Alias;
            propertyType.Mandatory = model.Mandatory;
            propertyType.Description = model.Description;
            propertyType.ValidationRegExp = model.RegularExpression;

            if (model.SortOrder.HasValue)
            {
                propertyType.SortOrder = model.SortOrder.Value;
            }

            var definition = GetDataTypeDefinition(model);

            propertyType.DataTypeDefinitionId = definition.Id;
        }

        private IDataTypeDefinition GetDataTypeDefinition(PropertyType model)
        {
            var definition = _dataTypeDefinitionService.GetDefinition(model.UIHint, model.Type);

            if (definition == null)
            {
                throw new Exception($"There is no data type definition for type {model.Type}.");
            }

            return definition;
        }

        private void Synchronize(TModel model, TContentType[] contentTypes)
        {
            var contentType = _contentTypeFinder.Find(model, contentTypes).SingleOrDefault();

            contentType = contentType == null
                ? CreateContentType(model)
                : UpdateContentType(contentType, model);

            SynchronizePropertyTypes(model, contentType);

            SaveContentType(contentType);

            // We get the content type once more to refresh it after saving it.
            contentType = GetContentTypes().Single(ct => ct.Alias != null && ct.Alias.Equals(contentType.Alias, StringComparison.InvariantCultureIgnoreCase));

            // Set/update tracking.
            SetContentTypeId(model, contentType);
            SetPropertyTypeId(model, contentType);
        }

        /// <summary>
        /// Sets the content type identifier of the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="contentType">The content type.</param>
        private void SetContentTypeId(TModel model, TContentType contentType)
        {
            if (!model.Id.HasValue)
            {
                return;
            }

            _typeRepository.SetContentTypeId(model.Id.Value, contentType.Id);
        }

        /// <summary>
        /// Sets the property type identifier for the properties of the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="contentType">The content type.</param>
        private void SetPropertyTypeId(TModel model, TContentType contentType)
        {
            var propertyTypes = contentType.PropertyTypes?.ToArray() ?? new global::Umbraco.Core.Models.PropertyType[] { };

            foreach (var property in model.Properties)
            {
                if (!property.Id.HasValue)
                {
                    continue;
                }

                var propertyType = _propertyTypeFinder.Find(property, propertyTypes).SingleOrDefault();

                if (propertyType != null)
                {
                    _typeRepository.SetPropertyTypeId(property.Id.Value, propertyType.Id);
                }
            }
        }

        private void SynchronizePropertyTypes(TModel model, TContentType contentType)
        {
            foreach (var property in model.Properties)
            {
                SynchronizePropertyType(contentType, property);
            }
        }

        private void SynchronizePropertyType(TContentType contentType, PropertyType model)
        {
            var propertyType = _propertyTypeFinder.Find(model, contentType.PropertyTypes.ToArray()).SingleOrDefault();

            if (propertyType == null)
            {
                CreatePropertyType(model, contentType);
            }
            else
            {
                UpdatePropertyType(contentType, propertyType, model);
            }
        }
    }
}