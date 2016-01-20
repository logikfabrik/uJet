// <copyright file="ITypeResolver.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.ObjectModel;

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
        ReadOnlyCollection<DocumentType> DocumentTypes { get; }

        /// <summary>
        /// Gets the media type models.
        /// </summary>
        /// <value>
        /// The media type models.
        /// </value>
        ReadOnlyCollection<MediaType> MediaTypes { get; }

        /// <summary>
        /// Gets the member type models.
        /// </summary>
        /// <value>
        /// The member type models.
        /// </value>
        ReadOnlyCollection<MemberType> MemberTypes { get; }

        /// <summary>
        /// Gets the data type models.
        /// </summary>
        /// <value>
        /// The data type models.
        /// </value>
        ReadOnlyCollection<DataType> DataTypes { get; }
    }
}