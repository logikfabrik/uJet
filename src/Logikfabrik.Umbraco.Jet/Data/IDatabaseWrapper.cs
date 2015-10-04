// <copyright file="IDatabaseWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    /// <summary>
    /// The <see cref="IDatabaseWrapper" /> interface.
    /// </summary>
    public interface IDatabaseWrapper
    {
        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        void CreateTable<T>() where T : new();

        /// <summary>
        /// Inserts the row.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <param name="row">The row.</param>
        /// <param name="primaryKey">The primary key.</param>
        void InsertRow<T>(T row, object primaryKey) where T : class;

        /// <summary>
        /// Gets the row.
        /// </summary>
        /// <typeparam name="T">The row type.</typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The row.</returns>
        T GetRow<T>(object primaryKey);

        /// <summary>
        /// Gets a value indicating whether a table of the specified type exists.
        /// </summary>
        /// <typeparam name="T">The table type.</typeparam>
        /// <returns>
        ///   <c>true</c> if a table of the specified type exists; otherwise, <c>false</c>.
        /// </returns>
        bool TableExist<T>();
    }
}
