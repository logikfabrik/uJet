// <copyright file="DataTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DataTypeSynchronizationService" /> class. Synchronizes types annotated using the <see cref="DataTypeAttribute" />.
    /// </summary>
    public class DataTypeSynchronizationService : ISynchronizationService
    {
        /// <summary>
        /// The data type repository.
        /// </summary>
        private readonly IDataTypeRepository _dataTypeRepository;

        /// <summary>
        /// The data type service.
        /// </summary>
        private readonly IDataTypeService _dataTypeService;

        /// <summary>
        /// The type service.
        /// </summary>
        private readonly ITypeService _typeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeSynchronizationService" /> class.
        /// </summary>
        public DataTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.DataTypeService,
                new DataTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database)),
                TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="dataTypeRepository">The data type repository.</param>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeService" />, <paramref name="dataTypeRepository" />, or <paramref name="typeService" /> are <c>null</c>.</exception>
        public DataTypeSynchronizationService(
            IDataTypeService dataTypeService,
            IDataTypeRepository dataTypeRepository,
            ITypeService typeService)
        {
            if (dataTypeService == null)
            {
                throw new ArgumentNullException(nameof(dataTypeService));
            }

            if (dataTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(dataTypeRepository));
            }

            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            _dataTypeService = dataTypeService;
            _dataTypeRepository = dataTypeRepository;
            _typeService = typeService;
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public void Synchronize()
        {
            var dataTypes = _typeService.DataTypes.Select(t => new DataType(t)).ToArray();

            ValidateDataTypeId(dataTypes);
            ValidateDataTypeName(dataTypes);

            foreach (var dataType in dataTypes.Where(dt => dt.Id.HasValue))
            {
                SynchronizeById(_dataTypeService.GetAllDataTypeDefinitions(), dataType);
            }

            foreach (var dataType in dataTypes.Where(dt => !dt.Id.HasValue))
            {
                SynchronizeByName(_dataTypeService.GetAllDataTypeDefinitions(), dataType);
            }
        }

        /// <summary>
        /// Synchronizes data type by name.
        /// </summary>
        /// <param name="dataTypeDefinitions">The data type definitions.</param>
        /// <param name="dataType">The data type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeDefinitions" />, or <paramref name="dataType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the data type identifier is not <c>null</c>.</exception>
        internal virtual void SynchronizeByName(IEnumerable<IDataTypeDefinition> dataTypeDefinitions, DataType dataType)
        {
            if (dataTypeDefinitions == null)
            {
                throw new ArgumentNullException(nameof(dataTypeDefinitions));
            }

            if (dataType == null)
            {
                throw new ArgumentNullException(nameof(dataType));
            }

            if (dataType.Id.HasValue)
            {
                throw new ArgumentException("Data type ID must be null.", nameof(dataType));
            }

            var dtd = dataTypeDefinitions.FirstOrDefault(type => type.Name == dataType.Name);

            if (dtd == null)
            {
                CreateDataType(dataType);
            }
            else
            {
                UpdateDataType(dtd, dataType);
            }
        }

        /// <summary>
        /// Synchronizes data type by identifier.
        /// </summary>
        /// <param name="dataTypeDefinitions">The data type definitions.</param>
        /// <param name="dataType">The data type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeDefinitions" />, or <paramref name="dataType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the data type identifier is <c>null</c>.</exception>
        internal virtual void SynchronizeById(IEnumerable<IDataTypeDefinition> dataTypeDefinitions, DataType dataType)
        {
            if (dataTypeDefinitions == null)
            {
                throw new ArgumentNullException(nameof(dataTypeDefinitions));
            }

            if (dataType == null)
            {
                throw new ArgumentNullException(nameof(dataType));
            }

            if (!dataType.Id.HasValue)
            {
                throw new ArgumentException("Data type ID cannot be null.", nameof(dataType));
            }

            IDataTypeDefinition dtd = null;

            var id = _dataTypeRepository.GetDefinitionId(dataType.Id.Value);

            if (id.HasValue)
            {
                // The data type has been synchronized before. Get the matching data type definition.
                // It might have been removed using the back office.
                dtd = dataTypeDefinitions.FirstOrDefault(type => type.Id == id.Value);
            }

            if (dtd == null)
            {
                CreateDataType(dataType);

                // Get the created data type definition.
                dtd =
                    _dataTypeService.GetDataTypeDefinitionByPropertyEditorAlias(dataType.Editor)
                        .First(type => type.Name == dataType.Name);

                // Connect the data type and the created data type definition.
                _dataTypeRepository.SetDefinitionId(dataType.Id.Value, dtd.Id);
            }
            else
            {
                UpdateDataType(dtd, dataType);
            }
        }

        /// <summary>
        /// Updates the data type.
        /// </summary>
        /// <param name="dataTypeDefinition">The data type definition.</param>
        /// <param name="dataType">The data type to update.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeDefinition" />, or <paramref name="dataType" /> are <c>null</c>.</exception>
        internal virtual void UpdateDataType(IDataTypeDefinition dataTypeDefinition, DataType dataType)
        {
            if (dataTypeDefinition == null)
            {
                throw new ArgumentNullException(nameof(dataTypeDefinition));
            }

            if (dataType == null)
            {
                throw new ArgumentNullException(nameof(dataType));
            }

            dataTypeDefinition.Name = dataType.Name;
            dataTypeDefinition.PropertyEditorAlias = dataType.Editor;
            dataTypeDefinition.DatabaseType = GetDatabaseType(dataType);

            _dataTypeService.Save(dataTypeDefinition);
        }

        /// <summary>
        /// Validates the data type identifier.
        /// </summary>
        /// <param name="dataTypes">The data types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an identifier in <paramref name="dataTypes" /> is conflicting.</exception>
        private static void ValidateDataTypeId(IEnumerable<DataType> dataTypes)
        {
            if (dataTypes == null)
            {
                throw new ArgumentNullException(nameof(dataTypes));
            }

            var set = new HashSet<Guid>();

            foreach (var dataType in dataTypes)
            {
                if (!dataType.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(dataType.Id.Value))
                {
                    throw new InvalidOperationException(
                        $"ID conflict for data type {dataType.Name}. ID {dataType.Id.Value} is already in use.");
                }

                set.Add(dataType.Id.Value);
            }
        }

        /// <summary>
        /// Validates the data type name.
        /// </summary>
        /// <param name="dataTypes">The data types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a name in <paramref name="dataTypes" /> is conflicting.</exception>
        private static void ValidateDataTypeName(IEnumerable<DataType> dataTypes)
        {
            if (dataTypes == null)
            {
                throw new ArgumentNullException(nameof(dataTypes));
            }

            var set = new HashSet<string>();

            foreach (var dataType in dataTypes)
            {
                if (set.Contains(dataType.Name))
                {
                    throw new InvalidOperationException(
                        string.Format("Name conflict for data type {0}. Name {0} is already in use.", dataType.Name));
                }

                set.Add(dataType.Name);
            }
        }

        /// <summary>
        /// Gets the database type.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <returns>The database type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataType" /> is <c>null</c>.</exception>
        private static DataTypeDatabaseType GetDatabaseType(DataType dataType)
        {
            if (dataType == null)
            {
                throw new ArgumentNullException(nameof(dataType));
            }

            if (dataType.Type == typeof(int))
            {
                return DataTypeDatabaseType.Integer;
            }

            return dataType.Type == typeof(DateTime) ? DataTypeDatabaseType.Date : DataTypeDatabaseType.Ntext;
        }

        /// <summary>
        /// Creates the data type.
        /// </summary>
        /// <param name="dataType">The data type to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataType" /> is <c>null</c>.</exception>
        private void CreateDataType(DataType dataType)
        {
            if (dataType == null)
            {
                throw new ArgumentNullException(nameof(dataType));
            }

            var dataTypeDefinition = new DataTypeDefinition(-1, dataType.Editor)
            {
                Name = dataType.Name,
                DatabaseType = GetDatabaseType(dataType)
            };

            _dataTypeService.Save(dataTypeDefinition);
        }
    }
}
