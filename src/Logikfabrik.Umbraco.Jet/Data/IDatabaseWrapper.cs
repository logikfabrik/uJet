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
        /// Creates the table for the specified object type.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        void CreateTable<T>()
            where T : new();

        /// <summary>
        /// Inserts the object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="primaryKey">The primary key.</param>
        void Insert<T>(T obj, object primaryKey)
            where T : class;

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The object.</returns>
        T Get<T>(object primaryKey);

        /// <summary>
        /// Gets a value indicating whether a table for the specified object type exists.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>
        ///   <c>true</c> if a table of for the specified object type exists; otherwise, <c>false</c>.
        /// </returns>
        bool TableExists<T>();
    }
}
