// <copyright file="ModelComparer{TModel,TModelTypeAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="ModelComparer{TModel,TModelTypeAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public class ModelComparer<TModel, TModelTypeAttribute> : IEqualityComparer<TModel>
        where TModel : Model<TModelTypeAttribute>
        where TModelTypeAttribute : ModelTypeAttribute
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
