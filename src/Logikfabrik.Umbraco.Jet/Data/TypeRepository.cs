// <copyright file="TypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;

    /// <summary>
    /// The <see cref="TypeRepository" /> class.
    /// </summary>
    public class TypeRepository : ITypeRepository
    {
        private static ITypeRepository instance;

        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly IDataTypeRepository _dataTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepository" /> class.
        /// </summary>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <param name="dataTypeRepository">The data type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeRepository" />, or <paramref name="dataTypeRepository" /> are <c>null</c>.</exception>
        internal TypeRepository(
            IContentTypeRepository contentTypeRepository,
            IDataTypeRepository dataTypeRepository)
        {
            if (contentTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(contentTypeRepository));
            }

            if (dataTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(dataTypeRepository));
            }

            _contentTypeRepository = contentTypeRepository;
            _dataTypeRepository = dataTypeRepository;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TypeRepository" /> class from being created.
        /// </summary>
        private TypeRepository()
            : this(new ContentTypeRepository(), new DataTypeRepository())
        {
        }

        /// <summary>
        /// Gets a singleton instance of the type repository.
        /// </summary>
        public static ITypeRepository Instance => instance ?? (instance = new TypeRepository());

        /// <summary>
        /// Gets the content type model identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The content type model identifier.
        /// </returns>
        public Guid? GetContentTypeModelId(int id)
        {
            return _contentTypeRepository.GetContentTypeModelId(id);
        }

        /// <summary>
        /// Gets the property type model identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The property type model identifier.
        /// </returns>
        public Guid? GetPropertyTypeModelId(int id)
        {
            return _contentTypeRepository.GetPropertyTypeModelId(id);
        }

        /// <summary>
        /// Gets the content type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The content type identifier.
        /// </returns>
        public int? GetContentTypeId(Guid id)
        {
            return _contentTypeRepository.GetContentTypeId(id);
        }

        /// <summary>
        /// Gets the property type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The property type identifier.
        /// </returns>
        public int? GetPropertyTypeId(Guid id)
        {
            return _contentTypeRepository.GetPropertyTypeId(id);
        }

        /// <summary>
        /// Sets the content type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="contentTypeId">The content type identifier.</param>
        public void SetContentTypeId(Guid id, int contentTypeId)
        {
            _contentTypeRepository.SetContentTypeId(id, contentTypeId);
        }

        /// <summary>
        /// Sets the property type identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="propertyTypeId">The property type identifier.</param>
        public void SetPropertyTypeId(Guid id, int propertyTypeId)
        {
            _contentTypeRepository.SetPropertyTypeId(id, propertyTypeId);
        }

        /// <summary>
        /// Gets the definition model identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The definition model identifier.
        /// </returns>
        public Guid? GetDefinitionModelId(int id)
        {
            return _dataTypeRepository.GetDefinitionModelId(id);
        }

        /// <summary>
        /// Gets the definition identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The definition identifier.
        /// </returns>
        public int? GetDefinitionId(Guid id)
        {
            return _dataTypeRepository.GetDefinitionId(id);
        }

        /// <summary>
        /// Sets the definition identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="definitionId">The definition identifier.</param>
        public void SetDefinitionId(Guid id, int definitionId)
        {
            _dataTypeRepository.SetDefinitionId(id, definitionId);
        }
    }
}