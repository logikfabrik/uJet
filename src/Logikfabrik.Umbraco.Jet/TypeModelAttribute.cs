// <copyright file="TypeModelAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using EnsureThat;

    /// <summary>
    /// The <see cref="TypeModelAttribute" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class TypeModelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeModelAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected TypeModelAttribute(string id)
        {
            Ensure.That(id).IsNotNullOrWhiteSpace();

            if (Guid.TryParse(id, out var result))
            {
                Id = result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeModelAttribute" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
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