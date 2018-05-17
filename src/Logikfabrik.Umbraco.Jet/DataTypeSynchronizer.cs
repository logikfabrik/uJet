// <copyright file="DataTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Models.EntityBase;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DataTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="DataTypeAttribute" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DataTypeSynchronizer : ISynchronizer
    {
        private readonly IDataTypeService _dataTypeService;
        private readonly IDataTypeDefinitionFinder _dataTypeDefinitionFinder;
        private readonly IModelService _modelService;
        private readonly ITypeRepository _typeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="dataTypeDefinitionFinder">The data type definition finder.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="typeRepository">The type repository.</param>
        // ReSharper disable once UnusedMember.Global
        public DataTypeSynchronizer(
            IDataTypeService dataTypeService,
            IDataTypeDefinitionFinder dataTypeDefinitionFinder,
            IModelService modelService,
            ITypeRepository typeRepository)
        {
            Ensure.That(dataTypeService).IsNotNull();
            Ensure.That(dataTypeDefinitionFinder).IsNotNull();
            Ensure.That(modelService).IsNotNull();
            Ensure.That(typeRepository).IsNotNull();

            _dataTypeService = dataTypeService;
            _dataTypeDefinitionFinder = dataTypeDefinitionFinder;
            _modelService = modelService;
            _typeRepository = typeRepository;
        }

        /// <inheritdoc />
        public void Run()
        {
            var models = _modelService.DataTypes.ToArray();

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

        private static IDataTypeDefinition CreateDataType(DataType model)
        {
            var dataType = new DataTypeDefinition(model.Editor)
            {
                Name = model.Name,
                DatabaseType = GetDatabaseType(model.Type)
            };

            return dataType;
        }

        private static IDataTypeDefinition UpdateDataTypeDefinition(IDataTypeDefinition dataType, DataType model)
        {
            dataType.Name = model.Name;
            dataType.PropertyEditorAlias = model.Editor;
            dataType.DatabaseType = GetDatabaseType(model.Type);

            return dataType;
        }

        private static DataTypeDatabaseType GetDatabaseType(Type type)
        {
            if (type == typeof(int))
            {
                return DataTypeDatabaseType.Integer;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (type == typeof(DateTime))
            {
                return DataTypeDatabaseType.Date;
            }

            // Return Ntext. Returning Nvarchar could result in length issues.
            return DataTypeDatabaseType.Ntext;
        }

        private static IDictionary<string, PreValue> GetPreValues(DataType model)
        {
            if (model.PreValues == null || !model.PreValues.Any())
            {
                return new Dictionary<string, PreValue>();
            }

            return model.PreValues.ToDictionary(preValue => preValue.Key, v => new PreValue(v.Value));
        }

        private static IDictionary<string, PreValue> GetUpdatedPreValues(IDictionary<string, PreValue> existingPreValues, IDictionary<string, string> newPreValues)
        {
            if (newPreValues == null || !newPreValues.Any())
            {
                return new Dictionary<string, PreValue>();
            }

            var preValues = new KeyValuePair<string, PreValue>[] { };

            existingPreValues.CopyTo(preValues, 0);

            var updatedPreValues = preValues.ToDictionary(preValue => preValue.Key, pair => pair.Value);

            foreach (var preValue in newPreValues)
            {
                if (updatedPreValues.ContainsKey(preValue.Key))
                {
                    updatedPreValues[preValue.Key].Value = preValue.Value;
                }
                else
                {
                    updatedPreValues[preValue.Key] = new PreValue(preValue.Value);
                }
            }

            return updatedPreValues;
        }

        private void Synchronize(IDataTypeDefinition[] dataTypes, DataType model)
        {
            var dataType = _dataTypeDefinitionFinder.Find(model, dataTypes).SingleOrDefault();

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

                var existingPreValues = _dataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id)?.FormatAsDictionary();

                if (existingPreValues != null)
                {
                    var preValuesToSave = existingPreValues.Any()
                        ? GetUpdatedPreValues(existingPreValues, model.PreValues)
                        : GetPreValues(model);

                    _dataTypeService.SavePreValues(dataType.Id, preValuesToSave);
                }
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