// <copyright file="TypeModelFinder{TModel,TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using EnsureThat;
    using Extensions;

    /// <summary>
    /// The <see cref="TypeModelFinder{TModel,TModelAttribute}" /> class. Class for finding models by model type.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelAttribute">The model attribute type.</typeparam>
    public class TypeModelFinder<TModel, TModelAttribute> : ITypeModelFinder<TModel, TModelAttribute>
        where TModel : TypeModel<TModelAttribute>
        where TModelAttribute : TypeModelAttribute
    {
        /// <summary>
        /// Gets the comparer.
        /// </summary>
        /// <value>
        /// The comparer.
        /// </value>
        protected TypeModelComparer<TModel, TModelAttribute> Comparer { get; } = new TypeModelComparer<TModel, TModelAttribute>();

        /// <inheritdoc />
        public TModel[] Find(Type modelTypeNeedle, TModel[] modelsHaystack)
        {
            Ensure.That(modelTypeNeedle).IsNotNull();
            Ensure.That(() => modelTypeNeedle.IsModelType<TModelAttribute>(), nameof(modelTypeNeedle)).IsTrue();
            Ensure.That(modelsHaystack).IsNotNull();

            return modelsHaystack.Where(model => model.ModelType == modelTypeNeedle).Distinct(Comparer).ToArray();
        }

        /// <inheritdoc />
        public TModel[] FindAll(Type modelTypeNeedle, TModel[] modelsHaystack)
        {
            Ensure.That(modelTypeNeedle).IsNotNull();
            Ensure.That(modelsHaystack).IsNotNull();

            return modelsHaystack.Where(model => modelTypeNeedle.IsAssignableFrom(model.ModelType)).Distinct(Comparer).ToArray();
        }
    }
}