// <copyright file="ITypeService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="ITypeService" /> interface.
    /// </summary>
    public interface ITypeService
    {
        /// <summary>
        /// Gets the document types within the current application domain.
        /// </summary>
        /// <value>
        /// The document types.
        /// </value>
        IEnumerable<Type> DocumentTypes { get; }

        /// <summary>
        /// Gets the data types within the current application domain.
        /// </summary>
        /// <value>
        /// The data types.
        /// </value>
        IEnumerable<Type> DataTypes { get; }

        /// <summary>
        /// Gets the media types within the current application domain.
        /// </summary>
        /// <value>
        /// The media types.
        /// </value>
        IEnumerable<Type> MediaTypes { get; }

        /// <summary>
        /// Gets the member types within the current application domain.
        /// </summary>
        /// <value>
        /// The member types.
        /// </value>
        IEnumerable<Type> MemberTypes { get; }

        /// <summary>
        /// Gets the composition for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The composition for the specified type.</returns>
        IDictionary<Type, IEnumerable<Type>> GetComposition(Type type, Func<Type, bool> predicate);
    }
}
