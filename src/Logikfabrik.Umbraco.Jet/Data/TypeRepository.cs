// <copyright file="TypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using EnsureThat;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="TypeRepository" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class TypeRepository : ITypeRepository
    {
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly IDataTypeRepository _dataTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRepository" /> class.
        /// </summary>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <param name="dataTypeRepository">The data type repository.</param>
        [UsedImplicitly]
        public TypeRepository(
            IContentTypeRepository contentTypeRepository,
            IDataTypeRepository dataTypeRepository)
        {
            Ensure.That(contentTypeRepository).IsNotNull();
            Ensure.That(dataTypeRepository).IsNotNull();

            _contentTypeRepository = contentTypeRepository;
            _dataTypeRepository = dataTypeRepository;
        }

        /// <inheritdoc />
        public Guid? GetContentTypeModelId(int contentTypeId)
        {
            return _contentTypeRepository.GetContentTypeModelId(contentTypeId);
        }

        /// <inheritdoc />
        public Guid? GetPropertyTypeModelId(int propertyTypeId)
        {
            return _contentTypeRepository.GetPropertyTypeModelId(propertyTypeId);
        }

        /// <inheritdoc />
        public Guid? GetDefinitionTypeModelId(int definitionId)
        {
            return _dataTypeRepository.GetDefinitionTypeModelId(definitionId);
        }

        /// <inheritdoc />
        public int? GetContentTypeId(Guid id)
        {
            return _contentTypeRepository.GetContentTypeId(id);
        }

        /// <inheritdoc />
        public int? GetPropertyTypeId(Guid id)
        {
            return _contentTypeRepository.GetPropertyTypeId(id);
        }

        /// <inheritdoc />
        public int? GetDefinitionId(Guid id)
        {
            return _dataTypeRepository.GetDefinitionId(id);
        }

        /// <inheritdoc />
        public void SetContentTypeId(Guid id, int contentTypeId)
        {
            _contentTypeRepository.SetContentTypeId(id, contentTypeId);
        }

        /// <inheritdoc />
        public void SetPropertyTypeId(Guid id, int propertyTypeId)
        {
            _contentTypeRepository.SetPropertyTypeId(id, propertyTypeId);
        }

        /// <inheritdoc />
        public void SetDefinitionId(Guid id, int definitionId)
        {
            _dataTypeRepository.SetDefinitionId(id, definitionId);
        }
    }
}