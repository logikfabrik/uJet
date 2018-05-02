// <copyright file="IdAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using EnsureThat;

    /// <summary>
    /// The <see cref="IdAttribute" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class IdAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public IdAttribute(string id)
        {
            EnsureArg.IsNotNullOrWhiteSpace(id);

            if (Guid.TryParse(id, out var result))
            {
                Id = result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
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
