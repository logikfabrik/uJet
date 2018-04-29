// <copyright file="ITypeResolver.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="ITypeResolver" /> interface.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Gets the document type models.
        /// </summary>
        /// <value>
        /// The document type models.
        /// </value>
        IEnumerable<DocumentType> DocumentTypes { get; }

        /// <summary>
        /// Gets the media type models.
        /// </summary>
        /// <value>
        /// The media type models.
        /// </value>
        IEnumerable<MediaType> MediaTypes { get; }

        /// <summary>
        /// Gets the member type models.
        /// </summary>
        /// <value>
        /// The member type models.
        /// </value>
        IEnumerable<MemberType> MemberTypes { get; }

        /// <summary>
        /// Gets the data type models.
        /// </summary>
        /// <value>
        /// The data type models.
        /// </value>
        IEnumerable<DataType> DataTypes { get; }
    }
}