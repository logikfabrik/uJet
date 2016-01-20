// <copyright file="EntityTypeComparer{TEntityType}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models.EntityBase;

    /// <summary>
    /// The <see cref="EntityTypeComparer{TEntityType}" /> class.
    /// </summary>
    /// <typeparam name="TEntityType">The entity type.</typeparam>
    public class EntityTypeComparer<TEntityType> : IEqualityComparer<TEntityType>
        where TEntityType : IEntity
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <c>true</c> if the specified objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TEntityType x, TEntityType y)
        {
            return x.Id == y.Id;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public int GetHashCode(TEntityType obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}