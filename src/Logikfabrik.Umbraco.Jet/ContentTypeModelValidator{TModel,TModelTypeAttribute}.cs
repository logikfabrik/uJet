// <copyright file="ContentTypeModelValidator{TModel,TModelTypeAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;

    /// <summary>
    /// The <see cref="ContentTypeModelValidator{TModel,TModelTypeAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public class ContentTypeModelValidator<TModel, TModelTypeAttribute> : TypeModelValidator<TModel, TModelTypeAttribute>
        where TModel : ContentTypeModel<TModelTypeAttribute>
        where TModelTypeAttribute : ContentTypeModelTypeAttribute
    {
        /// <inheritdoc />
        public override void Validate(TModel[] models)
        {
            Ensure.That(models).IsNotNull();

            ValidateById(models);
            ValidateByAlias(models);
            ValidatePropertiesById(models);
            ValidatePropertiesByAlias(models);
        }

        private static void ValidateByAlias(TModel[] models)
        {
            var set = new HashSet<string>();

            foreach (var model in models)
            {
                // ReSharper disable once PossibleUnintendedLinearSearchInSet
                if (set.Contains(model.Alias, StringComparer.InvariantCultureIgnoreCase))
                {
                    var conflictingTypes = models.Where(m => m.Alias.Equals(model.Alias, StringComparison.InvariantCultureIgnoreCase)).Select(m => m.ModelType.Name);

                    throw new InvalidOperationException($"Alias conflict for types '{string.Join("', '", conflictingTypes)}'. Alias '{model.Alias}' is already in use.");
                }

                set.Add(model.Alias);
            }
        }

        private static void ValidatePropertiesById(IEnumerable<TModel> models)
        {
            foreach (var model in models)
            {
                ValidatePropertiesById(model);
            }
        }

        private static void ValidatePropertiesByAlias(IEnumerable<TModel> models)
        {
            foreach (var model in models)
            {
                ValidatePropertiesByAlias(model);
            }
        }

        private static void ValidatePropertiesById(TModel model)
        {
            var set = new HashSet<string>();

            foreach (var property in model.Properties)
            {
                // ReSharper disable once PossibleUnintendedLinearSearchInSet
                if (set.Contains(property.Alias, StringComparer.InvariantCultureIgnoreCase))
                {
                    var conflictingProperties = model.Properties.Where(m => m.Alias.Equals(property.Alias, StringComparison.InvariantCultureIgnoreCase)).Select(m => m.Name);

                    throw new InvalidOperationException($"Alias conflict for properties '{string.Join("', '", conflictingProperties)}'. Alias '{property.Alias}' is already in use.");
                }

                set.Add(property.Alias);
            }
        }

        private static void ValidatePropertiesByAlias(TModel model)
        {
            var set = new HashSet<Guid>();

            foreach (var property in model.Properties)
            {
                if (!property.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(property.Id.Value))
                {
                    var conflictingProperties = model.Properties.Where(m => m.Id.HasValue && m.Id.Value == property.Id.Value).Select(m => m.Name);

                    throw new InvalidOperationException($"ID conflict for properties '{string.Join("', '", conflictingProperties)}'. ID '{property.Id.Value}' is already in use.");
                }

                set.Add(property.Id.Value);
            }
        }
    }
}
