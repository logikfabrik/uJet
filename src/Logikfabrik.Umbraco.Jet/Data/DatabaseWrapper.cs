﻿// <copyright file="DatabaseWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using System.Reflection;
    using EnsureThat;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.SqlSyntax;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="DatabaseWrapper" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
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
        [UsedImplicitly]
        public DatabaseWrapper(Database database, ILogger logger, ISqlSyntaxProvider syntaxProvider)
        {
            Ensure.That(database).IsNotNull();
            Ensure.That(logger).IsNotNull();
            Ensure.That(syntaxProvider).IsNotNull();

            _databaseSchemaHelper = new DatabaseSchemaHelper(database, logger, syntaxProvider);
            _database = database;
            SyntaxProvider = syntaxProvider;
        }

        /// <inheritdoc />
        public ISqlSyntaxProvider SyntaxProvider { get; }

        /// <inheritdoc />
        public T Get<T>(object primaryKey)
        {
            Ensure.That(primaryKey).IsNotNull();

            return _database.SingleOrDefault<T>(primaryKey);
        }

        /// <inheritdoc />
        public T Get<T>(Sql sql)
            where T : class
        {
            Ensure.That(sql).IsNotNull();

            return _database.FirstOrDefault<T>(sql);
        }

        /// <inheritdoc />
        public void Insert<T>(T obj, object primaryKey)
            where T : class
        {
            Ensure.That(obj).IsNotNull();
            Ensure.That(primaryKey).IsNotNull();

            if (!_database.Exists<T>(primaryKey))
            {
                _database.Insert(obj);
            }
            else
            {
                _database.Update(obj);
            }
        }

        /// <inheritdoc />
        public bool TableExists<T>()
        {
            var tableName = GetTableName(typeof(T));

            return _databaseSchemaHelper.TableExist(tableName);
        }

        /// <inheritdoc />
        public bool TableExists(Type type)
        {
            Ensure.That(type).IsNotNull();

            var tableName = GetTableName(type);

            return _databaseSchemaHelper.TableExist(tableName);
        }

        /// <inheritdoc />
        public void CreateTable<T>()
            where T : new()
        {
            if (TableExists<T>())
            {
                return;
            }

            _databaseSchemaHelper.CreateTable<T>();
        }

        /// <inheritdoc />
        public void CreateTable(Type type)
        {
            Ensure.That(type).IsNotNull();

            if (TableExists(type))
            {
                return;
            }

            _databaseSchemaHelper.CreateTable(true, type);
        }

        private static string GetTableName(MemberInfo member)
        {
            var attribute = member.GetCustomAttribute<TableNameAttribute>();

            return attribute?.Value;
        }
    }
}
