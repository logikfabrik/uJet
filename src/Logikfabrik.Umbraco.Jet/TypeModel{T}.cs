// <copyright file="TypeModel{T}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Reflection;
    using EnsureThat;
    using Extensions;

    /// <summary>
    /// The <see cref="TypeModel{T}" /> class.
    /// </summary>
    /// <typeparam name="T">The attribute type.</typeparam>
    public abstract class TypeModel<T>
        where T : TypeModelAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeModel{T}" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        protected TypeModel(Type modelType)
        {
            Ensure.That(modelType).IsNotNull();
            Ensure.That(() => modelType.IsModelType<T>(), nameof(modelType)).IsTrue();

            ModelType = modelType;
            Attribute = modelType.GetCustomAttribute<T>(false);
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
        /// Gets the type model attribute.
        /// </summary>
        /// <value>
        /// The type model attribute.
        /// </value>
        protected T Attribute { get; }
    }
}