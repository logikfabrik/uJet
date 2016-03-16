// <copyright file="ContentTypeModelValidator{TModel,TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="ContentTypeModelValidator{TModel,TModelAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The <see cref="ContentTypeModel{T}" /> type.</typeparam>
    /// <typeparam name="TModelAttribute">The <see cref="ContentTypeModelAttribute" /> type.</typeparam>
    public class ContentTypeModelValidator<TModel, TModelAttribute> : TypeModelValidator<TModel, TModelAttribute>
        where TModel : ContentTypeModel<TModelAttribute>
        where TModelAttribute : ContentTypeModelAttribute
    {
        /// <summary>
        /// Validates the specified models.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="models" /> is <c>null</c>.</exception>
        public override void Validate(TModel[] models)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            ValidateById(models);
            ValidateByAlias(models);
        }

        private static void ValidateByAlias(TModel[] models)
        {
            var set = new HashSet<string>();

            foreach (var model in models)
            {
                if (set.Contains(model.Alias, StringComparer.InvariantCultureIgnoreCase))
                {
                    var conflictingTypes = models.Where(m => m.Alias.Equals(model.Alias, StringComparison.InvariantCultureIgnoreCase)).Select(m => m.ModelType.Name);

                    throw new InvalidOperationException($"Alias conflict for types {string.Join(", ", conflictingTypes)}. Alias {model.Alias} is already in use.");
                }

                set.Add(model.Alias);
            }
        }
    }
}
