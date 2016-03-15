// <copyright file="IContentTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;

    /// <summary>
    /// The <see cref="IContentTypeRepository" /> interface.
    /// </summary>
    public interface IContentTypeRepository
    {
        /// <summary>
        /// Gets the content type model identifier.
        /// </summary>
        /// <param name="contentTypeId">The content type identifier.</param>
        /// <returns>
        /// The content type model identifier.
        /// </returns>
        Guid? GetContentTypeModelId(int contentTypeId);

        /// <summary>
        /// Gets the property type model identifier.
        /// </summary>
        /// <param name="propertyTypeId">The property type identifier.</param>
        /// <returns>
        /// The property type model identifier.
        /// </returns>
        Guid? GetPropertyTypeModelId(int propertyTypeId);

        /// <summary>
        /// Gets the content type identifier.
        /// </summary>
        /// <param name="id">The content type model identifier.</param>
        /// <returns>The content type identifier.</returns>
        int? GetContentTypeId(Guid id);

        /// <summary>
        /// Gets the property type identifier.
        /// </summary>
        /// <param name="id">The property type model identifier.</param>
        /// <returns>The property type identifier.</returns>
        int? GetPropertyTypeId(Guid id);

        /// <summary>
        /// Sets the content type identifier.
        /// </summary>
        /// <param name="id">The content type model identifier.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        void SetContentTypeId(Guid id, int contentTypeId);

        /// <summary>
        /// Sets the property type identifier.
        /// </summary>
        /// <param name="id">The property type model identifier.</param>
        /// <param name="propertyTypeId">The property type identifier.</param>
        void SetPropertyTypeId(Guid id, int propertyTypeId);
    }
}
