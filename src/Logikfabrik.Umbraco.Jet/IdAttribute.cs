// <copyright file="IdAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    public class IdAttribute : Attribute
    {
        public IdAttribute(string id)
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

        protected IdAttribute()
        {
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        public Guid? Id { get; }
    }
}
