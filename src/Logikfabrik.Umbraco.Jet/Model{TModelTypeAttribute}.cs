// <copyright file="Model{TModelTypeAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Reflection;
    using EnsureThat;

    /// <summary>
    /// The <see cref="Model{TModelTypeAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    public abstract class Model<TModelTypeAttribute>
        where TModelTypeAttribute : ModelTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model{TModelTypeAttribute}" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        protected Model(Type modelType)
        {
            Ensure.That(modelType).IsNotNull();
            Ensure.That(() => modelType.IsModelType<TModelTypeAttribute>(), nameof(modelType)).IsTrue();

            ModelType = modelType;
            Attribute = modelType.GetCustomAttribute<TModelTypeAttribute>(false);
        }

        /// <summary>
        /// Gets the model type.
        /// </summary>
        /// <value>
        /// The model type.
        /// </value>
        public Type ModelType { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id => Attribute.Id;

        /// <summary>
        /// Gets the model type attribute.
        /// </summary>
        /// <value>
        /// The model type attribute.
        /// </value>
        protected TModelTypeAttribute Attribute { get; }
    }
}