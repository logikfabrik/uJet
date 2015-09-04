//----------------------------------------------------------------------------------
// <copyright file="DatabaseWrapper.cs" company="Logikfabrik">
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
    using System.Reflection;
    using global::Umbraco.Core.Persistence;

    /// <summary>
    /// The <see cref="DatabaseWrapper" /> class.
    /// </summary>
    public class DatabaseWrapper : IDatabaseWrapper
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database database;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseWrapper" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if database is null.</exception>
        public DatabaseWrapper(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            this.database = database;
        }

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The row.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if primaryKey is null.</exception>
        public T GetRow<T>(object primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException("primaryKey");
            }

            return this.database.SingleOrDefault<T>(primaryKey);
        }

        /// <summary>
        /// Inserts the row.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <param name="row">The row.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if row is null.</exception>
        public void InsertRow<T>(T row, object primaryKey) where T : class
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            if (!this.database.Exists<T>(primaryKey))
            {
                this.database.Insert(row);
            }
            else
            {
                this.database.Update(row);
            }
        }

        public bool TableExist<T>()
        {
            var tableName = GetTableName<T>();

            return this.database.TableExist(tableName);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        public void CreateTable<T>() where T : new()
        {
            if (this.TableExist<T>())
            {
                return;
            }

            this.database.CreateTable<T>();
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <returns>The name of the table.</returns>
        private static string GetTableName<T>()
        {
            var attribute = typeof(T).GetCustomAttribute<TableNameAttribute>();

            return attribute == null ? null : attribute.Value;
        }
    }
}
