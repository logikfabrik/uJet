// <copyright file="ContentTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Extensions;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Mappings;

    /// <summary>
    /// The <see cref="ContentTypeSynchronizationService" /> class.
    /// </summary>
    public abstract class ContentTypeSynchronizationService : ISynchronizationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, or <paramref name="contentTypeRepository" /> are <c>null</c>.</exception>
        protected ContentTypeSynchronizationService(IContentTypeService contentTypeService, IContentTypeRepository contentTypeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (contentTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(contentTypeRepository));
            }

            ContentTypeService = contentTypeService;
            ContentTypeRepository = contentTypeRepository;
        }

        /// <summary>
        /// Gets the content type service.
        /// </summary>
        /// <value>
        /// The content type service.
        /// </value>
        protected IContentTypeService ContentTypeService { get; }

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
        /// Creates the content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBaseConstructor">The content type base constructor.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>The content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBaseConstructor" />, or <paramref name="contentType" /> are <c>null</c>.</exception>
        protected IContentTypeBase CreateContentType<T>(
            Func<IContentTypeBase> contentTypeBaseConstructor,
            ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (contentTypeBaseConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBaseConstructor));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var t = contentTypeBaseConstructor();

            t.Name = contentType.Name;
            t.Alias = contentType.Alias;
            t.Description = contentType.Description;
            t.AllowedAsRoot = contentType.AllowedAsRoot;

            if (contentType.Icon != null)
            {
                t.Icon = contentType.Icon;
            }

            if (contentType.Thumbnail != null)
            {
                t.Thumbnail = contentType.Thumbnail;
            }

            SynchronizePropertyTypes(t, contentType.Properties);

            return t;
        }

        /// <summary>
        /// Updates the content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="contentTypeBaseConstructor">The content type base constructor.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, <paramref name="contentTypeBaseConstructor" />, or <paramref name="contentType" /> are <c>null</c>.</exception>
        protected void UpdateContentType<T>(
            IContentTypeBase contentTypeBase,
            Func<IContentTypeBase> contentTypeBaseConstructor,
            ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (contentTypeBaseConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBaseConstructor));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            contentTypeBase.Name = contentType.Name;
            contentTypeBase.Alias = contentType.Alias;
            contentTypeBase.Description = contentType.Description;
            contentTypeBase.AllowedAsRoot = contentType.AllowedAsRoot;

            var defaultContentType = contentTypeBaseConstructor();

            contentTypeBase.Icon = contentType.Icon ?? defaultContentType.Icon;
            contentTypeBase.Thumbnail = contentType.Thumbnail ?? defaultContentType.Thumbnail;

            SynchronizePropertyTypes(contentTypeBase, contentType.Properties);
        }

        /// <summary>
        /// Sets the allowed content types.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBases">The content type bases.</param>
        /// <param name="contentTypes">The content types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBases" />, or <paramref name="contentTypes" /> are <c>null</c>.</exception>
        protected void SetAllowedContentTypes<T>(
            IContentTypeBase[] contentTypeBases,
            IEnumerable<ContentType<T>> contentTypes)
            where T : ContentTypeAttribute
        {
            if (contentTypeBases == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBases));
            }

            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            foreach (var contentType in contentTypes.Where(dt => dt.AllowedChildNodeTypes.Any()))
            {
                var contentTypeBase = contentTypeBases.FirstOrDefault(ct => ct.Alias == contentType.Alias);

                if (contentTypeBase == null)
                {
                    continue;
                }

                contentTypeBase.AllowedContentTypes = GetAllowedChildNodeContentTypes(contentType.AllowedChildNodeTypes);

                if (contentType.Type.IsDocumentType())
                {
                    ContentTypeService.Save((IContentType)contentTypeBase);
                }
                else if (contentType.Type.IsMediaType())
                {
                    ContentTypeService.Save((IMediaType)contentTypeBase);
                }
            }
        }

        /// <summary>
        /// Sets the property type identifier.
        /// </summary>
        /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="contentType" /> are <c>null</c>.</exception>
        protected void SetPropertyTypeId<T>(IContentTypeBase contentTypeBase, ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            foreach (var property in contentType.Properties)
            {
                if (!property.Id.HasValue)
                {
                    return;
                }

                var id = ContentTypeRepository.GetPropertyTypeId(property.Id.Value);

                PropertyType pt;

                if (id.HasValue)
                {
                    pt = contentTypeBase.PropertyTypes.SingleOrDefault(type => type.Id == id.Value);

                    if (pt != null)
                    {
                        continue;
                    }

                    // The content/media type property has been synchronized before, but for another content/media type.
                    // This is possible if the property is tracked, but the content/media type is not.
                }

                pt = contentTypeBase.PropertyTypes.Single(type => type.Alias == property.Alias);

                ContentTypeRepository.SetPropertyTypeId(property.Id.Value, pt.Id);
            }
        }

        /// <summary>
        /// Creates the content type property.
        /// </summary>
        /// <param name="contentTypeBase">The content type base to add the property to.</param>
        /// <param name="contentTypeProperty">The content type property to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="contentTypeProperty" /> are <c>null</c>.</exception>
        private static void CreatePropertyType(IContentTypeBase contentTypeBase, ContentTypeProperty contentTypeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (contentTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProperty));
            }

            var definition = GetDataTypeDefinition(contentTypeProperty);

            var propertyType = new PropertyType(definition)
            {
                Name = contentTypeProperty.Name,
                Alias = contentTypeProperty.Alias,
                Mandatory = contentTypeProperty.Mandatory,
                Description = contentTypeProperty.Description,
                ValidationRegExp = contentTypeProperty.RegularExpression
            };

            if (contentTypeProperty.SortOrder.HasValue)
            {
                propertyType.SortOrder = contentTypeProperty.SortOrder.Value;
            }

            if (!string.IsNullOrWhiteSpace(contentTypeProperty.PropertyGroup))
            {
                contentTypeBase.AddPropertyType(propertyType, contentTypeProperty.PropertyGroup);
            }
            else
            {
                contentTypeBase.AddPropertyType(propertyType);
            }
        }

        /// <summary>
        /// Synchronizes the property type by name.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="contentTypeProperty">The content type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="contentTypeProperty" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="contentTypeProperty" /> identifier is not <c>null</c>.</exception>
        private static void SynchronizePropertyTypeByName(IContentTypeBase contentTypeBase, ContentTypeProperty contentTypeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (contentTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProperty));
            }

            if (contentTypeProperty.Id.HasValue)
            {
                throw new ArgumentException("Content type property ID must be null.", nameof(contentTypeProperty));
            }

            var pt = contentTypeBase.PropertyTypes.FirstOrDefault(type => type.Alias == contentTypeProperty.Alias);

            if (pt == null)
            {
                CreatePropertyType(contentTypeBase, contentTypeProperty);
            }
            else
            {
                UpdatePropertyType(contentTypeBase, pt, contentTypeProperty);
            }
        }

        /// <summary>
        /// Updates the property type.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="contentTypeProperty">The content type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, <paramref name="propertyType" />, or <paramref name="contentTypeProperty" /> are <c>null</c>.</exception>
        private static void UpdatePropertyType(IContentTypeBase contentTypeBase, PropertyType propertyType, ContentTypeProperty contentTypeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (contentTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProperty));
            }

            if (!contentTypeBase.PropertyGroups.Contains(contentTypeProperty.PropertyGroup) ||
                (contentTypeBase.PropertyGroups.Contains(contentTypeProperty.PropertyGroup) &&
                 !contentTypeBase.PropertyGroups[contentTypeProperty.PropertyGroup].PropertyTypes.Contains(
                     contentTypeProperty.Alias)))
            {
                contentTypeBase.MovePropertyType(contentTypeProperty.Alias, contentTypeProperty.PropertyGroup);
            }

            propertyType.Name = contentTypeProperty.Name;
            propertyType.Alias = contentTypeProperty.Alias;
            propertyType.Mandatory = contentTypeProperty.Mandatory;
            propertyType.Description = contentTypeProperty.Description;
            propertyType.ValidationRegExp = contentTypeProperty.RegularExpression;

            if (contentTypeProperty.SortOrder.HasValue)
            {
                propertyType.SortOrder = contentTypeProperty.SortOrder.Value;
            }

            var definition = GetDataTypeDefinition(contentTypeProperty);

            // ReSharper disable once RedundantCheckBeforeAssignment
            if (propertyType.DataTypeDefinitionId != definition.Id)
            {
                propertyType.DataTypeDefinitionId = definition.Id;
            }
        }

        /// <summary>
        /// Gets the data type definition.
        /// </summary>
        /// <param name="contentTypeProperty">The content type property.</param>
        /// <returns>The data type definition.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeProperty" /> is <c>null</c>.</exception>
        /// <exception cref="Exception">Thrown if no data type definition can be found.</exception>
        private static IDataTypeDefinition GetDataTypeDefinition(ContentTypeProperty contentTypeProperty)
        {
            if (contentTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProperty));
            }

            var definition = DataTypeDefinitionMappings.GetDefinition(
                contentTypeProperty.UIHint,
                contentTypeProperty.Type);

            if (definition == null)
            {
                throw new Exception(
                    $"There is no definition for content type property of type {contentTypeProperty.Type}.");
            }

            return definition;
        }

        /// <summary>
        /// Synchronizes the property types.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="contentTypeProperties">The content type properties.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="contentTypeProperties" /> are <c>null</c>.</exception>
        private void SynchronizePropertyTypes(IContentTypeBase contentTypeBase, IEnumerable<ContentTypeProperty> contentTypeProperties)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (contentTypeProperties == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProperties));
            }

            var p = contentTypeProperties.ToArray();

            if (!p.Any())
            {
                return;
            }

            foreach (var property in p.Where(pt => pt.Id.HasValue))
            {
                SynchronizePropertyTypeById(contentTypeBase, property);
            }

            foreach (var property in p.Where(pt => !pt.Id.HasValue))
            {
                SynchronizePropertyTypeByName(contentTypeBase, property);
            }
        }

        /// <summary>
        /// Synchronizes the property type by identifier.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="contentTypeProperty">The content type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="contentTypeProperty" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="contentTypeProperty" /> identifier is <c>null</c>.</exception>
        private void SynchronizePropertyTypeById(IContentTypeBase contentTypeBase, ContentTypeProperty contentTypeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (contentTypeProperty == null)
            {
                throw new ArgumentNullException(nameof(contentTypeProperty));
            }

            if (!contentTypeProperty.Id.HasValue)
            {
                throw new ArgumentException("Content type property ID cannot be null.", nameof(contentTypeProperty));
            }

            PropertyType pt = null;

            var id = ContentTypeRepository.GetPropertyTypeId(contentTypeProperty.Id.Value);

            if (id.HasValue)
            {
                // The content/media type property has been synchronized before. Get the matching property type.
                // It might have been removed using the back office.
                pt = contentTypeBase.PropertyTypes.FirstOrDefault(type => type.Id == id.Value);
            }

            if (pt == null)
            {
                CreatePropertyType(contentTypeBase, contentTypeProperty);
            }
            else
            {
                UpdatePropertyType(contentTypeBase, pt, contentTypeProperty);
            }
        }

        /// <summary>
        /// Gets the allowed child node content types.
        /// </summary>
        /// <param name="allowedChildNodeTypes">The allowed child node types.</param>
        /// <returns>The allowed child node content types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="allowedChildNodeTypes" /> is <c>null</c>.</exception>
        private IEnumerable<ContentTypeSort> GetAllowedChildNodeContentTypes(IEnumerable<Type> allowedChildNodeTypes)
        {
            if (allowedChildNodeTypes == null)
            {
                throw new ArgumentNullException(nameof(allowedChildNodeTypes));
            }

            var nodeTypes = new List<ContentTypeSort>();

            foreach (var allowedChildNodeType in allowedChildNodeTypes)
            {
                var contentType = ContentTypeService.GetContentType(allowedChildNodeType.Name.Alias());

                if (contentType == null)
                {
                    continue;
                }

                nodeTypes.Add(
                    new ContentTypeSort(
                        new Lazy<int>(() => contentType.Id), nodeTypes.Count, contentType.Alias));
            }

            return nodeTypes;
        }
    }
}