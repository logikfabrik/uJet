// <copyright file="TypeModelAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="TypeModelAttribute" /> class.
    /// </summary>
    public abstract class TypeModelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeModelAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id" /> is <c>null</c> or white space.</exception>
        protected TypeModelAttribute(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("ID cannot be null or white space.", nameof(id));
            }

            Guid result;

            if (Guid.TryParse(id, out result))
            {
                Id = result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeModelAttribute" /> class.
        /// </summary>
        protected TypeModelAttribute()
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