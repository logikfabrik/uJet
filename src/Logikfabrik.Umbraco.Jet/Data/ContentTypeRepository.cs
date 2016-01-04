// <copyright file="ContentTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Core.Persistence;

    /// <summary>
    /// The <see cref="ContentTypeRepository" /> class.
    /// </summary>
    public class ContentTypeRepository : IContentTypeRepository
    {
        private readonly IDatabaseWrapper _databaseWrapper;

        private readonly HashSet<Tuple<Guid, int>> _contentTypeId;
        private readonly HashSet<Tuple<Guid, int>> _propertyTypeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeRepository" /> class.
        /// </summary>
        public ContentTypeRepository()
            : this(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeRepository" /> class.
        /// </summary>
        /// <param name="databaseWrapper">The database wrapper.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="databaseWrapper" /> is <code>null</code>.</exception>
        public ContentTypeRepository(IDatabaseWrapper databaseWrapper)
        {
            if (databaseWrapper == null)
            {
                throw new ArgumentNullException(nameof(databaseWrapper));
            }

            _databaseWrapper = databaseWrapper;
            _contentTypeId = new HashSet<Tuple<Guid, int>>();
            _propertyTypeId = new HashSet<Tuple<Guid, int>>();
        }

        /// <summary>
        /// Gets the content type model identifier.
        /// </summary>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <returns>
        /// The content type model identifier.
        /// </returns>
        public Guid? GetContentTypeModelId(int contentTypeId)
        {
            var modelId = _contentTypeId.SingleOrDefault(t => t.Item2 == contentTypeId)?.Item1;

            if (modelId.HasValue)
            {
                return modelId;
            }

            modelId = GetContentTypeByContentTypeId(contentTypeId)?.Id;

            if (modelId.HasValue)
            {
                _contentTypeId.Add(new Tuple<Guid, int>(modelId.Value, contentTypeId));
            }

            return modelId;
        }

        /// <summary>
        /// Gets the property type model identifier.
        /// </summary>
        /// <param name="propertyTypeId">The property type identifier.</param>
        /// <returns>
        /// The property type model identifier.
        /// </returns>
        public Guid? GetPropertyTypeModelId(int propertyTypeId)
        {
            var modelId = _propertyTypeId.SingleOrDefault(t => t.Item2 == propertyTypeId)?.Item1;

            if (modelId.HasValue)
            {
                return modelId;
            }

            modelId = GetPropertyTypeByPropertyTypeId(propertyTypeId)?.Id;

            if (modelId.HasValue)
            {
                _propertyTypeId.Add(new Tuple<Guid, int>(modelId.Value, propertyTypeId));
            }

            return modelId;
        }

        /// <summary>
        /// Gets the content type identifier.
        /// </summary>
        /// <param name="id">The content type model identifier.</param>
        /// <returns>The content type identifier.</returns>
        public int? GetContentTypeId(Guid id)
        {
            var contentTypeId = _contentTypeId.SingleOrDefault(t => t.Item1 == id)?.Item2;

            if (contentTypeId.HasValue)
            {
                return contentTypeId;
            }

            contentTypeId = GetContentTypeById(id)?.ContentTypeId;

            if (contentTypeId.HasValue)
            {
                _contentTypeId.Add(new Tuple<Guid, int>(id, contentTypeId.Value));
            }

            return contentTypeId;
        }

        /// <summary>
        /// Gets the property type identifier.
        /// </summary>
        /// <param name="id">The property model identifier.</param>
        /// <returns>The property type identifier.</returns>
        public int? GetPropertyTypeId(Guid id)
        {
            var propertyTypeId = _propertyTypeId.SingleOrDefault(t => t.Item1 == id)?.Item2;

            if (propertyTypeId.HasValue)
            {
                return propertyTypeId;
            }

            propertyTypeId = GetPropertyTypeById(id)?.PropertyTypeId;

            if (propertyTypeId.HasValue)
            {
                _propertyTypeId.Add(new Tuple<Guid, int>(id, propertyTypeId.Value));
            }

            return propertyTypeId;
        }

        /// <summary>
        /// Sets the content type identifier.
        /// </summary>
        /// <param name="id">The content type model identifier.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        public void SetContentTypeId(Guid id, int contentTypeId)
        {
            var contentType = new ContentType { Id = id, ContentTypeId = contentTypeId };

            _databaseWrapper.CreateTable<ContentType>();
            _databaseWrapper.Insert(contentType, id);
        }

        /// <summary>
        /// Sets the property type identifier.
        /// </summary>
        /// <param name="id">The property type model identifier.</param>
        /// <param name="propertyTypeId">The property type identifier.</param>
        public void SetPropertyTypeId(Guid id, int propertyTypeId)
        {
            var propertyType = new PropertyType { Id = id, PropertyTypeId = propertyTypeId };

            _databaseWrapper.CreateTable<PropertyType>();
            _databaseWrapper.Insert(propertyType, id);
        }

        /// <summary>
        /// Gets the content type with the specified content type identifier.
        /// </summary>
        /// <param name="id">The content type identifier.</param>
        /// <returns>The content type with the specified content type identifier.</returns>
        internal virtual ContentType GetContentTypeByContentTypeId(int id)
        {
            if (!_databaseWrapper.TableExists<ContentType>())
            {
                return null;
            }

            var sql = new Sql().Where<ContentType>(ct => ct.ContentTypeId == id);

            return _databaseWrapper.Get<ContentType>(sql);
        }

        /// <summary>
        /// Gets the content type with the specified identifier.
        /// </summary>
        /// <param name="id">The content type model identifier.</param>
        /// <returns>The content type with the specified identifier.</returns>
        internal virtual ContentType GetContentTypeById(Guid id)
        {
            return !_databaseWrapper.TableExists<ContentType>() ? null : _databaseWrapper.Get<ContentType>(id);
        }

        /// <summary>
        /// Gets the property type with the specified property type identifier.
        /// </summary>
        /// <param name="id">The property type identifier.</param>
        /// <returns>The property type with the specified property type identifier.</returns>
        internal virtual PropertyType GetPropertyTypeByPropertyTypeId(int id)
        {
            if (!_databaseWrapper.TableExists<PropertyType>())
            {
                return null;
            }

            var sql = new Sql().Where<PropertyType>(ct => ct.PropertyTypeId == id);

            return _databaseWrapper.Get<PropertyType>(sql);
        }

        /// <summary>
        /// Gets the property type with the specified identifier.
        /// </summary>
        /// <param name="id">The property type model identifier.</param>
        /// <returns>The property type with the specified identifier.</returns>
        internal virtual PropertyType GetPropertyTypeById(Guid id)
        {
            return !_databaseWrapper.TableExists<PropertyType>() ? null : _databaseWrapper.Get<PropertyType>(id);
        }
    }
}
