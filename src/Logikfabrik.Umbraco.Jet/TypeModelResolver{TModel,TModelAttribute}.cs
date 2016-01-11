// <copyright file="TypeModelResolver{TModel,TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="TypeModelResolver{TModel,TModelAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The <see cref="TypeModel{T}" /> type.</typeparam>
    /// <typeparam name="TModelAttribute">The <see cref="TypeModelAttribute" /> type.</typeparam>
    public class TypeModelResolver<TModel, TModelAttribute>
        where TModel : TypeModel<TModelAttribute>
        where TModelAttribute : TypeModelAttribute
    {
        /// <summary>
        /// Resolves models using the specified model types.
        /// </summary>
        /// <param name="modelTypes">The model types.</param>
        /// <param name="models">The type models.</param>
        /// <returns>The resolved models.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="modelTypes" />, or <paramref name="models" /> are <c>null</c>.</exception>
        public IEnumerable<TModel> ResolveByModelTypes(Type[] modelTypes, TModel[] models)
        {
            if (modelTypes == null)
            {
                throw new ArgumentNullException(nameof(modelTypes));
            }

            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            return modelTypes.SelectMany(modelType => ResolveByModelType(modelType, models)).Distinct();
        }

        private IEnumerable<TModel> ResolveByModelType(Type modelType, TModel[] models)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            return models.Where(model => modelType.IsAssignableFrom(model.ModelType));
        }
    }
}