// <copyright file="ContentTypeModel{TModelTypeAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using EnsureThat;
    using Extensions;

    /// <summary>
    /// The <see cref="ContentTypeModel{TModelTypeAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModelTypeAttribute">The attribute type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ContentTypeModel<TModelTypeAttribute> : Model<TModelTypeAttribute>
        where TModelTypeAttribute : ContentTypeModelTypeAttribute
    {
        private readonly Lazy<IEnumerable<PropertyType>> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeModel{TModelTypeAttribute}" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ContentTypeModel(Type modelType)
            : base(modelType)
        {
            _properties = new Lazy<IEnumerable<PropertyType>>(GetProperties);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => Attribute.Name;

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias => string.IsNullOrEmpty(Attribute.Alias) ? ModelType.Name.Alias() : Attribute.Alias;

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public IEnumerable<PropertyType> Properties => _properties.Value;

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description => Attribute.Description;

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string Icon => Attribute.Icon;

        /// <summary>
        /// Gets a value indicating whether content of this type are containers.
        /// </summary>
        /// <value>
        ///   <c>true</c> if containers; otherwise, <c>false</c>.
        /// </value>
        public bool IsContainer => Attribute.IsContainer;

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <returns>The properties.</returns>
        protected virtual IEnumerable<PropertyType> GetProperties()
        {
            return ModelType.GetProperties().Where(IsValidProperty).Select(property => new PropertyType(property));
        }

        /// <summary>
        /// Determines whether the specified property is valid.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        ///   <c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsValidProperty(PropertyInfo property)
        {
            // TODO: Break out to separate class.
            Ensure.That(property).IsNotNull();

            if (!property.CanRead || !property.CanWrite)
            {
                return false;
            }

            var attribute = property.GetCustomAttribute<ScaffoldColumnAttribute>();

            return attribute == null || attribute.Scaffold;
        }
    }
}