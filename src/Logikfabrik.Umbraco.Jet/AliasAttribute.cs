// <copyright file="IdAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="AliasAttribute" /> class.
    /// </summary>
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AliasAttribute" /> class.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="alias" /> is <c>null</c> or white space.</exception>
        public AliasAttribute(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentException("Alias cannot be null or white space.", nameof(alias));
            }

            Alias = alias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasAttribute" /> class.
        /// </summary>
        protected AliasAttribute()
        {
        }

        public string Alias { get; }

    }
}
