// <copyright file="ContentTypeModelValidator{T1,T2}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="ContentTypeModelValidator{T1,T2}" /> class.
    /// </summary>
    /// <typeparam name="T1">The <see cref="ContentTypeModel{T}" /> type.</typeparam>
    /// <typeparam name="T2">The <see cref="ContentTypeModelAttribute" /> type.</typeparam>
    public class ContentTypeModelValidator<T1, T2> : TypeModelValidator<T1, T2>
        where T1 : ContentTypeModel<T2>
        where T2 : ContentTypeModelAttribute
    {
        /// <summary>
        /// Validates the specified models.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="models" /> is <c>null</c>.</exception>
        public void Validate(T1[] models)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            ValidateById(models);
            ValidateByAlias(models);
        }

        private static void ValidateByAlias(T1[] models)
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
