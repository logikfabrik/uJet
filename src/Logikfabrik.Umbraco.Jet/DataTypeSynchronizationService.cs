// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet
{
    public class DataTypeSynchronizationService : ISynchronizationService
    {
        private readonly IDataTypeService _dataTypeService;
        private readonly ITypeService _typeService;
        
        public DataTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.DataTypeService,
                TypeService.Instance) { }

        public DataTypeSynchronizationService(
            IDataTypeService dataTypeService,
            ITypeService typeService)
        {
            if (dataTypeService == null)
                throw new ArgumentNullException("dataTypeService");

            if (typeService == null)
                throw new ArgumentNullException("typeService");

            _dataTypeService = dataTypeService;
            _typeService = typeService;
        }

        /// <summary>
        /// Synchronizes data types.
        /// </summary>
        public void Synchronize()
        {
            var dataTypeDefinitions = _dataTypeService.GetAllDataTypeDefinitions().ToArray();

            // Create and/or update data types.
            foreach (var dataType in _typeService.DataTypes.Select(t => new DataType(t)))
                if (dataTypeDefinitions.All(ct => ct.Name != dataType.Name))
                    CreateDataType(dataType);
                else
                    UpdateDataType(dataTypeDefinitions.First(ct => ct.Name == dataType.Name), dataType);
        }

        /// <summary>
        /// Creates a new data type.
        /// </summary>
        /// <param name="dataType">The reflected data type to create.</param>
        private void CreateDataType(DataType dataType)
        {
            if (dataType == null)
                throw new ArgumentNullException("dataType");

            var dataTypeDefinition = new DataTypeDefinition(-1, dataType.Editor)
            {
                Name = dataType.Name,
                DatabaseType = GetDatabaseType(dataType)
            };

            _dataTypeService.Save(dataTypeDefinition);
        }

        /// <summary>
        /// Updates a data type.
        /// </summary>
        /// <param name="dataTypeDefinition">The data type to update.</param>
        /// <param name="dataType">The reflected data type to update.</param>
        private void UpdateDataType(IDataTypeDefinition dataTypeDefinition, DataType dataType)
        {
            if (dataTypeDefinition == null)
                throw new ArgumentNullException("dataTypeDefinition");

            if (dataType == null)
                throw new ArgumentNullException("dataType");

            dataTypeDefinition.PropertyEditorAlias = dataType.Editor;
            // TODO: Handle incompatible changes.
            dataTypeDefinition.DatabaseType = GetDatabaseType(dataType);

            _dataTypeService.Save(dataTypeDefinition);
        }

        private static DataTypeDatabaseType GetDatabaseType(DataType dataType)
        {
            if (dataType == null)
                throw new ArgumentNullException("dataType");

            if (dataType.Type == typeof(int))
                return DataTypeDatabaseType.Integer;

            return dataType.Type == typeof(DateTime) ? DataTypeDatabaseType.Date : DataTypeDatabaseType.Ntext;
        }
    }
}
