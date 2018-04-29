// <copyright file="ITypeService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
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
        /// Gets the document type model types.
        /// </summary>
        /// <value>
        /// The document type model types.
        /// </value>
        IEnumerable<Type> DocumentTypes { get; }

        /// <summary>
        /// Gets the data type model types.
        /// </summary>
        /// <value>
        /// The data type model types.
        /// </value>
        IEnumerable<Type> DataTypes { get; }

        /// <summary>
        /// Gets the media type model types.
        /// </summary>
        /// <value>
        /// The media type model types.
        /// </value>
        IEnumerable<Type> MediaTypes { get; }

        /// <summary>
        /// Gets the member type model types.
        /// </summary>
        /// <value>
        /// The member type model types.
        /// </value>
        IEnumerable<Type> MemberTypes { get; }
    }
}