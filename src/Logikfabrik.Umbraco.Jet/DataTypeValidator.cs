// <copyright file="DataTypeValidator.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;

    /// <summary>
    /// The <see cref="DataTypeValidator" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DataTypeValidator : TypeModelValidator<DataType, DataTypeAttribute>
    {
        /// <inheritdoc />
        public override void Validate(DataType[] models)
        {
            EnsureArg.IsNotNull(models);

            ValidateById(models);
            ValidateByName(models);
        }

        private static void ValidateByName(DataType[] models)
        {
            var set = new HashSet<string>();

            foreach (var model in models)
            {
                // ReSharper disable once PossibleUnintendedLinearSearchInSet
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