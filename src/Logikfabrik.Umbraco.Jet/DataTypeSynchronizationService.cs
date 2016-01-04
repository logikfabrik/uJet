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
    /// The <see cref="DataTypeSynchronizationService" /> class. Synchronizes model types annotated using the <see cref="DataTypeAttribute" />.
    /// </summary>
    public class DataTypeSynchronizationService : ISynchronizationService
    {
        private readonly IDataTypeService _dataTypeService;
        private readonly ITypeResolver _typeResolver;
        private readonly ITypeRepository _typeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeSynchronizationService" /> class.
        /// </summary>
        public DataTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.DataTypeService,
                TypeResolver.Instance,
                TypeRepository.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeService" />, <paramref name="typeResolver" />, or <paramref name="typeRepository" /> are <c>null</c>.</exception>
        public DataTypeSynchronizationService(
            IDataTypeService dataTypeService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository)
        {
            if (dataTypeService == null)
            {
                throw new ArgumentNullException(nameof(dataTypeService));
            }

            if (typeResolver == null)
            {
                throw new ArgumentNullException(nameof(typeResolver));
            }

            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            _dataTypeService = dataTypeService;
            _typeResolver = typeResolver;
            _typeRepository = typeRepository;
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public void Synchronize()
        {
            if (!_typeResolver.DataTypes.Any())
            {
                return;
            }

            ValidateDataTypeModelId();
            ValidateDataTypeModelName();

            var dataTypeDefinitions = _dataTypeService.GetAllDataTypeDefinitions().ToArray();

            foreach (var dataTypeModel in _typeResolver.DataTypes)
            {
                Synchronize(dataTypeDefinitions, dataTypeModel);
            }
        }

        /// <summary>
        /// Creates a data type definition for the specified data type model.
        /// </summary>
        /// <param name="dataTypeModel">The data type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeModel" /> is <c>null</c>.</exception>
        /// <returns>The created data type definition.</returns>
        internal virtual IDataTypeDefinition CreateDataTypeDefinition(DataType dataTypeModel)
        {
            if (dataTypeModel == null)
            {
                throw new ArgumentNullException(nameof(dataTypeModel));
            }

            var dataTypeDefinition = new DataTypeDefinition(dataTypeModel.Editor)
            {
                Name = dataTypeModel.Name,
                DatabaseType = GetDatabaseType(dataTypeModel)
            };

            return dataTypeDefinition;
        }

        /// <summary>
        /// Updates the data type definition for the specified data type model.
        /// </summary>
        /// <param name="dataTypeDefinition">The data type definition.</param>
        /// <param name="dataTypeModel">The data type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeDefinition" />, or <paramref name="dataTypeModel" /> are <c>null</c>.</exception>
        /// <returns>The updated data type definition.</returns>
        internal virtual IDataTypeDefinition UpdateDataTypeDefinition(IDataTypeDefinition dataTypeDefinition, DataType dataTypeModel)
        {
            if (dataTypeDefinition == null)
            {
                throw new ArgumentNullException(nameof(dataTypeDefinition));
            }

            if (dataTypeModel == null)
            {
                throw new ArgumentNullException(nameof(dataTypeModel));
            }

            dataTypeDefinition.Name = dataTypeModel.Name;
            dataTypeDefinition.PropertyEditorAlias = dataTypeModel.Editor;
            dataTypeDefinition.DatabaseType = GetDatabaseType(dataTypeModel);

            return dataTypeDefinition;
        }

        /// <summary>
        /// Gets the database type for the specified data type model.
        /// </summary>
        /// <param name="dataTypeModel">The data type model.</param>
        /// <returns>The database type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeModel" /> is <c>null</c>.</exception>
        private static DataTypeDatabaseType GetDatabaseType(DataType dataTypeModel)
        {
            if (dataTypeModel == null)
            {
                throw new ArgumentNullException(nameof(dataTypeModel));
            }

            if (dataTypeModel.Type == typeof(int))
            {
                return DataTypeDatabaseType.Integer;
            }

            return dataTypeModel.Type == typeof(DateTime) ? DataTypeDatabaseType.Date : DataTypeDatabaseType.Ntext;
        }

        private void Synchronize(IDataTypeDefinition[] dataTypeDefinitions, DataType dataTypeModel)
        {
            if (dataTypeDefinitions == null)
            {
                throw new ArgumentNullException(nameof(dataTypeDefinitions));
            }

            if (dataTypeModel == null)
            {
                throw new ArgumentNullException(nameof(dataTypeModel));
            }

            var dataTypeDefinition = _typeResolver.ResolveType(dataTypeModel, dataTypeDefinitions);

            dataTypeDefinition = dataTypeDefinition == null
                ? CreateDataTypeDefinition(dataTypeModel)
                : UpdateDataTypeDefinition(dataTypeDefinition, dataTypeModel);

            _dataTypeService.Save(dataTypeDefinition);

            // We get the data type definition once more to refresh it after saving it.
            dataTypeDefinition = _dataTypeService.GetDataTypeDefinitionByName(dataTypeDefinition.Name);

            // Set/update tracking.
            SetDataTypeId(dataTypeDefinition, dataTypeModel);
        }

        /// <summary>
        /// Sets the data type definition identifier of the specified data type model.
        /// </summary>
        /// <param name="dataTypeDefinition">The data type definition.</param>
        /// <param name="dataTypeModel">The data type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeDefinition" />, or <paramref name="dataTypeModel" /> are <c>null</c>.</exception>
        private void SetDataTypeId(IDataTypeDefinition dataTypeDefinition, DataType dataTypeModel)
        {
            if (dataTypeDefinition == null)
            {
                throw new ArgumentNullException(nameof(dataTypeDefinition));
            }

            if (dataTypeModel == null)
            {
                throw new ArgumentNullException(nameof(dataTypeModel));
            }

            if (!dataTypeModel.Id.HasValue)
            {
                return;
            }

            _typeRepository.SetDefinitionId(dataTypeModel.Id.Value, dataTypeDefinition.Id);
        }

        /// <summary>
        /// Validates the data type model identifiers.
        /// </summary>
        private void ValidateDataTypeModelId()
        {
            var set = new HashSet<Guid>();

            foreach (var dataTypeModel in _typeResolver.DataTypes)
            {
                if (!dataTypeModel.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(dataTypeModel.Id.Value))
                {
                    var conflictingTypes = _typeResolver.DataTypes.Where(dtm => dtm.Id == dataTypeModel.Id.Value).Select(dtm => dtm.Type.Name);

                    throw new InvalidOperationException($"ID conflict for model types {string.Join(", ", conflictingTypes)}. ID {dataTypeModel.Id.Value} is already in use.");
                }

                set.Add(dataTypeModel.Id.Value);
            }
        }

        /// <summary>
        /// Validates the data type model names.
        /// </summary>
        private void ValidateDataTypeModelName()
        {
            var set = new HashSet<string>();

            foreach (var dataTypeModel in _typeResolver.DataTypes)
            {
                if (set.Contains(dataTypeModel.Name))
                {
                    var conflictingTypes = _typeResolver.DataTypes.Where(dtm => dtm.Name == dataTypeModel.Name).Select(dtm => dtm.Type.Name);

                    throw new InvalidOperationException($"Name conflict for model types {string.Join(", ", conflictingTypes)}. Name {dataTypeModel.Name} is already in use.");
                }

                set.Add(dataTypeModel.Name);
            }
        }
    }
}