// <copyright file="TypeModelFinder{TModel,TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
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
        private readonly TypeModelComparer<TModel, TModelAttribute> _comparer = new TypeModelComparer<TModel, TModelAttribute>();

        /// <summary>
        /// Finds the models with a model type matching any of the specified model types.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelTypeNeedles" />, or <paramref name="modelsHaystack" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if a type in <paramref name="modelTypeNeedles" /> is not a model type.</exception>
        /// <remarks>A model is a match if a model type needle is the same type as the model's model type.</remarks>
        public TModel[] Find(Type[] modelTypeNeedles, TModel[] modelsHaystack)
        {
            if (modelTypeNeedles == null)
            {
                throw new ArgumentNullException(nameof(modelTypeNeedles));
            }

            foreach (var needle in modelTypeNeedles)
            {
                if (!needle.IsModelType<TModelAttribute>())
                {
                    throw new ArgumentException($"Type {needle} is not a model type.", nameof(modelTypeNeedles));
                }
            }

            if (modelsHaystack == null)
            {
                throw new ArgumentNullException(nameof(modelsHaystack));
            }

            return modelTypeNeedles.SelectMany(needle => Find(needle, modelsHaystack)).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds the models with a model type matching the specified model type.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelTypeNeedle" />, or <paramref name="modelsHaystack" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="modelTypeNeedle" /> is not a model type.</exception>
        /// <remarks>A model is a match if the model type needle is the same type as the model's model type.</remarks>
        public TModel[] Find(Type modelTypeNeedle, TModel[] modelsHaystack)
        {
            if (modelTypeNeedle == null)
            {
                throw new ArgumentNullException(nameof(modelTypeNeedle));
            }

            if (!modelTypeNeedle.IsModelType<TModelAttribute>())
            {
                throw new ArgumentException($"Type {modelTypeNeedle} is not a model type.", nameof(modelTypeNeedle));
            }

            if (modelsHaystack == null)
            {
                throw new ArgumentNullException(nameof(modelsHaystack));
            }

            return modelsHaystack.Where(model => model.ModelType == modelTypeNeedle).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds all models with a model type matching any of the specified model types.
        /// </summary>
        /// <param name="modelTypeNeedles">The model types to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelTypeNeedles" />, or <paramref name="modelsHaystack" /> are <c>null</c>.</exception>
        /// <remarks>A model is a match if a model type needle is assignable from the model's model type.</remarks>
        public TModel[] FindAll(Type[] modelTypeNeedles, TModel[] modelsHaystack)
        {
            if (modelTypeNeedles == null)
            {
                throw new ArgumentNullException(nameof(modelTypeNeedles));
            }

            if (modelsHaystack == null)
            {
                throw new ArgumentNullException(nameof(modelsHaystack));
            }

            return modelTypeNeedles.SelectMany(needle => FindAll(needle, modelsHaystack)).Distinct(_comparer).ToArray();
        }

        /// <summary>
        /// Finds all models with a model type matching the specified model type.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelTypeNeedle" />, or <paramref name="modelsHaystack" /> are <c>null</c>.</exception>
        /// <remarks>A model is a match if the model type needle is assignable from the model's model type.</remarks>
        public TModel[] FindAll(Type modelTypeNeedle, TModel[] modelsHaystack)
        {
            if (modelTypeNeedle == null)
            {
                throw new ArgumentNullException(nameof(modelTypeNeedle));
            }

            if (modelsHaystack == null)
            {
                throw new ArgumentNullException(nameof(modelsHaystack));
            }

            return modelsHaystack.Where(model => modelTypeNeedle.IsAssignableFrom(model.ModelType)).Distinct(_comparer).ToArray();
        }
    }
}