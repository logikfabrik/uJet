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
        /// Gets the document model types within the current application domain.
        /// </summary>
        /// <value>
        /// The document model types.
        /// </value>
        IEnumerable<Type> DocumentTypes { get; }

        /// <summary>
        /// Gets the data model types within the current application domain.
        /// </summary>
        /// <value>
        /// The data model types.
        /// </value>
        IEnumerable<Type> DataTypes { get; }

        /// <summary>
        /// Gets the media model types within the current application domain.
        /// </summary>
        /// <value>
        /// The media model types.
        /// </value>
        IEnumerable<Type> MediaTypes { get; }

        /// <summary>
        /// Gets the member model types within the current application domain.
        /// </summary>
        /// <value>
        /// The member model types.
        /// </value>
        IEnumerable<Type> MemberTypes { get; }
    }
}