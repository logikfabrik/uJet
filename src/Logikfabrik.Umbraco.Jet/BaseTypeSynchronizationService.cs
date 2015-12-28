// <copyright file="BaseTypeSynchronizationService.cs" company="Logikfabrik">
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
    /// The <see cref="BaseTypeSynchronizationService" /> class.
    /// </summary>
    public abstract class BaseTypeSynchronizationService : ISynchronizationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeRepository" /> is <c>null</c>.</exception>
        protected BaseTypeSynchronizationService(IContentTypeRepository contentTypeRepository)
        {
            if (contentTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(contentTypeRepository));
            }

            ContentTypeRepository = contentTypeRepository;
        }

        /// <summary>
        /// Gets the content type repository.
        /// </summary>
        /// <value>
        /// The content type repository.
        /// </value>
        protected IContentTypeRepository ContentTypeRepository { get; }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public abstract void Synchronize();

        /// <summary>
        /// Creates a new content type using the uJet content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeConstructor">The content type constructor.</param>
        /// <param name="jetContentType">The base type.</param>
        /// <returns>The created content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeConstructor" />, or <paramref name="jetContentType" /> are <c>null</c>.</exception>
        protected IContentTypeBase CreateBaseContentType<T>(
            Func<IContentTypeBase> contentTypeConstructor,
            BaseType<T> jetContentType)
            where T : BaseTypeAttribute
        {
            if (contentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeConstructor));
            }

            if (jetContentType == null)
            {
                throw new ArgumentNullException(nameof(jetContentType));
            }

            var contentType = contentTypeConstructor();

            contentType.Name = jetContentType.Name;
            contentType.Alias = jetContentType.Alias;
            contentType.Description = jetContentType.Description;

            if (!string.IsNullOrWhiteSpace(jetContentType.Icon))
            {
                contentType.Icon = jetContentType.Icon;
            }

            return contentType;
        }

        /// <summary>
        /// Gets the content type that matches the uJet content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="jetContentType">The uJet content type.</param>
        /// <returns>The content type that matches the uJet content type specified.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, or <paramref name="jetContentType" /> are <c>null</c>.</exception>
        protected IContentTypeBase GetBaseContentType<T>(IContentTypeBase[] contentTypes, BaseType<T> jetContentType)
            where T : BaseTypeAttribute
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (jetContentType == null)
            {
                throw new ArgumentNullException(nameof(jetContentType));
            }

            IContentTypeBase contentType;

            // Step 1; we try to find a match using the type ID (if any).
            if (jetContentType.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = ContentTypeRepository.GetContentTypeId(jetContentType.Id.Value);

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
            contentType = contentTypes.SingleOrDefault(ct => ct.Alias == jetContentType.Alias);

            // We might have found a match.
            return contentType;
        }

        /// <summary>
        /// Updates the content type to match the uJet content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentTypeConstructor">The content type constructor.</param>
        /// <param name="jetContentType">The uJet content type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, <paramref name="contentTypeConstructor" />, or <paramref name="jetContentType" /> are <c>null</c>.</exception>
        protected void UpdateBaseContentType<T>(
            IContentTypeBase contentType,
            Func<IContentTypeBase> contentTypeConstructor,
            BaseType<T> jetContentType)
            where T : BaseTypeAttribute
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentTypeConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeConstructor));
            }

            if (jetContentType == null)
            {
                throw new ArgumentNullException(nameof(jetContentType));
            }

            contentType.Name = jetContentType.Name;
            contentType.Alias = jetContentType.Alias;
            contentType.Description = jetContentType.Description;

            var defaultContentType = contentTypeConstructor();

            contentType.Icon = jetContentType.Icon ?? defaultContentType.Icon;
        }

        /// <summary>
        /// Synchronizes the property types.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="jetTypeProperties">The uJet type properties.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="jetTypeProperties" /> are <c>null</c>.</exception>
        protected void SynchronizePropertyTypes(IContentTypeBase contentType, IEnumerable<TypeProperty> jetTypeProperties)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (jetTypeProperties == null)
            {
                throw new ArgumentNullException(nameof(jetTypeProperties));
            }

            foreach (var jetTypeProperty in jetTypeProperties)
            {
                SynchronizePropertyType(contentType, jetTypeProperty);
            }
        }

        /// <summary>
        /// Sets the property type identifier.
        /// </summary>
        /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
        /// <param name="contentType">The content type.</param>
        /// <param name="jetContentType">The uJet content type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="jetContentType" /> are <c>null</c>.</exception>
        protected void SetPropertyTypeId<T>(IContentTypeBase contentType, BaseType<T> jetContentType)
            where T : BaseTypeAttribute
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            foreach (var jetTypeProperty in jetContentType.Properties)
            {
                if (!jetTypeProperty.Id.HasValue)
                {
                    continue;
                }

                var umbracoId = ContentTypeRepository.GetPropertyTypeId(jetTypeProperty.Id.Value);

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
                }

                propertyType = contentType.PropertyTypes.SingleOrDefault(pt => pt.Alias == jetTypeProperty.Alias);

                if (propertyType != null)
                {
                    ContentTypeRepository.SetPropertyTypeId(jetTypeProperty.Id.Value, propertyType.Id);
                }
            }
        }

        /// <summary>
        /// Creates a new property type using the uJet type property.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="jetTypeProperty">The uJet type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="jetTypeProperty" /> are <c>null</c>.</exception>
        private static void CreatePropertyType(IContentTypeBase contentType, TypeProperty jetTypeProperty)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (jetTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(jetTypeProperty));
            }

            var definition = GetDataTypeDefinition(jetTypeProperty);

            var propertyType = new global::Umbraco.Core.Models.PropertyType(definition)
            {
                Name = jetTypeProperty.Name,
                Alias = jetTypeProperty.Alias,
                Mandatory = jetTypeProperty.Mandatory,
                Description = jetTypeProperty.Description,
                ValidationRegExp = jetTypeProperty.RegularExpression
            };

            if (jetTypeProperty.SortOrder.HasValue)
            {
                propertyType.SortOrder = jetTypeProperty.SortOrder.Value;
            }

            if (!string.IsNullOrWhiteSpace(jetTypeProperty.PropertyGroup))
            {
                contentType.AddPropertyType(propertyType, jetTypeProperty.PropertyGroup);
            }
            else
            {
                contentType.AddPropertyType(propertyType);
            }
        }

        /// <summary>
        /// Updates the property type to match the uJet type property.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="propertyType">The property type.</param>
        /// <param name="jetTypeProperty">The uJet type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, <paramref name="propertyType" />, or <paramref name="jetTypeProperty" /> are <c>null</c>.</exception>
        private static void UpdatePropertyType(IContentTypeBase contentType, global::Umbraco.Core.Models.PropertyType propertyType, TypeProperty jetTypeProperty)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (jetTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(jetTypeProperty));
            }

            if (!contentType.PropertyGroups.Contains(jetTypeProperty.PropertyGroup) || (contentType.PropertyGroups.Contains(jetTypeProperty.PropertyGroup) && !contentType.PropertyGroups[jetTypeProperty.PropertyGroup].PropertyTypes.Contains(jetTypeProperty.Alias)))
            {
                contentType.MovePropertyType(jetTypeProperty.Alias, jetTypeProperty.PropertyGroup);
            }

            propertyType.Name = jetTypeProperty.Name;
            propertyType.Alias = jetTypeProperty.Alias;
            propertyType.Mandatory = jetTypeProperty.Mandatory;
            propertyType.Description = jetTypeProperty.Description;
            propertyType.ValidationRegExp = jetTypeProperty.RegularExpression;

            if (jetTypeProperty.SortOrder.HasValue)
            {
                propertyType.SortOrder = jetTypeProperty.SortOrder.Value;
            }

            var definition = GetDataTypeDefinition(jetTypeProperty);

            if (propertyType.DataTypeDefinitionId != definition.Id)
            {
                propertyType.DataTypeDefinitionId = definition.Id;
            }
        }

        /// <summary>
        /// Gets the data type definition for the uJet type property.
        /// </summary>
        /// <param name="jetTypeProperty">The uJet type property.</param>
        /// <returns>The data type definition.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetTypeProperty" /> is <c>null</c>.</exception>
        /// <exception cref="Exception">Thrown if no data type definition can be found.</exception>
        private static IDataTypeDefinition GetDataTypeDefinition(TypeProperty jetTypeProperty)
        {
            if (jetTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(jetTypeProperty));
            }

            var definition = DataTypeDefinitionMappings.GetDefinition(jetTypeProperty.UIHint, jetTypeProperty.Type);

            if (definition == null)
            {
                throw new Exception($"There is no definition for type property of type {jetTypeProperty.Type}.");
            }

            return definition;
        }

        private global::Umbraco.Core.Models.PropertyType GetBasePropertyType(IContentTypeBase contentType, TypeProperty jetTypeProperty)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (jetTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(jetTypeProperty));
            }

            global::Umbraco.Core.Models.PropertyType propertyType;

            // Step 1; we try to find a match using the type ID (if any).
            if (jetTypeProperty.Id.HasValue)
            {
                // The type property has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = ContentTypeRepository.GetPropertyTypeId(jetTypeProperty.Id.Value);

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
            propertyType = contentType.PropertyTypes.SingleOrDefault(pt => pt.Alias == jetTypeProperty.Alias);

            // We might have found a match.
            return propertyType;
        }

        /// <summary>
        /// Synchronizes the property type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="jetTypeProperty">The uJet type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="jetTypeProperty" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="jetTypeProperty" /> identifier is <c>null</c>.</exception>
        private void SynchronizePropertyType(IContentTypeBase contentType, TypeProperty jetTypeProperty)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (jetTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(jetTypeProperty));
            }

            var propertyType = GetBasePropertyType(contentType, jetTypeProperty);

            if (propertyType == null)
            {
                CreatePropertyType(contentType, jetTypeProperty);
            }
            else
            {
                UpdatePropertyType(contentType, propertyType, jetTypeProperty);
            }
        }
    }
}
