// <copyright file="DataTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Core.Persistence;

    /// <summary>
    /// The <see cref="DataTypeRepository" /> class.
    /// </summary>
    public class DataTypeRepository : IDataTypeRepository
    {
        private readonly IDatabaseWrapper _databaseWrapper;

        private readonly HashSet<Tuple<Guid, int>> _definitionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeRepository" /> class.
        /// </summary>
        public DataTypeRepository()
            : this(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeRepository" /> class.
        /// </summary>
        /// <param name="databaseWrapper">The database wrapper.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="databaseWrapper" /> is <c>null</c>.</exception>
        public DataTypeRepository(IDatabaseWrapper databaseWrapper)
        {
            if (databaseWrapper == null)
            {
                throw new ArgumentNullException(nameof(databaseWrapper));
            }

            _databaseWrapper = databaseWrapper;
            _definitionId = new HashSet<Tuple<Guid, int>>();
        }

        /// <summary>
        /// Gets the definition model identifier.
        /// </summary>
        /// <param name="definitionId">The definition identifier.</param>
        /// <returns>
        /// The definition model identifier.
        /// </returns>
        public Guid? GetDefinitionModelId(int definitionId)
        {
            var modelId = _definitionId.SingleOrDefault(t => t.Item2 == definitionId)?.Item1;

            if (modelId.HasValue)
            {
                return modelId;
            }

            modelId = GetDefinitionByDefinitionId(definitionId)?.Id;

            if (modelId.HasValue)
            {
                _definitionId.Add(new Tuple<Guid, int>(modelId.Value, definitionId));
            }

            return modelId;
        }

        /// <summary>
        /// Gets the definition identifier.
        /// </summary>
        /// <param name="id">The definition model identifier.</param>
        /// <returns>
        /// The definition identifier.
        /// </returns>
        public int? GetDefinitionId(Guid id)
        {
            var definitionId = _definitionId.SingleOrDefault(t => t.Item1 == id)?.Item2;

            if (definitionId.HasValue)
            {
                return definitionId;
            }

            definitionId = GetDefinitionById(id)?.DefinitionId;

            if (definitionId.HasValue)
            {
                _definitionId.Add(new Tuple<Guid, int>(id, definitionId.Value));
            }

            return definitionId;
        }

        /// <summary>
        /// Sets the definition identifier.
        /// </summary>
        /// <param name="id">The definition model identifier.</param>
        /// <param name="definitionId">The definition identifier.</param>
        public void SetDefinitionId(Guid id, int definitionId)
        {
            var dataType = new DataType { Id = id, DefinitionId = definitionId };

            _databaseWrapper.CreateTable<DataType>();
            _databaseWrapper.Insert(dataType, id);
        }

        /// <summary>
        /// Gets the definition with the specified definition identifier.
        /// </summary>
        /// <param name="id">The definition identifier.</param>
        /// <returns>The definition with the specified definition identifier.</returns>
        internal virtual DataType GetDefinitionByDefinitionId(int id)
        {
            if (!_databaseWrapper.TableExists<DataType>())
            {
                return null;
            }

            var sql = new Sql().Where<DataType>(ct => ct.DefinitionId == id);

            return _databaseWrapper.Get<DataType>(sql);
        }

        /// <summary>
        /// Gets the definition with the specified definition model identifier.
        /// </summary>
        /// <param name="id">The definition model identifier.</param>
        /// <returns>The definition with the specified definition model identifier.</returns>
        internal virtual DataType GetDefinitionById(Guid id)
        {
            return !_databaseWrapper.TableExists<Jet.DataType>() ? null : _databaseWrapper.Get<DataType>(id);
        }
    }
}
