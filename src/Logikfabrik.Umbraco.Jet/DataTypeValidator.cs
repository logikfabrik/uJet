// <copyright file="DataTypeValidator.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="DataTypeValidator" /> class.
    /// </summary>
    public class DataTypeValidator : TypeModelValidator<DataType, DataTypeAttribute>
    {
        /// <summary>
        /// Validates the specified models.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="models" /> is <c>null</c>.</exception>
        public override void Validate(DataType[] models)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            ValidateById(models);
            ValidateByName(models);
        }

        private static void ValidateByName(DataType[] models)
        {
            var set = new HashSet<string>();

            foreach (var model in models)
            {
                if (set.Contains(model.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    var conflictingTypes = models.Where(m => m.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)).Select(m => m.ModelType.Name);

                    throw new InvalidOperationException($"Name conflict for types {string.Join(", ", conflictingTypes)}. Name {model.Name} is already in use.");
                }

                set.Add(model.Name);
            }
        }
    }
}