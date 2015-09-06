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
    /// Base type for content type synchronization services.
    /// </summary>
    public abstract class ContentTypeSynchronizationService : ISynchronizationService
    {
        private readonly IContentTypeService contentTypeService;
        private readonly IContentTypeRepository contentTypeRepository;

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

            this.contentTypeService = contentTypeService;
            this.contentTypeRepository = contentTypeRepository;
        }

        public abstract void Synchronize();

        /// <summary>
        /// Creates a content type.
        /// </summary>
        /// <typeparam name="T">The reflected content type type.</typeparam>
        /// <param name="contentTypeBaseConstructor">The content type constructor.</param>
        /// <param name="contentType">The reflected content type to create.</param>
        /// <returns>A content type.</returns>
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
        /// Updates a content type.
        /// </summary>
        /// <typeparam name="T">The reflected content type type.</typeparam>
        /// <param name="contentTypeBase">The content type to update.</param>
        /// <param name="contentTypeBaseConstructor">The content type constructor.</param>
        /// <param name="contentType">The reflected content type to update.</param>
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
        /// Sets allowed content types.
        /// </summary>
        /// <typeparam name="T">The reflected content type type.</typeparam>
        /// <param name="contentTypeBases">Content types.</param>
        /// <param name="contentTypes">Reflected content types.</param>
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
                    contentTypeService.Save((IContentType)contentTypeBase);
                }
                else if (contentType.Type.IsMediaType())
                {
                    contentTypeService.Save((IMediaType)contentTypeBase);
                }
            }
        }

        protected void SetPropertyTypeId<T>(IContentTypeBase contentTypeBase, ContentType<T> contentType)
            where T : ContentTypeAttribute
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            foreach (var property in contentType.Properties.Where(p => p.Id.HasValue))
            {
                // ReSharper disable once PossibleInvalidOperationException
                var id = contentTypeRepository.GetPropertyTypeId(property.Id.Value);

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

                contentTypeRepository.SetPropertyTypeId(property.Id.Value, pt.Id);
            }
        }

        /// <summary>
        /// Creates a property type.
        /// </summary>
        /// <param name="contentTypeBase">The content type to add the property to.</param>
        /// <param name="contentTypeProperty">The reflected content type property to create.</param>
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

            var definition = GetDataDefinition(contentTypeProperty);

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
        /// Updates a property type.
        /// </summary>
        /// <param name="contentTypeBase">The content type to update.</param>
        /// <param name="propertyType">The property type to update.</param>
        /// <param name="contentTypeProperty">The reflected content type property.</param>
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

            var definition = GetDataDefinition(contentTypeProperty);

            // ReSharper disable once RedundantCheckBeforeAssignment
            if (propertyType.DataTypeDefinitionId != definition.Id)
            {
                propertyType.DataTypeDefinitionId = definition.Id;
            }
        }

        /// <summary>
        /// Gets a data type definition.
        /// </summary>
        /// <param name="contentTypeProperty">The reflected content type property to create.</param>
        /// <returns>A data type definition.</returns>
        private static IDataTypeDefinition GetDataDefinition(ContentTypeProperty contentTypeProperty)
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

            var id = contentTypeRepository.GetPropertyTypeId(contentTypeProperty.Id.Value);

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

        private IEnumerable<ContentTypeSort> GetAllowedChildNodeContentTypes(IEnumerable<Type> allowedChildNodeTypes)
        {
            if (allowedChildNodeTypes == null)
            {
                throw new ArgumentNullException(nameof(allowedChildNodeTypes));
            }

            var nodeTypes = new List<ContentTypeSort>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var allowedChildNodeType in allowedChildNodeTypes)
            {
                var contentType = contentTypeService.GetContentType(allowedChildNodeType.Name.Alias());

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