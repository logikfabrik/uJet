// <copyright file="DataTypeFinder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DataTypeFinder" /> class.
    /// </summary>
    public class DataTypeFinder
    {
        private readonly EntityTypeComparer<IDataTypeDefinition> _comparer = new EntityTypeComparer<IDataTypeDefinition>();
        private readonly ITypeRepository _typeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeFinder" /> class.
        /// </summary>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeRepository" /> is <c>null</c>.</exception>
        public DataTypeFinder(ITypeRepository typeRepository)
        {
            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            _typeRepository = typeRepository;
        }

        /// <summary>
        /// Finds the data types matching the specified model.
        /// </summary>
        /// <param name="modelNeedle">The model to find the data types for.</param>
        /// <param name="dataTypesHaystack">The haystack of data types.</param>
        /// <returns>The data types found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelNeedle" />, or <paramref name="dataTypesHaystack" /> are <c>null</c>.</exception>
        public IDataTypeDefinition[] Find(DataType modelNeedle, IDataTypeDefinition[] dataTypesHaystack)
        {
            if (modelNeedle == null)
            {
                throw new ArgumentNullException(nameof(modelNeedle));
            }

            if (dataTypesHaystack == null)
            {
                throw new ArgumentNullException(nameof(dataTypesHaystack));
            }

            if (modelNeedle.Id.HasValue)
            {
                var id = _typeRepository.GetDefinitionId(modelNeedle.Id.Value);

                if (id.HasValue)
                {
                    var dataTypes = dataTypesHaystack.Where(dataType => dataType.Id == id.Value).Distinct(_comparer).ToArray();

                    if (dataTypes.Any())
                    {
                        return dataTypes;
                    }
                }
            }

            return dataTypesHaystack.Where(dataType => dataType.Name.Equals(modelNeedle.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }
    }
}