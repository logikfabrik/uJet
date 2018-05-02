// <copyright file="ITypeModelFinder{TModel,TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    public interface ITypeModelFinder<TModel, TModelAttribute>
        where TModel : TypeModel<TModelAttribute>
        where TModelAttribute : TypeModelAttribute
    {
        /// <summary>
        /// Finds the models with a model type matching the specified model type.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <remarks>A model is a match if the model type needle is the same type as the model's model type.</remarks>
        TModel[] Find(Type modelTypeNeedle, TModel[] modelsHaystack);

        /// <summary>
        /// Finds all models with a model type matching the specified model type.
        /// </summary>
        /// <param name="modelTypeNeedle">The model type to find the models for.</param>
        /// <param name="modelsHaystack">The haystack of models.</param>
        /// <returns>The models found.</returns>
        /// <remarks>A model is a match if the model type needle is assignable from the model's model type.</remarks>
        TModel[] FindAll(Type modelTypeNeedle, TModel[] modelsHaystack);
    }
}