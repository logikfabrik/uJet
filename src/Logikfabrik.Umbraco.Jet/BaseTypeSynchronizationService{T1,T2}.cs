// <copyright file="BaseTypeSynchronizationService{T1,T2}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
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
    /// The <see cref="BaseTypeSynchronizationService{T1, T2}" /> class.
    /// </summary>
    /// <typeparam name="T1">The type type.</typeparam>
    /// <typeparam name="T2">The type attribute type.</typeparam>
    public abstract class BaseTypeSynchronizationService<T1, T2> : ISynchronizationService
        where T1 : BaseType<T2>
        where T2 : BaseTypeAttribute
    {
        private readonly IContentTypeRepository _contentTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTypeSynchronizationService{T1, T2}" /> class.
        /// </summary>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeRepository" /> is <c>null</c>.</exception>
        protected BaseTypeSynchronizationService(IContentTypeRepository contentTypeRepository)
        {
            if (contentTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(contentTypeRepository));
            }

            _contentTypeRepository = contentTypeRepository;
        }

        /// <summary>
        /// Gets the content type models.
        /// </summary>
        /// <value>The content type models.</value>
        protected abstract T1[] ContentTypeModels { get; }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public virtual void Synchronize()
        {
            if (!ContentTypeModels.Any())
            {
                return;
            }

            ValidateModelTypeId();
            ValidateModelTypeAlias();

            var contentTypes = GetContentTypes();

            foreach (var contentTypeModel in ContentTypeModels)
            {
                Synchronize(contentTypes, contentTypeModel);
            }
        }

        /// <summary>
        /// Creates a content type for the specified content type model.
        /// </summary>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <returns>The created content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeModel" /> is <c>null</c>.</exception>
        internal virtual IContentTypeBase CreateContentType(T1 contentTypeModel)
        {
            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            var contentType = GetContentType();

            contentType.Name = contentTypeModel.Name;
            contentType.Alias = contentTypeModel.Alias;
            contentType.Description = contentTypeModel.Description;

            if (!string.IsNullOrWhiteSpace(contentTypeModel.Icon))
            {
                contentType.Icon = contentTypeModel.Icon;
            }

            return contentType;
        }

        /// <summary>
        /// Updates the content type for the specified content type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <returns>The updated content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="contentTypeModel" /> are <c>null</c>.</exception>
        internal virtual IContentTypeBase UpdateContentType(IContentTypeBase contentType, T1 contentTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            contentType.Name = contentTypeModel.Name;
            contentType.Alias = contentTypeModel.Alias;
            contentType.Description = contentTypeModel.Description;

            var defaultContentType = GetContentType();

            contentType.Icon = contentTypeModel.Icon ?? defaultContentType.Icon;

            return contentType;
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>The content types.</returns>
        protected abstract IContentTypeBase[] GetContentTypes();

        /// <summary>
        /// Gets a content type.
        /// </summary>
        /// <returns>A content type.</returns>
        protected abstract IContentTypeBase GetContentType();

        /// <summary>
        /// Gets the content type with the specified alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>The content type with the specified alias.</returns>
        protected abstract IContentTypeBase GetContentType(string alias);

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected abstract void SaveContentType(IContentTypeBase contentType);

        /// <summary>
        /// Finds the content type for the specified content type model.
        /// </summary>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <param name="contentTypes">The content types.</param>
        /// <returns>A content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeModel" />, or <paramref name="contentTypes" /> are <c>null</c>.</exception>
        protected IContentTypeBase FindContentType(T1 contentTypeModel, IContentTypeBase[] contentTypes)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            IContentTypeBase contentType;

            // Step 1; we try to find a match using the type ID (if any).
            if (contentTypeModel.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _contentTypeRepository.GetContentTypeId(contentTypeModel.Id.Value);

                if (umbracoId.HasValue)
                {
                    // There was an Umbraco ID matching the type ID in the uJet tables. We try to match that ID with an existing content type.
                    contentType = contentTypes.SingleOrDefault(ct => ct.Id == umbracoId.Value);

                    if (contentType != null)
                    {
                        // We've found a match. The type matches an existing Umbraco content type.
                        return contentType;
                    }
                }
            }

            // Step 2; we try to find a match using the type alias.
            contentType = contentTypes.SingleOrDefault(ct => ct.Alias == contentTypeModel.Alias);

            // We might have found a match.
            return contentType;
        }

        private void Synchronize(IContentTypeBase[] contentTypes, T1 contentTypeModel)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            var contentType = FindContentType(contentTypeModel, contentTypes);

            contentType = contentType == null
                ? CreateContentType(contentTypeModel)
                : UpdateContentType(contentType, contentTypeModel);

            SynchronizePropertyTypes(contentType, contentTypeModel);

            SaveContentType(contentType);

            // We get the content type once more to refresh it after saving it.
            contentType = GetContentType(contentType.Alias);

            // Set/update tracking.
            SetContentTypeId(contentType, contentTypeModel);
            SetPropertyTypeId(contentType, contentTypeModel);
        }

        /// <summary>
        /// Validates the model type identifiers.
        /// </summary>
        private void ValidateModelTypeId()
        {
            var set = new HashSet<Guid>();

            foreach (var contentTypeModel in ContentTypeModels)
            {
                if (!contentTypeModel.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(contentTypeModel.Id.Value))
                {
                    var conflictingTypes = ContentTypeModels.Where(ctm => ctm.Id == contentTypeModel.Id.Value).Select(ctm => ctm.Type.Name);

                    throw new InvalidOperationException($"ID conflict for model types {string.Join(", ", conflictingTypes)}. ID {contentTypeModel.Id.Value} is already in use.");
                }

                set.Add(contentTypeModel.Id.Value);
            }
        }

        /// <summary>
        /// Validates the model type aliases.
        /// </summary>
        private void ValidateModelTypeAlias()
        {
            var set = new HashSet<string>();

            foreach (var contentTypeModel in ContentTypeModels)
            {
                if (set.Contains(contentTypeModel.Alias))
                {
                    var conflictingTypes = ContentTypeModels.Where(ctm => ctm.Alias == contentTypeModel.Alias).Select(ctm => ctm.Type.Name);

                    throw new InvalidOperationException($"Alias conflict for model types {string.Join(", ", conflictingTypes)}. Alias {contentTypeModel.Alias} is already in use.");
                }

                set.Add(contentTypeModel.Alias);
            }
        }

        /// <summary>
        /// Sets the content type identifier of the specified content type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="contentTypeModel" /> are <c>null</c>.</exception>
        private void SetContentTypeId(IContentTypeBase contentType, T1 contentTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            if (!contentTypeModel.Id.HasValue)
            {
                return;
            }

            _contentTypeRepository.SetContentTypeId(contentTypeModel.Id.Value, contentType.Id);
        }

        /// <summary>
        /// Sets the property type identifier for the properties of the specified content type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="contentTypeModel" /> are <c>null</c>.</exception>
        private void SetPropertyTypeId(IContentTypeBase contentType, T1 contentTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            foreach (var property in contentTypeModel.Properties)
            {
                if (!property.Id.HasValue)
                {
                    continue;
                }

                var umbracoId = _contentTypeRepository.GetPropertyTypeId(property.Id.Value);

                global::Umbraco.Core.Models.PropertyType propertyType;

                if (umbracoId.HasValue)
                {
                    propertyType = contentType.PropertyTypes.SingleOrDefault(type => type.Id == umbracoId.Value);

                    if (propertyType != null)
                    {
                        // The Umbraco property type and uJet type property are in sync.
                        continue;
                    }

                    // The uJet type property has been synchronized before, but for a property type for another content type.
                    // This is possible if the uJet type property is tracked, but the content type is not.
                    // TODO: Bättre förklaring!
                }

                propertyType = contentType.PropertyTypes.SingleOrDefault(pt => pt.Alias == property.Alias);

                if (propertyType != null)
                {
                    _contentTypeRepository.SetPropertyTypeId(property.Id.Value, propertyType.Id);
                }
            }
        }

        private void SynchronizePropertyTypes(IContentTypeBase contentType, T1 contentTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            foreach (var property in contentTypeModel.Properties)
            {
                SynchronizePropertyType(contentType, property);
            }
        }

        private void SynchronizePropertyType(IContentTypeBase contentType, TypeProperty propertyTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            var propertyType = FindPropertyType(contentType, propertyTypeModel);

            if (propertyType == null)
            {
                CreatePropertyType(contentType, propertyTypeModel);
            }
            else
            {
                UpdatePropertyType(contentType, propertyType, propertyTypeModel);
            }
        }

        /// <summary>
        /// Creates a property type for the specified property type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="propertyTypeModel">The property type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="propertyTypeModel" /> are <c>null</c>.</exception>
        private void CreatePropertyType(IContentTypeBase contentType, TypeProperty propertyTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            var definition = GetDataTypeDefinition(propertyTypeModel);

            var propertyType = new global::Umbraco.Core.Models.PropertyType(definition)
            {
                Name = propertyTypeModel.Name,
                Alias = propertyTypeModel.Alias,
                Mandatory = propertyTypeModel.Mandatory,
                Description = propertyTypeModel.Description,
                ValidationRegExp = propertyTypeModel.RegularExpression
            };

            if (propertyTypeModel.SortOrder.HasValue)
            {
                propertyType.SortOrder = propertyTypeModel.SortOrder.Value;
            }

            if (!string.IsNullOrWhiteSpace(propertyTypeModel.PropertyGroup))
            {
                contentType.AddPropertyType(propertyType, propertyTypeModel.PropertyGroup);
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, <paramref name="propertyType" />, or <paramref name="propertyTypeModel" /> are <c>null</c>.</exception>
        private void UpdatePropertyType(IContentTypeBase contentType, global::Umbraco.Core.Models.PropertyType propertyType, TypeProperty propertyTypeModel)
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
        /// Gets the data type definition for the specified property type model.
        /// </summary>
        /// <param name="propertyTypeModel">The property type model.</param>
        /// <returns>The data type definition for the specified property type model.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyTypeModel" /> is <c>null</c>.</exception>
        private IDataTypeDefinition GetDataTypeDefinition(TypeProperty propertyTypeModel)
        {
            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            var definition = DataTypeDefinitionMappings.GetDefinition(propertyTypeModel.UIHint, propertyTypeModel.Type);

            if (definition == null)
            {
                throw new Exception($"There is data type no definition for type {propertyTypeModel.Type}.");
            }

            return definition;
        }

        private global::Umbraco.Core.Models.PropertyType FindPropertyType(IContentTypeBase contentType, TypeProperty propertyTypeModel)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            global::Umbraco.Core.Models.PropertyType propertyType;

            // Step 1; we try to find a match using the type ID (if any).
            if (propertyTypeModel.Id.HasValue)
            {
                // The type property has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _contentTypeRepository.GetPropertyTypeId(propertyTypeModel.Id.Value);

                if (umbracoId.HasValue)
                {
                    // There was an Umbraco ID matching the type ID in the uJet tables. We try to match that ID with an existing property type.
                    propertyType = contentType.PropertyTypes.SingleOrDefault(pt => pt.Id == umbracoId.Value);

                    if (propertyType != null)
                    {
                        // We've found a match. The type matches an existing Umbraco property type.
                        return propertyType;
                    }
                }
            }

            // Step 2; we try to find a match using the type alias.
            propertyType = contentType.PropertyTypes.SingleOrDefault(pt => pt.Alias == propertyTypeModel.Alias);

            // We might have found a match.
            return propertyType;
        }
    }
}