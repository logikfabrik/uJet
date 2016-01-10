// <copyright file="ContentTypeModelSynchronizationService{TContentTypeModel,TContentTypeModelAttribute,TContentType}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;
    using Mappings;

    /// <summary>
    /// The <see cref="ContentTypeModelSynchronizationService{TContentTypeModel, TContentTypeModelAttribute, TContentType}" /> class.
    /// </summary>
    /// <typeparam name="TContentTypeModel">The content type model type.</typeparam>
    /// <typeparam name="TContentTypeModelAttribute">The content type model attribute type.</typeparam>
    /// <typeparam name="TContentType">The content type.</typeparam>
    public abstract class ContentTypeModelSynchronizationService<TContentTypeModel, TContentTypeModelAttribute, TContentType> : ISynchronizationService
        where TContentTypeModel : ContentTypeModel<TContentTypeModelAttribute>
        where TContentTypeModelAttribute : ContentTypeModelAttribute
        where TContentType : IContentTypeBase
    {
        private readonly ITypeRepository _typeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeModelSynchronizationService{TContentTypeModel, TContentTypeModelAttribute, TContentType}" /> class.
        /// </summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeResolver" />, or <paramref name="typeRepository" /> are <c>null</c>.</exception>
        protected ContentTypeModelSynchronizationService(ITypeResolver typeResolver, ITypeRepository typeRepository)
        {
            if (typeResolver == null)
            {
                throw new ArgumentNullException(nameof(typeResolver));
            }

            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            Resolver = typeResolver;
            _typeRepository = typeRepository;
        }

        /// <summary>
        /// Gets the type resolver.
        /// </summary>
        /// <value>
        /// The type resolver.
        /// </value>
        protected ITypeResolver Resolver { get; }

        /// <summary>
        /// Gets the content type models.
        /// </summary>
        /// <value>The content type models.</value>
        protected abstract TContentTypeModel[] Models { get; }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public virtual void Synchronize()
        {
            if (!Models.Any())
            {
                return;
            }

            new ContentTypeModelValidator<TContentTypeModel, TContentTypeModelAttribute>().Validate(Models);

            var contentTypes = GetContentTypes();

            foreach (var model in Models)
            {
                Synchronize(model, contentTypes);
            }
        }

        /// <summary>
        /// Creates a content type for the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The created content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="model" /> is <c>null</c>.</exception>
        internal virtual TContentType CreateContentType(TContentTypeModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var contentType = GetContentType();

            contentType.Name = model.Name;
            contentType.Alias = model.Alias;
            contentType.Description = model.Description;

            if (!string.IsNullOrWhiteSpace(model.Icon))
            {
                contentType.Icon = model.Icon;
            }

            return contentType;
        }

        /// <summary>
        /// Updates the content type for the specified model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="model">The model.</param>
        /// <returns>The updated content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="model" /> are <c>null</c>.</exception>
        internal virtual TContentType UpdateContentType(TContentType contentType, TContentTypeModel model)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            contentType.Name = model.Name;
            contentType.Alias = model.Alias;
            contentType.Description = model.Description;
            contentType.Icon = model.Icon ?? GetContentType().Icon;

            return contentType;
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>The content types.</returns>
        protected abstract TContentType[] GetContentTypes();

        /// <summary>
        /// Gets a content type.
        /// </summary>
        /// <returns>A content type.</returns>
        protected abstract TContentType GetContentType();

        /// <summary>
        /// Gets the content type with the specified alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>The content type.</returns>
        protected abstract TContentType GetContentType(string alias);

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected abstract void SaveContentType(TContentType contentType);

        private void Synchronize(TContentTypeModel model, TContentType[] contentTypes)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            var contentType = Resolver.ResolveType<TContentTypeModel, TContentTypeModelAttribute, TContentType>(model, contentTypes);

            contentType = contentType == null
                ? CreateContentType(model)
                : UpdateContentType(contentType, model);

            SynchronizePropertyTypes(model, contentType);

            SaveContentType(contentType);

            // We get the content type once more to refresh it after saving it.
            contentType = GetContentType(contentType.Alias);

            // Set/update tracking.
            SetContentTypeId(model, contentType);
            SetPropertyTypeId(model, contentType);
        }

        /// <summary>
        /// Sets the content type identifier of the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="contentType">The content type.</param>
        private void SetContentTypeId(TContentTypeModel model, TContentType contentType)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

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
        private void SetPropertyTypeId(TContentTypeModel model, TContentType contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var propertyTypes = contentType.PropertyTypes.ToArray();

            foreach (var property in model.Properties)
            {
                if (!property.Id.HasValue)
                {
                    continue;
                }

                var propertyType = Resolver.ResolveType(property, propertyTypes);

                if (propertyType != null)
                {
                    _typeRepository.SetPropertyTypeId(property.Id.Value, propertyType.Id);
                }
            }
        }

        private void SynchronizePropertyTypes(TContentTypeModel model, TContentType contentType)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            foreach (var property in model.Properties)
            {
                SynchronizePropertyType(contentType, property);
            }
        }

        private void SynchronizePropertyType(TContentType contentType, TypeProperty propertyTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            var propertyType = Resolver.ResolveType(propertyTypeModel, contentType.PropertyTypes.ToArray());

            if (propertyType == null)
            {
                CreatePropertyType(propertyTypeModel, contentType);
            }
            else
            {
                UpdatePropertyType(contentType, propertyType, propertyTypeModel);
            }
        }

        /// <summary>
        /// Creates a property type for the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="contentType">The content type.</param>
        private void CreatePropertyType(TypeProperty model, TContentType contentType)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

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

        /// <summary>
        /// Updates the property type for the specified property type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="propertyType">The property type.</param>
        /// <param name="propertyTypeModel">The property type model.</param>
        private void UpdatePropertyType(TContentType contentType, global::Umbraco.Core.Models.PropertyType propertyType, TypeProperty propertyTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            if (!contentType.PropertyGroups.Contains(propertyTypeModel.PropertyGroup) || (contentType.PropertyGroups.Contains(propertyTypeModel.PropertyGroup) && !contentType.PropertyGroups[propertyTypeModel.PropertyGroup].PropertyTypes.Contains(propertyTypeModel.Alias)))
            {
                contentType.MovePropertyType(propertyTypeModel.Alias, propertyTypeModel.PropertyGroup);
            }

            propertyType.Name = propertyTypeModel.Name;
            propertyType.Alias = propertyTypeModel.Alias;
            propertyType.Mandatory = propertyTypeModel.Mandatory;
            propertyType.Description = propertyTypeModel.Description;
            propertyType.ValidationRegExp = propertyTypeModel.RegularExpression;

            if (propertyTypeModel.SortOrder.HasValue)
            {
                propertyType.SortOrder = propertyTypeModel.SortOrder.Value;
            }

            var definition = GetDataTypeDefinition(propertyTypeModel);

            if (propertyType.DataTypeDefinitionId != definition.Id)
            {
                propertyType.DataTypeDefinitionId = definition.Id;
            }
        }

        /// <summary>
        /// Gets the data type definition for the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The data type definition.</returns>
        private IDataTypeDefinition GetDataTypeDefinition(TypeProperty model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var definition = DataTypeDefinitionMappings.GetDefinition(model.UIHint, model.Type);

            if (definition == null)
            {
                throw new Exception($"There is data type no definition for type {model.Type}.");
            }

            return definition;
        }
    }
}