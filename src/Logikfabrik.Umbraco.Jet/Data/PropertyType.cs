// <copyright file="PropertyType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using global::Umbraco.Core.Persistence;

    /// <summary>
    /// The <see cref="PropertyType" /> class.
    /// </summary>
    [TableName("uJetPropertyType")]
    [PrimaryKey("Id", autoIncrement = false)]
    public class PropertyType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the property type identifier.
        /// </summary>
        /// <value>
        /// The property type identifier.
        /// </value>
        public int PropertyTypeId { get; set; }
    }
}
