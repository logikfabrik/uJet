// <copyright file="TypeModelResolver{TTypeModel,TTypeModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="TypeModelResolver{TTypeModel,TTypeModelAttribute}"/> class.
    /// </summary>
    /// <typeparam name="TTypeModel">The <see cref="TypeModel{T}"/> type.</typeparam>
    /// <typeparam name="TTypeModelAttribute">The <see cref="TypeModelAttribute"/> type.</typeparam>
    public class TypeModelResolver<TTypeModel, TTypeModelAttribute>
        where TTypeModel : TypeModel<TTypeModelAttribute>
        where TTypeModelAttribute : TypeModelAttribute
    {
        /// <summary>
        /// Resolves the type models by the specified model types.
        /// </summary>
        /// <param name="modelTypes">The model types.</param>
        /// <param name="models">The type models.</param>
        /// <returns>The resolved type models.</returns>
        public IEnumerable<TTypeModel> ResolveByModelTypes(Type[] modelTypes, TTypeModel[] models)
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

        /// <summary>
        /// Resolves the type models by the specified model type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <param name="models">The type models.</param>
        /// <returns>The resolved type models.</returns>
        public IEnumerable<TTypeModel> ResolveByModelType(Type modelType, TTypeModel[] models)
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