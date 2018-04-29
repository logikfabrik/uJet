﻿// <copyright file="EntityTypeComparer{TEntityType}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models.EntityBase;

    /// <inheritdoc />
    public class EntityTypeComparer<TEntityType> : IEqualityComparer<TEntityType>
        where TEntityType : IEntity
    {
        /// <inheritdoc />
        public bool Equals(TEntityType x, TEntityType y)
        {
            return x?.Id == y?.Id;
        }

        /// <inheritdoc />
        public int GetHashCode(TEntityType obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}