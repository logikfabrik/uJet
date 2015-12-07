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
        /// Gets the base type repository.
        /// </summary>
        /// <value>
        /// The base type repository.
        /// </value>
        protected IContentTypeRepository ContentTypeRepository { get; }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public abstract void Synchronize();

        /// <summary>
        /// Creates the content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBaseConstructor">The content type base constructor.</param>
        /// <param name="baseType">The base type.</param>
        /// <returns>The content type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBaseConstructor" />, or <paramref name="baseType" /> are <c>null</c>.</exception>
        protected IContentTypeBase CreateBaseType<T>(
            Func<IContentTypeBase> contentTypeBaseConstructor,
            BaseType<T> baseType)
            where T : BaseTypeAttribute
        {
            if (contentTypeBaseConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBaseConstructor));
            }

            if (baseType == null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            var t = contentTypeBaseConstructor();

            t.Name = baseType.Name;
            t.Alias = baseType.Alias;
            t.Description = baseType.Description;

            if (baseType.Icon != null)
            {
                t.Icon = baseType.Icon;
            }

            return t;
        }

        /// <summary>
        /// Updates the content type.
        /// </summary>
        /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="contentTypeBaseConstructor">The content type base constructor.</param>
        /// <param name="baseType">The base type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, <paramref name="contentTypeBaseConstructor" />, or <paramref name="baseType" /> are <c>null</c>.</exception>
        protected void UpdateBaseType<T>(
            IContentTypeBase contentTypeBase,
            Func<IContentTypeBase> contentTypeBaseConstructor,
            BaseType<T> baseType)
            where T : BaseTypeAttribute
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (contentTypeBaseConstructor == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBaseConstructor));
            }

            if (baseType == null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            contentTypeBase.Name = baseType.Name;
            contentTypeBase.Alias = baseType.Alias;
            contentTypeBase.Description = baseType.Description;

            var defaultContentType = contentTypeBaseConstructor();

            contentTypeBase.Icon = baseType.Icon ?? defaultContentType.Icon;
        }

        /// <summary>
        /// Synchronizes the property types.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="typeProperties">The type properties.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="typeProperties" /> are <c>null</c>.</exception>
        protected void SynchronizePropertyTypes(IContentTypeBase contentTypeBase, IEnumerable<TypeProperty> typeProperties)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (typeProperties == null)
            {
                throw new ArgumentNullException(nameof(typeProperties));
            }

            var p = typeProperties.ToArray();

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
                SynchronizePropertyTypeByAlias(contentTypeBase, property);
            }
        }

        /// <summary>
        /// Sets the property type identifier.
        /// </summary>
        /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="baseType">The base type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="baseType" /> are <c>null</c>.</exception>
        protected void SetPropertyTypeId<T>(IContentTypeBase contentTypeBase, BaseType<T> baseType)
            where T : BaseTypeAttribute
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            foreach (var property in baseType.Properties)
            {
                if (!property.Id.HasValue)
                {
                    return;
                }

                var id = ContentTypeRepository.GetPropertyTypeId(property.Id.Value);

                global::Umbraco.Core.Models.PropertyType pt;

                if (id.HasValue)
                {
                    pt = contentTypeBase.PropertyTypes.SingleOrDefault(type => type.Id == id.Value);

                    if (pt != null)
                    {
                        continue;
                    }

                    // The type property has been synchronized before, but for another type.
                    // This is possible if the property is tracked, but the type is not.
                }

                pt = contentTypeBase.PropertyTypes.Single(type => type.Alias == property.Alias);

                ContentTypeRepository.SetPropertyTypeId(property.Id.Value, pt.Id);
            }
        }

        /// <summary>
        /// Creates the type property.
        /// </summary>
        /// <param name="contentTypeBase">The content type base to add the property to.</param>
        /// <param name="typeProperty">The type property to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="typeProperty" /> are <c>null</c>.</exception>
        private static void CreatePropertyType(IContentTypeBase contentTypeBase, TypeProperty typeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (typeProperty == null)
            {
                throw new ArgumentNullException(nameof(typeProperty));
            }

            var definition = GetDataTypeDefinition(typeProperty);

            var propertyType = new global::Umbraco.Core.Models.PropertyType(definition)
            {
                Name = typeProperty.Name,
                Alias = typeProperty.Alias,
                Mandatory = typeProperty.Mandatory,
                Description = typeProperty.Description,
                ValidationRegExp = typeProperty.RegularExpression
            };

            if (typeProperty.SortOrder.HasValue)
            {
                propertyType.SortOrder = typeProperty.SortOrder.Value;
            }

            if (!string.IsNullOrWhiteSpace(typeProperty.PropertyGroup))
            {
                contentTypeBase.AddPropertyType(propertyType, typeProperty.PropertyGroup);
            }
            else
            {
                contentTypeBase.AddPropertyType(propertyType);
            }
        }

        /// <summary>
        /// Synchronizes the property type by alias.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="typeProperty">The type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="typeProperty" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="typeProperty" /> identifier is not <c>null</c>.</exception>
        private static void SynchronizePropertyTypeByAlias(IContentTypeBase contentTypeBase, TypeProperty typeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (typeProperty == null)
            {
                throw new ArgumentNullException(nameof(typeProperty));
            }

            if (typeProperty.Id.HasValue)
            {
                throw new ArgumentException("Type property ID must be null.", nameof(typeProperty));
            }

            var pt = contentTypeBase.PropertyTypes.FirstOrDefault(type => type.Alias == typeProperty.Alias);

            if (pt == null)
            {
                CreatePropertyType(contentTypeBase, typeProperty);
            }
            else
            {
                UpdatePropertyType(contentTypeBase, pt, typeProperty);
            }
        }

        /// <summary>
        /// Updates the property type.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="typeProperty">The type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, <paramref name="propertyType" />, or <paramref name="typeProperty" /> are <c>null</c>.</exception>
        private static void UpdatePropertyType(IContentTypeBase contentTypeBase, global::Umbraco.Core.Models.PropertyType propertyType, TypeProperty typeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (typeProperty == null)
            {
                throw new ArgumentNullException(nameof(typeProperty));
            }

            if (!contentTypeBase.PropertyGroups.Contains(typeProperty.PropertyGroup) ||
                (contentTypeBase.PropertyGroups.Contains(typeProperty.PropertyGroup) &&
                 !contentTypeBase.PropertyGroups[typeProperty.PropertyGroup].PropertyTypes.Contains(
                     typeProperty.Alias)))
            {
                contentTypeBase.MovePropertyType(typeProperty.Alias, typeProperty.PropertyGroup);
            }

            propertyType.Name = typeProperty.Name;
            propertyType.Alias = typeProperty.Alias;
            propertyType.Mandatory = typeProperty.Mandatory;
            propertyType.Description = typeProperty.Description;
            propertyType.ValidationRegExp = typeProperty.RegularExpression;

            if (typeProperty.SortOrder.HasValue)
            {
                propertyType.SortOrder = typeProperty.SortOrder.Value;
            }

            var definition = GetDataTypeDefinition(typeProperty);

            if (propertyType.DataTypeDefinitionId != definition.Id)
            {
                propertyType.DataTypeDefinitionId = definition.Id;
            }
        }

        /// <summary>
        /// Gets the data type definition.
        /// </summary>
        /// <param name="typeProperty">The type property.</param>
        /// <returns>The data type definition.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeProperty" /> is <c>null</c>.</exception>
        /// <exception cref="Exception">Thrown if no data type definition can be found.</exception>
        private static IDataTypeDefinition GetDataTypeDefinition(TypeProperty typeProperty)
        {
            if (typeProperty == null)
            {
                throw new ArgumentNullException(nameof(typeProperty));
            }

            var definition = DataTypeDefinitionMappings.GetDefinition(
                typeProperty.UIHint,
                typeProperty.Type);

            if (definition == null)
            {
                throw new Exception(
                    $"There is no definition for type property of type {typeProperty.Type}.");
            }

            return definition;
        }

        /// <summary>
        /// Synchronizes the property type by identifier.
        /// </summary>
        /// <param name="contentTypeBase">The content type base.</param>
        /// <param name="typeProperty">The type property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeBase" />, or <paramref name="typeProperty" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="typeProperty" /> identifier is <c>null</c>.</exception>
        private void SynchronizePropertyTypeById(IContentTypeBase contentTypeBase, TypeProperty typeProperty)
        {
            if (contentTypeBase == null)
            {
                throw new ArgumentNullException(nameof(contentTypeBase));
            }

            if (typeProperty == null)
            {
                throw new ArgumentNullException(nameof(typeProperty));
            }

            if (!typeProperty.Id.HasValue)
            {
                throw new ArgumentException("Type property ID cannot be null.", nameof(typeProperty));
            }

            global::Umbraco.Core.Models.PropertyType pt = null;

            var id = ContentTypeRepository.GetPropertyTypeId(typeProperty.Id.Value);

            if (id.HasValue)
            {
                // The type property has been synchronized before. Get the matching property type.
                // It might have been removed using the back office.
                pt = contentTypeBase.PropertyTypes.FirstOrDefault(type => type.Id == id.Value);
            }

            if (pt == null)
            {
                CreatePropertyType(contentTypeBase, typeProperty);
            }
            else
            {
                UpdatePropertyType(contentTypeBase, pt, typeProperty);
            }
        }
    }
}
