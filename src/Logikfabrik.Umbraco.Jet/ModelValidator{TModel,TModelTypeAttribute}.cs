// <copyright file="ModelValidator{TModel,TModelTypeAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="ModelValidator{TModel,TModelTypeAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    public abstract class ModelValidator<TModel, TModelTypeAttribute>
        where TModel : Model<TModelTypeAttribute>
        where TModelTypeAttribute : ModelTypeAttribute
    {
        /// <summary>
        /// Validates the specified models.
        /// </summary>
        /// <param name="models">The models.</param>
        public abstract void Validate(TModel[] models);

        /// <summary>
        /// Validates the specified models by identifier.
        /// </summary>
        /// <param name="models">The models.</param>
        protected void ValidateById(TModel[] models)
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

                    throw new InvalidOperationException($"ID conflict for types '{string.Join("', '", conflictingTypes)}'. ID '{model.Id.Value}' is already in use.");
                }

                set.Add(model.Id.Value);
            }
        }
    }
}