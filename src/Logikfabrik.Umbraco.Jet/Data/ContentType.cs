// <copyright file="ContentType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using global::Umbraco.Core.Persistence;

    /// <summary>
    /// The <see cref="ContentType" /> class.
    /// </summary>
    [TableName("uJetContentType")]
    [PrimaryKey("Id", autoIncrement = false)]
    public class ContentType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content type identifier.
        /// </summary>
        /// <value>
        /// The content type identifier.
        /// </value>
        public int ContentTypeId { get; set; }
    }
}
