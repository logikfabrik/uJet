// <copyright file="IDatabaseWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.SqlSyntax;

    /// <summary>
    /// The <see cref="IDatabaseWrapper" /> interface.
    /// </summary>
    public interface IDatabaseWrapper
    {
        /// <summary>
        /// Gets the syntax provider.
        /// </summary>
        /// <value>
        /// The syntax provider.
        /// </value>
        ISqlSyntaxProvider SyntaxProvider { get; }

        /// <summary>
        /// Creates the table for the specified object type.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        void CreateTable<T>()
            where T : new();

        /// <summary>
        /// Creates the table for the specified object type.
        /// </summary>
        /// <param name="type">The object type.</param>
        void CreateTable(Type type);

        /// <summary>
        /// Inserts the specified object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="primaryKey">The primary key.</param>
        void Insert<T>(T obj, object primaryKey)
            where T : class;

        /// <summary>
        /// Gets the object of the specified object type with the specified primary key.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The object.</returns>
        T Get<T>(object primaryKey);

        /// <summary>
        /// Gets the object of the specified object type using the specified query.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="sql">The query.</param>
        /// <returns>The object.</returns>
        T Get<T>(Sql sql)
            where T : class;

        /// <summary>
        /// Gets a value indicating whether a table for the specified object type exists.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>
        ///   <c>true</c> if a table of for the specified object type exists; otherwise, <c>false</c>.
        /// </returns>
        bool TableExists<T>();

        /// <summary>
        /// Gets a value indicating whether a table for the specified object type exists.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>
        ///   <c>true</c> if a table of for the specified object type exists; otherwise, <c>false</c>.
        /// </returns>
        bool TableExists(Type type);
    }
}
