// <copyright file="ModelFinder{TModel,TModelTypeAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using EnsureThat;

    /// <summary>
    /// The <see cref="ModelFinder{TModel,TModelTypeAttribute}" /> class. Class for finding models by model type.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public class ModelFinder<TModel, TModelTypeAttribute> : IModelFinder<TModel, TModelTypeAttribute>
        where TModel : Model<TModelTypeAttribute>
        where TModelTypeAttribute : ModelTypeAttribute
    {
        /// <summary>
        /// Gets the comparer.
        /// </summary>
        /// <value>
        /// The comparer.
        /// </value>
        protected ModelComparer<TModel, TModelTypeAttribute> Comparer { get; } = new ModelComparer<TModel, TModelTypeAttribute>();

        /// <inheritdoc />
        public TModel[] Find(Type modelTypeNeedle, TModel[] modelsHaystack)
        {
            Ensure.That(modelTypeNeedle).IsNotNull();
            Ensure.That(() => modelTypeNeedle.IsModelType<TModelTypeAttribute>(), nameof(modelTypeNeedle)).IsTrue();
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