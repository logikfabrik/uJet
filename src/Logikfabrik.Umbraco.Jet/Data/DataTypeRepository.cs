// <copyright file="DataTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;

    /// <summary>
    /// The <see cref="DataTypeRepository" /> class.
    /// </summary>
    public class DataTypeRepository : IDataTypeRepository
    {
        /// <summary>
        /// The database wrapper.
        /// </summary>
        private readonly IDatabaseWrapper databaseWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeRepository" /> class.
        /// </summary>
        /// <param name="databaseWrapper">The database wrapper.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if databaseWrapper is null.</exception>
        public DataTypeRepository(IDatabaseWrapper databaseWrapper)
        {
            if (databaseWrapper == null)
            {
                throw new ArgumentNullException(nameof(databaseWrapper));
            }

            this.databaseWrapper = databaseWrapper;
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
            if (!databaseWrapper.TableExist<DataTypeRow>())
            {
                return null;
            }

            var row = databaseWrapper.GetRow<DataTypeRow>(id);

            return row?.DefinitionId;
        }

        /// <summary>
        /// Sets the definition identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="definitionId">The definition identifier.</param>
        public void SetDefinitionId(Guid id, int definitionId)
        {
            var row = new DataTypeRow { Id = id, DefinitionId = definitionId };

            databaseWrapper.CreateTable<DataTypeRow>();
            databaseWrapper.InsertRow(row, id);
        }
    }
}
