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
        /// Gets the object of the specified object type with the specified primary key.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="primaryKey" /> is <c>null</c>.</exception>
        public T Get<T>(object primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException(nameof(primaryKey));
            }

            return _database.SingleOrDefault<T>(primaryKey);
        }

        /// <summary>
        /// Gets the object of the specified object type using the specified query.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="sql">The query.</param>
        /// <returns>The object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sql" /> is <c>null</c>.</exception>
        public T Get<T>(Sql sql)
            where T : class
        {
            if (sql == null)
            {
                throw new ArgumentNullException(nameof(sql));
            }

            return _database.FirstOrDefault<T>(sql);
        }

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="obj" /> is <c>null</c>.</exception>
        public void Insert<T>(T obj, object primaryKey)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (!_database.Exists<T>(primaryKey))
            {
                _database.Insert(obj);
            }
            else
            {
                _database.Update(obj);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a table for the specified object type exists.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>
        ///   <c>true</c> if a table of for the specified object type exists; otherwise, <c>false</c>.
        /// </returns>
        public bool TableExists<T>()
        {
            var tableName = GetTableName(typeof(T));

            return _databaseSchemaHelper.TableExist(tableName);
        }

        /// <summary>
        /// Gets a value indicating whether a table for the specified object type exists.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>
        ///   <c>true</c> if a table of for the specified object type exists; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        public bool TableExists(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var tableName = GetTableName(type);

            return _databaseSchemaHelper.TableExist(tableName);
        }

        /// <summary>
        /// Creates the table for the specified object type.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        public void CreateTable<T>()
            where T : new()
        {
            if (TableExists<T>())
            {
                return;
            }

            _databaseSchemaHelper.CreateTable<T>();
        }

        /// <summary>
        /// Creates the table for the specified object type.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        public void CreateTable(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (TableExists(type))
            {
                return;
            }

            _databaseSchemaHelper.CreateTable(true, type);
        }

        /// <summary>
        /// Gets the name of the table for the specified object type.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>The name of the table.</returns>
        protected static string GetTableName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var attribute = type.GetCustomAttribute<TableNameAttribute>();

            return attribute?.Value;
        }
    }
}
