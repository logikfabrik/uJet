// <copyright file="DataTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
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
        }

        /// <summary>
        /// Gets the definition model identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The definition model identifier.
        /// </returns>
        public Guid? GetDefinitionModelId(int id)
        {
            var sql = new Sql().Where<DataType>(ct => ct.DefinitionId == id);

            var dataType = _databaseWrapper.Get<DataType>(sql);

            return dataType?.Id;
        }

        /// <summary>
        /// Gets the definition identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The definition identifier.
        /// </returns>
        public int? GetDefinitionId(Guid id)
        {
            if (!_databaseWrapper.TableExists<DataType>())
            {
                return null;
            }

            var dataType = _databaseWrapper.Get<DataType>(id);

            return dataType?.DefinitionId;
        }

        /// <summary>
        /// Sets the definition identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="definitionId">The definition identifier.</param>
        public void SetDefinitionId(Guid id, int definitionId)
        {
            var dataType = new DataType { Id = id, DefinitionId = definitionId };

            _databaseWrapper.CreateTable<DataType>();
            _databaseWrapper.Insert(dataType, id);
        }
    }
}
