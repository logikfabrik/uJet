// <copyright file="DataTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

using EnsureThat;

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Models.EntityBase;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DataTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="DataTypeAttribute" />.
    /// </summary>
    public class DataTypeSynchronizer : ISynchronizer
    {
        private readonly IDataTypeService _dataTypeService;
        private readonly ITypeResolver _typeResolver;
        private readonly ITypeRepository _typeRepository;
        private readonly DataTypeFinder _dataTypeFinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        public DataTypeSynchronizer(
            IDataTypeService dataTypeService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository)
        {
            EnsureArg.IsNotNull(dataTypeService);
            EnsureArg.IsNotNull(typeResolver);
            EnsureArg.IsNotNull(typeRepository);

            _dataTypeService = dataTypeService;
            _typeResolver = typeResolver;
            _typeRepository = typeRepository;
            _dataTypeFinder = new DataTypeFinder(typeRepository);
        }

        /// <inheritdoc />
        public void Run()
        {
            var models = _typeResolver.DataTypes.ToArray();

            if (!models.Any())
            {
                return;
            }

            new DataTypeValidator().Validate(models);

            var dataTypes = _dataTypeService.GetAllDataTypeDefinitions().ToArray();

            foreach (var model in models)
            {
                Synchronize(dataTypes, model);
            }
        }

        /// <summary>
        /// Creates a data type.
        /// </summary>
        /// <param name="model">The model to use when creating the data type.</param>
        /// <returns>The created data type.</returns>
        internal virtual IDataTypeDefinition CreateDataType(DataType model)
        {
            var dataType = new DataTypeDefinition(model.Editor)
            {
                Name = model.Name,
                DatabaseType = GetDatabaseType(model.Type)
            };

            return dataType;
        }

        /// <summary>
        /// Updates the specified data type.
        /// </summary>
        /// <param name="dataType">The data type to update.</param>
        /// <param name="model">The model to use when updating the data type.</param>
        /// <returns>The updated data type.</returns>
        internal virtual IDataTypeDefinition UpdateDataTypeDefinition(IDataTypeDefinition dataType, DataType model)
        {
            dataType.Name = model.Name;
            dataType.PropertyEditorAlias = model.Editor;
            dataType.DatabaseType = GetDatabaseType(model.Type);

            return dataType;
        }

        /// <summary>
        /// Gets the database type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The database type.</returns>
        private static DataTypeDatabaseType GetDatabaseType(Type type)
        {
            if (type == typeof(int))
            {
                return DataTypeDatabaseType.Integer;
            }

            return type == typeof(DateTime) ? DataTypeDatabaseType.Date : DataTypeDatabaseType.Ntext;
        }

        private static IDictionary<string, PreValue> GetPreValues(DataType model)
        {
            if (model.PreValues == null || !model.PreValues.Any())
            {
                return new Dictionary<string, PreValue>();
            }

            return model.PreValues.ToDictionary(preValue => preValue.Key, v => new PreValue(v.Value));
        }

        private static IDictionary<string, PreValue> GetUpdatedPreValues(IDictionary<string, PreValue> preValuesToUpdate, IDictionary<string, string> newPreValues)
        {
            if (newPreValues == null || !newPreValues.Any())
            {
                return new Dictionary<string, PreValue>();
            }

            foreach (var preValue in newPreValues)
            {
                if (preValuesToUpdate.ContainsKey(preValue.Key))
                {
                    preValuesToUpdate[preValue.Key].Value = preValue.Value;
                }
                else
                {
                    preValuesToUpdate[preValue.Key] = new PreValue(preValue.Value);
                }
            }

            return preValuesToUpdate;
        }

        private void Synchronize(IDataTypeDefinition[] dataTypes, DataType model)
        {
            var dataType = _dataTypeFinder.Find(model, dataTypes).SingleOrDefault();

            if (dataType == null)
            {
                // Create new data type.
                dataType = CreateDataType(model);

                _dataTypeService.SaveDataTypeAndPreValues(dataType, GetPreValues(model));
            }
            else
            {
                // Update the data type and its pre-values.
                _dataTypeService.Save(UpdateDataTypeDefinition(dataType, model));

                var existingPreValues = _dataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id).FormatAsDictionary();

                var preValuesToSave = existingPreValues.Any()
                    ? GetUpdatedPreValues(existingPreValues, model.PreValues)
                    : GetPreValues(model);

                _dataTypeService.SavePreValues(dataType.Id, preValuesToSave);
            }

            // We get the data type once more to refresh it after saving it.
            dataType = _dataTypeService.GetDataTypeDefinitionByName(dataType.Name);

            // Set/update tracking.
            SetDataTypeId(model, dataType);
        }

        /// <summary>
        /// Sets the data type identifier of the specified model.
        /// </summary>
        /// <param name="model">The data type model.</param>
        /// <param name="dataType">The data type.</param>
        private void SetDataTypeId(DataType model, IEntity dataType)
        {
            if (!model.Id.HasValue)
            {
                return;
            }

            _typeRepository.SetDefinitionId(model.Id.Value, dataType.Id);
        }
    }
}