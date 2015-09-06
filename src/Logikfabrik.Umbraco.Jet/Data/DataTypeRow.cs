// <copyright file="DataTypeRow.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using global::Umbraco.Core.Persistence;

    /// <summary>
    /// The <see cref="DataTypeRow" /> class.
    /// </summary>
    [TableName("uJetDataType")]
    [PrimaryKey("Id", autoIncrement = false)]
    public class DataTypeRow
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the definition identifier.
        /// </summary>
        /// <value>
        /// The definition identifier.
        /// </value>
        public int DefinitionId { get; set; }
    }
}
