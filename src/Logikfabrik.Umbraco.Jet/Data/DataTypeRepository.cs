//----------------------------------------------------------------------------------
// <copyright file="DataTypeRepository.cs" company="Logikfabrik">
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
                throw new ArgumentNullException("databaseWrapper");
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
            if (!this.databaseWrapper.TableExist<DataTypeRow>())
            {
                return null;
            }

            var row = this.databaseWrapper.GetRow<DataTypeRow>(id);

            if (row == null)
            {
                return null;
            }

            return row.DefinitionId;
        }

        /// <summary>
        /// Sets the definition identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="definitionId">The definition identifier.</param>
        public void SetDefinitionId(Guid id, int definitionId)
        {
            var row = new DataTypeRow { Id = id, DefinitionId = definitionId };

            this.databaseWrapper.CreateTable<DataTypeRow>();
            this.databaseWrapper.InsertRow(row, id);
        }
    }
}
