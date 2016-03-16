// <copyright file="PropertyTypeFinder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;

    /// <summary>
    /// The <see cref="PropertyTypeFinder" /> class.
    /// </summary>
    public class PropertyTypeFinder
    {
        private readonly EntityTypeComparer<global::Umbraco.Core.Models.PropertyType> _comparer = new EntityTypeComparer<global::Umbraco.Core.Models.PropertyType>();
        private readonly ITypeRepository _typeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTypeFinder" /> class.
        /// </summary>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeRepository" /> is <c>null</c>.</exception>
        public PropertyTypeFinder(ITypeRepository typeRepository)
        {
            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            _typeRepository = typeRepository;
        }

        /// <summary>
        /// Finds the property types matching the specified model.
        /// </summary>
        /// <param name="modelNeedle">The model to find the property types for.</param>
        /// <param name="propertyTypesHaystack">The haystack of property types.</param>
        /// <returns>The found property types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelNeedle" />, or <paramref name="propertyTypesHaystack" /> are <c>null</c>.</exception>
        public global::Umbraco.Core.Models.PropertyType[] Find(PropertyType modelNeedle, global::Umbraco.Core.Models.PropertyType[] propertyTypesHaystack)
        {
            if (modelNeedle == null)
            {
                throw new ArgumentNullException(nameof(modelNeedle));
            }

            if (propertyTypesHaystack == null)
            {
                throw new ArgumentNullException(nameof(propertyTypesHaystack));
            }

            if (modelNeedle.Id.HasValue)
            {
                var id = _typeRepository.GetPropertyTypeId(modelNeedle.Id.Value);

                if (id.HasValue)
                {
                    var propertyTypes = propertyTypesHaystack.Where(propertyType => propertyType.Id == id.Value).Distinct(_comparer).ToArray();

                    if (propertyTypes.Any())
                    {
                        return propertyTypes;
                    }
                }
            }

            return propertyTypesHaystack.Where(propertyType => propertyType.Alias.Equals(modelNeedle.Alias, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }
    }
}
