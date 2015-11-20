// <copyright file="DatabaseWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using System.Reflection;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.SqlSyntax;

    /// <summary>
    /// The <see cref="DatabaseWrapper" /> class.
    /// </summary>
    public class DatabaseWrapper : IDatabaseWrapper
    {
        private readonly DatabaseSchemaHelper _databaseSchemaHelper;
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseWrapper" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="syntaxProvider">The syntax provider.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="database" />, <paramref name="logger" />, or <paramref name="syntaxProvider" /> are <c>null</c>.</exception>
        public DatabaseWrapper(Database database, ILogger logger, ISqlSyntaxProvider syntaxProvider)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (syntaxProvider == null)
            {
                throw new ArgumentNullException(nameof(syntaxProvider));
            }

            _databaseSchemaHelper = new DatabaseSchemaHelper(database, logger, syntaxProvider);
            _database = database;
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

            return _database.SingleOrDefault<T>(primaryKey);
        }

        /// <summary>
        /// Inserts the row.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <param name="row">The row.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="row" /> is <c>null</c>.</exception>
        public void InsertRow<T>(T row, object primaryKey)
            where T : class
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            if (!_database.Exists<T>(primaryKey))
            {
                _database.Insert(row);
            }
            else
            {
                _database.Update(row);
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

            return _databaseSchemaHelper.TableExist(tableName);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        public void CreateTable<T>()
            where T : new()
        {
            if (TableExist<T>())
            {
                return;
            }

            _databaseSchemaHelper.CreateTable<T>();
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
