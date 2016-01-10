// <copyright file="TypeModelValidator{TTypeModel,TTypeModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="TypeModelValidator{TTypeModel,TTypeModelAttribute}"/> class.
    /// </summary>
    /// <typeparam name="TTypeModel">The <see cref="TypeModel{T}"/> type.</typeparam>
    /// <typeparam name="TTypeModelAttribute">The <see cref="TypeModelAttribute"/> type.</typeparam>
    public abstract class TypeModelValidator<TTypeModel, TTypeModelAttribute>
        where TTypeModel : TypeModel<TTypeModelAttribute>
        where TTypeModelAttribute : TypeModelAttribute
    {
        /// <summary>
        /// Validates the specified models by identifier.
        /// </summary>
        /// <param name="models">The models.</param>
        protected void ValidateById(TTypeModel[] models)
        {
            var set = new HashSet<Guid>();

            foreach (var model in models)
            {
                if (!model.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(model.Id.Value))
                {
                    var conflictingTypes = models.Where(m => m.Id.HasValue && m.Id.Value == model.Id.Value).Select(m => m.ModelType.Name);

                    throw new InvalidOperationException($"ID conflict for types {string.Join(", ", conflictingTypes)}. ID {model.Id.Value} is already in use.");
                }

                set.Add(model.Id.Value);
            }
        }
    }
}