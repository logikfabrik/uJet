// <copyright file="DataTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using global::Umbraco.Core.Persistence;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="DataTypeRepository" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DataTypeRepository : IDataTypeRepository
    {
        private readonly IDatabaseWrapper _databaseWrapper;
        private readonly HashSet<Tuple<Guid, int>> _definitionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeRepository" /> class.
        /// </summary>
        /// <param name="databaseWrapper">The database wrapper.</param>
        [UsedImplicitly]
        public DataTypeRepository(IDatabaseWrapper databaseWrapper)
        {
            Ensure.That(databaseWrapper).IsNotNull();

            _databaseWrapper = databaseWrapper;
            _definitionId = new HashSet<Tuple<Guid, int>>();
        }

        /// <inheritdoc />
        public Guid? GetDefinitionTypeModelId(int definitionId)
        {
            var modelId = _definitionId.SingleOrDefault(did => did.Item2 == definitionId)?.Item1;

            if (modelId.HasValue)
            {
                return modelId;
            }

            modelId = GetDefinition(definitionId)?.Id;

            if (modelId.HasValue)
            {
                _definitionId.Add(new Tuple<Guid, int>(modelId.Value, definitionId));
            }

            return modelId;
        }

        /// <inheritdoc />
        public int? GetDefinitionId(Guid id)
        {
            var definitionId = _definitionId.SingleOrDefault(did => did.Item1 == id)?.Item2;

            if (definitionId.HasValue)
            {
                return definitionId;
            }

            definitionId = GetDefinition(id)?.DefinitionId;

            if (definitionId.HasValue)
            {
                _definitionId.Add(new Tuple<Guid, int>(id, definitionId.Value));
            }

            return definitionId;
        }

        /// <inheritdoc />
        public void SetDefinitionId(Guid id, int definitionId)
        {
            var dataType = new DataType { Id = id, DefinitionId = definitionId };

            _databaseWrapper.CreateTable<DataType>();
            _databaseWrapper.Insert(dataType, id);
        }

        private DataType GetDefinition(int definitionId)
        {
            if (!_databaseWrapper.TableExists<DataType>())
            {
                return null;
            }

            var sql = new Sql().Where<DataType>(dataType => dataType.DefinitionId == definitionId, _databaseWrapper.SyntaxProvider);

            return _databaseWrapper.Get<DataType>(sql);
        }

        private DataType GetDefinition(Guid id)
        {
            return !_databaseWrapper.TableExists<DataType>() ? null : _databaseWrapper.Get<DataType>(id);
        }
    }
}