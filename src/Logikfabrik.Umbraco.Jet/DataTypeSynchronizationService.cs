//----------------------------------------------------------------------------------
// <copyright file="DataTypeSynchronizationService.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

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
                throw new ArgumentNullException("dataTypeService");
            }

            if (dataTypeRepository == null)
            {
                throw new ArgumentNullException("dataTypeRepository");
            }

            if (typeService == null)
            {
                throw new ArgumentNullException("typeService");
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
            var dataTypes = this.typeService.DataTypes.Select(t => new DataType(t)).ToArray();

            ValidateDataTypeId(dataTypes);
            ValidateDataTypeName(dataTypes);

            foreach (var dataType in dataTypes.Where(dt => dt.Id.HasValue))
            {
                this.SynchronizeById(this.dataTypeService.GetAllDataTypeDefinitions(), dataType);
            }
            
            foreach (var dataType in dataTypes.Where(dt => !dt.Id.HasValue))
            {
                this.SynchronizeByName(this.dataTypeService.GetAllDataTypeDefinitions(), dataType);
            }
        }

        private static void ValidateDataTypeId(IEnumerable<DataType> dataTypes)
        {
            if (dataTypes == null)
            {
                throw new ArgumentNullException("dataTypes");
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
                        string.Format(
                            "ID conflict for data type {0}. ID {1} is already in use.", dataType.Name, dataType.Id.Value));
                }

                set.Add(dataType.Id.Value);
            }
        }

        private static void ValidateDataTypeName(IEnumerable<DataType> dataTypes)
        {
            if (dataTypes == null)
            {
                throw new ArgumentNullException("dataTypes");
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
                throw new ArgumentNullException("dataType");
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
                throw new ArgumentNullException("dataTypeDefinitions");
            }

            if (dataType == null)
            {
                throw new ArgumentNullException("dataType");
            }

            if (dataType.Id.HasValue)
            {
                throw new ArgumentException("Data type ID must be null.", "dataType");
            }

            var dtd = dataTypeDefinitions.FirstOrDefault(type => type.Name == dataType.Name);

            if (dtd == null)
            {
                this.CreateDataType(dataType);
            }
            else
            {
                this.UpdateDataType(dtd, dataType);
            }
        }

        private void SynchronizeById(IEnumerable<IDataTypeDefinition> dataTypeDefinitions, DataType dataType)
        {
            if (dataTypeDefinitions == null)
            {
                throw new ArgumentNullException("dataTypeDefinitions");
            }

            if (dataType == null)
            {
                throw new ArgumentNullException("dataType");
            }

            if (!dataType.Id.HasValue)
            {
                throw new ArgumentException("Data type ID cannot be null.", "dataType");
            }

            IDataTypeDefinition dtd = null;

            var id = this.dataTypeRepository.GetDefinitionId(dataType.Id.Value);

            if (id.HasValue)
            {
                // The data type has been synchronized before. Get the matching data type definition.
                // It might have been removed using the back office.
                dtd = dataTypeDefinitions.FirstOrDefault(type => type.Id == id.Value);
            }

            if (dtd == null)
            {
                this.CreateDataType(dataType);

                // Get the created data type definition.
                dtd =
                    this.dataTypeService.GetDataTypeDefinitionByPropertyEditorAlias(dataType.Editor)
                        .First(type => type.Name == dataType.Name);

                // Connect the data type and the created data type definition.
                this.dataTypeRepository.SetDefinitionId(dataType.Id.Value, dtd.Id);
            }
            else
            {
                this.UpdateDataType(dtd, dataType);
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
                throw new ArgumentNullException("dataType");
            }

            var dataTypeDefinition = new DataTypeDefinition(-1, dataType.Editor)
            {
                Name = dataType.Name,
                DatabaseType = GetDatabaseType(dataType)
            };

            this.dataTypeService.Save(dataTypeDefinition);
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
                throw new ArgumentNullException("dataTypeDefinition");
            }

            if (dataType == null)
            {
                throw new ArgumentNullException("dataType");
            }

            dataTypeDefinition.Name = dataType.Name;
            dataTypeDefinition.PropertyEditorAlias = dataType.Editor;
            dataTypeDefinition.DatabaseType = GetDatabaseType(dataType);

            this.dataTypeService.Save(dataTypeDefinition);
        }
    }
}
