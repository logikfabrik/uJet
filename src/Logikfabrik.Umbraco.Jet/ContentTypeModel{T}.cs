// <copyright file="ContentTypeModel{T}.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// The <see cref="ContentTypeModel{T}" /> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="ContentTypeModelAttribute" /> type.</typeparam>
    public abstract class ContentTypeModel<T> : TypeModel<T>
        where T : ContentTypeModelAttribute
    {
        private readonly Lazy<IEnumerable<TypeProperty>> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeModel{T}" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        protected ContentTypeModel(Type modelType)
            : base(modelType)
        {
            _properties = new Lazy<IEnumerable<TypeProperty>>(GetProperties);
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
        public string Alias => ModelType.Name.Alias();

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public IEnumerable<TypeProperty> Properties => _properties.Value;

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
        /// Gets the properties.
        /// </summary>
        /// <returns>The properties.</returns>
        protected virtual IEnumerable<TypeProperty> GetProperties()
        {
            return from property in ModelType.GetProperties() where IsValidProperty(property) select new TypeProperty(property);
        }

        /// <summary>
        /// Determines whether the specified property is valid.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        protected bool IsValidProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!property.CanRead || !property.CanWrite)
            {
                return false;
            }

            var attribute = property.GetCustomAttribute<ScaffoldColumnAttribute>();

            return attribute == null || attribute.Scaffold;
        }
    }
}