// <copyright file="ModelTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using EnsureThat;

    /// <summary>
    /// The <see cref="ModelTypeAttribute" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ModelTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ModelTypeAttribute(string id)
        {
            Ensure.That(id).IsNotNullOrWhiteSpace();

            if (Guid.TryParse(id, out var result))
            {
                Id = result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTypeAttribute" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        protected ModelTypeAttribute()
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