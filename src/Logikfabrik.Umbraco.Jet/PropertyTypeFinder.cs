// <copyright file="PropertyTypeFinder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using EnsureThat;
    using Logging;

    /// <summary>
    /// The <see cref="PropertyTypeFinder" /> class.
    /// </summary>
    public class PropertyTypeFinder : IPropertyTypeFinder
    {
        private readonly ILogService _logService;
        private readonly ITypeRepository _typeRepository;
        private readonly EntityTypeComparer<global::Umbraco.Core.Models.PropertyType> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTypeFinder" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="typeRepository">The type repository.</param>
        public PropertyTypeFinder(ILogService logService, ITypeRepository typeRepository)
        {
            Ensure.That(logService).IsNotNull();
            Ensure.That(typeRepository).IsNotNull();

            _logService = logService;
            _typeRepository = typeRepository;
            _comparer = new EntityTypeComparer<global::Umbraco.Core.Models.PropertyType>();
        }

        /// <inheritdoc />
        public global::Umbraco.Core.Models.PropertyType[] Find(PropertyTypeModel modelNeedle, global::Umbraco.Core.Models.PropertyType[] propertyTypesHaystack)
        {
            Ensure.That(modelNeedle).IsNotNull();
            Ensure.That(propertyTypesHaystack).IsNotNull();

            _logService.Log<PropertyTypeFinder>(new LogEntry(LogEntryType.Debug, $"Find property types matching {modelNeedle.Name} ({modelNeedle.Alias})."));

            global::Umbraco.Core.Models.PropertyType[] propertyTypes;

            if (modelNeedle.Id.HasValue)
            {
                var id = _typeRepository.GetPropertyTypeId(modelNeedle.Id.Value);

                if (id.HasValue)
                {
                    propertyTypes = propertyTypesHaystack.Where(propertyType => propertyType.Id == id.Value).Distinct(_comparer).ToArray();

                    if (propertyTypes.Any())
                    {
                        _logService.Log<PropertyTypeFinder>(new LogEntry(LogEntryType.Debug, $"Found {propertyTypes.Length} property types matching {modelNeedle.Name} ({modelNeedle.Alias}) with ID {id}."));

                        return propertyTypes;
                    }
                }
            }

            propertyTypes = propertyTypesHaystack.Where(propertyType => propertyType.Alias != null && propertyType.Alias.Equals(modelNeedle.Alias, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            _logService.Log<PropertyTypeFinder>(new LogEntry(LogEntryType.Debug, $"Found {propertyTypes.Length} property types matching {modelNeedle.Name} ({modelNeedle.Alias})."));

            return propertyTypes;
        }
    }
}
