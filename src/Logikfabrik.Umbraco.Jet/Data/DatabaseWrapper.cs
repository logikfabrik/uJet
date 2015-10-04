// <copyright file="DatabaseWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="database" /> is <c>null</c>.</exception>
        public DatabaseWrapper(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The row.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="primaryKey" /> is <c>null</c>.</exception>
        public T GetRow<T>(object primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException(nameof(primaryKey));
            }

            return database.SingleOrDefault<T>(primaryKey);
        }

        /// <summary>
        /// Inserts the row.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <param name="row">The row.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="row" /> is <c>null</c>.</exception>
        public void InsertRow<T>(T row, object primaryKey) where T : class
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            if (!database.Exists<T>(primaryKey))
            {
                database.Insert(row);
            }
            else
            {
                database.Update(row);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a table of the specified type exists.
        /// </summary>
        /// <typeparam name="T">The table type.</typeparam>
        /// <returns>
        ///   <c>true</c> if a table of the specified type exists; otherwise, <c>false</c>.
        /// </returns>
        public bool TableExist<T>()
        {
            var tableName = GetTableName<T>();

            return database.TableExist(tableName);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        public void CreateTable<T>() where T : new()
        {
            if (TableExist<T>())
            {
                return;
            }

            database.CreateTable<T>();
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <returns>The name of the table.</returns>
        private static string GetTableName<T>()
        {
            var attribute = typeof(T).GetCustomAttribute<TableNameAttribute>();

            return attribute?.Value;
        }
    }
}
