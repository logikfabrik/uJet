// <copyright file="ContentTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;

    /// <summary>
    /// The <see cref="ContentTypeRepository" /> class.
    /// </summary>
    public class ContentTypeRepository : IContentTypeRepository
    {
        private readonly IDatabaseWrapper _databaseWrapper;

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
        }

        /// <summary>
        /// Gets the content type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The content type identifier.</returns>
        public int? GetContentTypeId(Guid id)
        {
            if (!_databaseWrapper.TableExists<ContentType>())
            {
                return null;
            }

            var contentType = _databaseWrapper.Get<ContentType>(id);

            return contentType?.ContentTypeId;
        }

        /// <summary>
        /// Gets the property type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The property type identifier.</returns>
        public int? GetPropertyTypeId(Guid id)
        {
            if (!_databaseWrapper.TableExists<PropertyType>())
            {
                return null;
            }

            var propertyType = _databaseWrapper.Get<PropertyType>(id);

            return propertyType?.PropertyTypeId;
        }

        /// <summary>
        /// Sets the content type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
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
        /// <param name="id">The identifier.</param>
        /// <param name="propertyTypeId">The property type identifier.</param>
        public void SetPropertyTypeId(Guid id, int propertyTypeId)
        {
            var propertyType = new PropertyType { Id = id, PropertyTypeId = propertyTypeId };

            _databaseWrapper.CreateTable<PropertyType>();
            _databaseWrapper.Insert(propertyType, id);
        }
    }
}
