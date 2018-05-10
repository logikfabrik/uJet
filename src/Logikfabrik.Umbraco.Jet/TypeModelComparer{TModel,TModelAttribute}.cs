// <copyright file="TypeModelComparer{TModel,TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="TypeModelComparer{TModel,TModelAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelAttribute">The model attribute type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public class TypeModelComparer<TModel, TModelAttribute> : IEqualityComparer<TModel>
        where TModel : Model<TModelAttribute>
        where TModelAttribute : ModelTypeAttribute
    {
        /// <inheritdoc />
        public bool Equals(TModel x, TModel y)
        {
            return x?.ModelType == y?.ModelType;
        }

        /// <inheritdoc />
        public int GetHashCode(TModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
