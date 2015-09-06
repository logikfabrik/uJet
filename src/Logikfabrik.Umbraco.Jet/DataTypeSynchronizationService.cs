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
    /// The data type synchronization service. Responsible for synchronizing data types
    /// found in the code base with the Umbraco database.
    /// </summary>
    public class DataTypeSynchronizationService : ISynchronizationService
    {
        private readonly IDataTypeRepository dataTypeRepository;
        private readonly IDataTypeService dataTypeService;
        private readonly ITypeService typeService;

        public DataTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.DataTypeService,
                new DataTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database)),
                TypeService.Instance)
        {
        }

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

            this.dataTypeService = dataTypeService;
            this.dataTypeRepository = dataTypeRepository;
            this.typeService = typeService;
        }

        /// <summary>
        /// Synchronizes data types.
        /// </summary>
        public void Synchronize()
        {
            var dataTypes = typeService.DataTypes.Select(t => new DataType(t)).ToArray();

            ValidateDataTypeId(dataTypes);
            ValidateDataTypeName(dataTypes);

            foreach (var dataType in dataTypes.Where(dt => dt.Id.HasValue))
            {
                SynchronizeById(dataTypeService.GetAllDataTypeDefinitions(), dataType);
            }

            foreach (var dataType in dataTypes.Where(dt => !dt.Id.HasValue))
            {
                SynchronizeByName(dataTypeService.GetAllDataTypeDefinitions(), dataType);
            }
        }

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

        private void SynchronizeByName(IEnumerable<IDataTypeDefinition> dataTypeDefinitions, DataType dataType)
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

        private void SynchronizeById(IEnumerable<IDataTypeDefinition> dataTypeDefinitions, DataType dataType)
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

            var id = dataTypeRepository.GetDefinitionId(dataType.Id.Value);

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
                    dataTypeService.GetDataTypeDefinitionByPropertyEditorAlias(dataType.Editor)
                        .First(type => type.Name == dataType.Name);

                // Connect the data type and the created data type definition.
                dataTypeRepository.SetDefinitionId(dataType.Id.Value, dtd.Id);
            }
            else
            {
                UpdateDataType(dtd, dataType);
            }
        }

        /// <summary>
        /// Creates a new data type.
        /// </summary>
        /// <param name="dataType">The reflected data type to create.</param>
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

            dataTypeService.Save(dataTypeDefinition);
        }

        /// <summary>
        /// Updates a data type.
        /// </summary>
        /// <param name="dataTypeDefinition">The data type to update.</param>
        /// <param name="dataType">The reflected data type to update.</param>
        private void UpdateDataType(IDataTypeDefinition dataTypeDefinition, DataType dataType)
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

            dataTypeService.Save(dataTypeDefinition);
        }
    }
}
