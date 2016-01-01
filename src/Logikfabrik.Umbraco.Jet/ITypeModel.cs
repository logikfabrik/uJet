// <copyright file="ITypeModel.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="ITypeModel" /> interface.
    /// </summary>
    public interface ITypeModel
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid? Id { get; }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        string Alias { get; }
    }
}
