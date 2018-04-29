// <copyright file="IdAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="IdAttribute" /> class.
    /// </summary>
    public class IdAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public IdAttribute(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("ID cannot be null or white space.", nameof(id));
            }

            if (Guid.TryParse(id, out var result))
            {
                Id = result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute" /> class.
        /// </summary>
        protected IdAttribute()
        {
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id { get; }
    }
}
