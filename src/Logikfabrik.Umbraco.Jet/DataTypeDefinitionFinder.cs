// <copyright file="DataTypeDefinitionFinder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using EnsureThat;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DataTypeDefinitionFinder" /> class.
    /// </summary>
    public class DataTypeDefinitionFinder : IDataTypeDefinitionFinder
    {
        private readonly ITypeRepository _typeRepository;
        private readonly EntityTypeComparer<IDataTypeDefinition> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeDefinitionFinder" /> class.
        /// </summary>
        /// <param name="typeRepository">The type repository.</param>
        public DataTypeDefinitionFinder(ITypeRepository typeRepository)
        {
            Ensure.That(typeRepository).IsNotNull();

            _typeRepository = typeRepository;
            _comparer = new EntityTypeComparer<IDataTypeDefinition>();
        }

        /// <inheritdoc />
        public IDataTypeDefinition[] Find(DataType modelNeedle, IDataTypeDefinition[] dataTypeDefinitionsHaystack)
        {
            Ensure.That(modelNeedle).IsNotNull();
            Ensure.That(dataTypeDefinitionsHaystack).IsNotNull();

            // ReSharper disable once InvertIf
            if (modelNeedle.Id.HasValue)
            {
                var id = _typeRepository.GetDefinitionId(modelNeedle.Id.Value);

                // ReSharper disable once InvertIf
                if (id.HasValue)
                {
                    var dataTypes = dataTypeDefinitionsHaystack.Where(dataType => dataType.Id == id.Value).Distinct(_comparer).ToArray();

                    if (dataTypes.Any())
                    {
                        return dataTypes;
                    }
                }
            }

            return dataTypeDefinitionsHaystack.Where(dataType => dataType.Name != null && dataType.Name.Equals(modelNeedle.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }
    }
}