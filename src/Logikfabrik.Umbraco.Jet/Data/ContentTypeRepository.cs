// <copyright file="ContentTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using global::Umbraco.Core.Persistence;

    /// <summary>
    /// The <see cref="ContentTypeRepository" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class ContentTypeRepository : IContentTypeRepository
    {
        private readonly IDatabaseWrapper _databaseWrapper;
        private readonly HashSet<Tuple<Guid, int>> _contentTypeId;
        private readonly HashSet<Tuple<Guid, int>> _propertyTypeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeRepository" /> class.
        /// </summary>
        /// <param name="databaseWrapper">The database wrapper.</param>
        public ContentTypeRepository(IDatabaseWrapper databaseWrapper)
        {
            EnsureArg.IsNotNull(databaseWrapper);

            _databaseWrapper = databaseWrapper;
            _contentTypeId = new HashSet<Tuple<Guid, int>>();
            _propertyTypeId = new HashSet<Tuple<Guid, int>>();
        }

        /// <inheritdoc />
        public Guid? GetContentTypeModelId(int contentTypeId)
        {
            var modelId = _contentTypeId.SingleOrDefault(t => t.Item2 == contentTypeId)?.Item1;

            if (modelId.HasValue)
            {
                return modelId;
            }

            modelId = GetContentType(contentTypeId)?.Id;

            if (modelId.HasValue)
            {
                _contentTypeId.Add(new Tuple<Guid, int>(modelId.Value, contentTypeId));
            }

            return modelId;
        }

        /// <inheritdoc />
        public Guid? GetPropertyTypeModelId(int propertyTypeId)
        {
            var modelId = _propertyTypeId.SingleOrDefault(id => id.Item2 == propertyTypeId)?.Item1;

            if (modelId.HasValue)
            {
                return modelId;
            }

            modelId = GetPropertyType(propertyTypeId)?.Id;

            if (modelId.HasValue)
            {
                _propertyTypeId.Add(new Tuple<Guid, int>(modelId.Value, propertyTypeId));
            }

            return modelId;
        }

        /// <inheritdoc />
        public int? GetContentTypeId(Guid id)
        {
            var contentTypeId = _contentTypeId.SingleOrDefault(t => t.Item1 == id)?.Item2;

            if (contentTypeId.HasValue)
            {
                return contentTypeId;
            }

            contentTypeId = GetContentType(id)?.ContentTypeId;

            if (contentTypeId.HasValue)
            {
                _contentTypeId.Add(new Tuple<Guid, int>(id, contentTypeId.Value));
            }

            return contentTypeId;
        }

        /// <inheritdoc />
        public int? GetPropertyTypeId(Guid id)
        {
            var propertyTypeId = _propertyTypeId.SingleOrDefault(t => t.Item1 == id)?.Item2;

            if (propertyTypeId.HasValue)
            {
                return propertyTypeId;
            }

            propertyTypeId = GetPropertyType(id)?.PropertyTypeId;

            if (propertyTypeId.HasValue)
            {
                _propertyTypeId.Add(new Tuple<Guid, int>(id, propertyTypeId.Value));
            }

            return propertyTypeId;
        }

        /// <inheritdoc />
        public void SetContentTypeId(Guid id, int contentTypeId)
        {
            var contentType = new ContentType { Id = id, ContentTypeId = contentTypeId };

            _databaseWrapper.CreateTable<ContentType>();
            _databaseWrapper.Insert(contentType, id);
        }

        /// <inheritdoc />
        public void SetPropertyTypeId(Guid id, int propertyTypeId)
        {
            var propertyType = new PropertyType { Id = id, PropertyTypeId = propertyTypeId };

            _databaseWrapper.CreateTable<PropertyType>();
            _databaseWrapper.Insert(propertyType, id);
        }

        private ContentType GetContentType(int contentTypeId)
        {
            if (!_databaseWrapper.TableExists<ContentType>())
            {
                return null;
            }

            var sql = new Sql().Where<ContentType>(contentType => contentType.ContentTypeId == contentTypeId, _databaseWrapper.SyntaxProvider);

            return _databaseWrapper.Get<ContentType>(sql);
        }

        private ContentType GetContentType(Guid id)
        {
            return Get<ContentType>(id);
        }

        private PropertyType GetPropertyType(int propertyTypeId)
        {
            if (!_databaseWrapper.TableExists<PropertyType>())
            {
                return null;
            }

            var sql = new Sql().Where<PropertyType>(propertyType => propertyType.PropertyTypeId == propertyTypeId, _databaseWrapper.SyntaxProvider);

            return _databaseWrapper.Get<PropertyType>(sql);
        }

        private PropertyType GetPropertyType(Guid id)
        {
            return Get<PropertyType>(id);
        }

        private T Get<T>(Guid id)
            where T : class
        {
            return !_databaseWrapper.TableExists<T>() ? null : _databaseWrapper.Get<T>(id);
        }
    }
}