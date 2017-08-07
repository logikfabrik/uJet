// <copyright file="DataTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeService" />, <paramref name="typeResolver" />, or <paramref name="typeRepository" /> are <c>null</c>.</exception>
        public DataTypeSynchronizer(
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
            _dataTypeFinder = new DataTypeFinder(typeRepository);
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="model" /> is <c>null</c>.</exception>
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

        private static IDictionary<string, PreValue> GetDataTypePrevaluesDictionary(DataType model)
        {
            if (model.PreValues == null || !model.PreValues.Any())
            {
                return new Dictionary<string, PreValue>();
            }

            return model.PreValues.ToDictionary(preValue => preValue.Key, v => new PreValue(v.Value));
        }

        // Updates a PreValueCollection and returns the result as a Dictionary. No existing keys are deleted.
        private static IDictionary<string, PreValue> GetUpdatedPrevalues(IDictionary<string, PreValue> prevaluesToUpdate, IDictionary<string, string> newPreValues)
        {
            if (newPreValues == null || !newPreValues.Any())
            {
                return new Dictionary<string, PreValue>();
            }

            foreach (var p in newPreValues)
            {
                if (prevaluesToUpdate.ContainsKey(p.Key))
                {
                    prevaluesToUpdate[p.Key].Value = p.Value;
                }
                else
                {
                    prevaluesToUpdate[p.Key] = new PreValue(p.Value);
                }
            }

            return prevaluesToUpdate;
        }

        private void Synchronize(IDataTypeDefinition[] dataTypes, DataType model)
        {
            var dataType = _dataTypeFinder.Find(model, dataTypes).SingleOrDefault();

            if (dataType == null)
            {
                // create new data type
                dataType = CreateDataType(model);
                _dataTypeService.SaveDataTypeAndPreValues(dataType, GetDataTypePrevaluesDictionary(model));
            }
            else
            {
                // update the data type and its prevalues
                _dataTypeService.Save(UpdateDataTypeDefinition(dataType, model));

                // get existing prevalues
                var existingPreValues = _dataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id).FormatAsDictionary();
                var preValuesToSave = existingPreValues.Any()
                    ? GetUpdatedPrevalues(existingPreValues, model.PreValues) // update existing if found
                    : GetDataTypePrevaluesDictionary(model); // otherwise just use new prevalues

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