// <copyright file="ITypeService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The <see cref="ITypeService" /> interface.
    /// </summary>
    public interface ITypeService
    {
        /// <summary>
        /// Gets the document type model types, within the current application domain.
        /// </summary>
        /// <value>
        /// The document type model types.
        /// </value>
        ReadOnlyCollection<Type> DocumentTypes { get; }

        /// <summary>
        /// Gets the data type model types, within the current application domain.
        /// </summary>
        /// <value>
        /// The data type model types.
        /// </value>
        ReadOnlyCollection<Type> DataTypes { get; }

        /// <summary>
        /// Gets the media type model types, within the current application domain.
        /// </summary>
        /// <value>
        /// The media type model types.
        /// </value>
        ReadOnlyCollection<Type> MediaTypes { get; }

        /// <summary>
        /// Gets the member type model types, within the current application domain.
        /// </summary>
        /// <value>
        /// The member type model types.
        /// </value>
        ReadOnlyCollection<Type> MemberTypes { get; }
    }
}