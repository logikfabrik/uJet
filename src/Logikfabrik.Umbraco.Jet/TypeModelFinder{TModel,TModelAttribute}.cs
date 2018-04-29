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
    public class TypeModelFinder<TModel, TModelAttribute>
        where TModel : TypeModel<TModelAttribute>
        where TModelAttribute : TypeModelAttribute
    {
        private readonly TypeModelComparer<TModel, TModelAttribute> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeModelFinder{TModel, TModelAttribute}" /> class.
        /// </summary>
        public TypeModelFinder()
        {
            _comparer = new TypeModelComparer<TModel, TModelAttribute>();
        }

        /// <summary>
        /// Finds the models with a model type matching any of the specified model types.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <remarks>A model is a match if a model type needle is the same type as the model's model type.</remarks>
        public TModel[] Find(Type[] modelTypeNeedles, TModel[] modelsHaystack)
        {
            EnsureArg.IsNotNull(modelTypeNeedles);
            EnsureArg.IsTrue(modelTypeNeedles.All(needle => needle.IsModelType<TModelAttribute>()), nameof(modelTypeNeedles));
            EnsureArg.IsNotNull(modelsHaystack);

            var models = modelTypeNeedles.SelectMany(needle => Find(needle, modelsHaystack)).Distinct(_comparer).ToArray();

            return models;
        }

        /// <summary>
        /// Finds the models with a model type matching the specified model type.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <remarks>A model is a match if the model type needle is the same type as the model's model type.</remarks>
        public TModel[] Find(Type modelTypeNeedle, TModel[] modelsHaystack)
        {
            EnsureArg.IsNotNull(modelTypeNeedle);
            EnsureArg.IsTrue(modelTypeNeedle.IsModelType<TModelAttribute>(), nameof(modelTypeNeedle));
            EnsureArg.IsNotNull(modelsHaystack);

            return modelsHaystack.Where(model => model.ModelType == modelTypeNeedle).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds all models with a model type matching any of the specified model types.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <remarks>A model is a match if a model type needle is assignable from the model's model type.</remarks>
        public TModel[] FindAll(Type[] modelTypeNeedles, TModel[] modelsHaystack)
        {
            EnsureArg.IsNotNull(modelTypeNeedles);
            EnsureArg.IsNotNull(modelsHaystack);

            return modelTypeNeedles.SelectMany(needle => FindAll(needle, modelsHaystack)).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds all models with a model type matching the specified model type.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <remarks>A model is a match if the model type needle is assignable from the model's model type.</remarks>
        public TModel[] FindAll(Type modelTypeNeedle, TModel[] modelsHaystack)
        {
            EnsureArg.IsNotNull(modelTypeNeedle);
            EnsureArg.IsNotNull(modelsHaystack);

            return modelsHaystack.Where(model => modelTypeNeedle.IsAssignableFrom(model.ModelType)).Distinct(_comparer).ToArray();
        }
    }
}