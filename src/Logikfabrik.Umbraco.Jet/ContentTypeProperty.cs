// <copyright file="ContentTypeProperty.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// The <see cref="ContentTypeProperty" /> class.
    /// </summary>
    public class ContentTypeProperty
    {
        /// <summary>
        /// Value indicating whether this <see cref="ContentTypeProperty" /> has a default value.
        /// </summary>
        private readonly bool hasDefaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeProperty" /> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="property" /> is <c>null</c>.</exception>
        public ContentTypeProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Type = property.PropertyType;
            Id = GetId(property);
            Mandatory = GetIsMandatory(property);
            Alias = GetAlias(property);
            RegularExpression = GetRegularExpression(property);
            UIHint = GetUIHint(property);
            DefaultValue = GetDefaultValue(property, out hasDefaultValue);

            var attribute = property.GetCustomAttribute<DisplayAttribute>();

            Name = GetName(property, attribute);
            SortOrder = GetSortOrder(attribute);
            Description = GetDescription(attribute);
            PropertyGroup = GetPropertyGroup(attribute);
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ContentTypeProperty" /> is mandatory.
        /// </summary>
        /// <value>
        ///   <c>true</c> if mandatory; otherwise, <c>false</c>.
        /// </value>
        public bool Mandatory { get; }

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public int? SortOrder { get; }

        /// <summary>
        /// Gets the property group.
        /// </summary>
        /// <value>
        /// The property group.
        /// </value>
        public string PropertyGroup { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; }

        /// <summary>
        /// Gets the regular expression.
        /// </summary>
        /// <value>
        /// The regular expression.
        /// </value>
        public string RegularExpression { get; }

        /// <summary>
        /// Gets the UI hint.
        /// </summary>
        /// <value>
        /// The UI hint.
        /// </value>
        public string UIHint { get; }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ContentTypeProperty" /> has a default value.
        /// </summary>
        public bool HasDefaultValue => hasDefaultValue;

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The identifier.</returns>
        private static Guid? GetId(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<IdAttribute>();

            return attribute?.Id;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The name.</returns>
        private static string GetName(PropertyInfo property, DisplayAttribute attribute)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            string name = null;

            if (attribute != null)
            {
                name = attribute.GetName();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = property.Name;
            }

            return name;
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The alias.</returns>
        private static string GetAlias(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.Name.Alias();
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The description.</returns>
        private static string GetDescription(DisplayAttribute attribute)
        {
            return attribute?.GetDescription();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ContentTypeProperty" /> is mandatory.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        ///   <c>true</c> if mandatory; otherwise, <c>false</c>.
        /// </returns>
        private static bool GetIsMandatory(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.GetCustomAttribute<RequiredAttribute>() != null;
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="hasDefaultValue"><c>true</c> if this <see cref="ContentTypeProperty" /> has a default value; otherwise, <c>false</c>.</param>
        /// <returns>The default value.</returns>
        private static object GetDefaultValue(PropertyInfo property, out bool hasDefaultValue)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

            hasDefaultValue = attribute != null;

            return hasDefaultValue ? attribute.Value : null;
        }

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The sort order.</returns>
        private static int? GetSortOrder(DisplayAttribute attribute)
        {
            return attribute?.GetOrder();
        }

        /// <summary>
        /// Gets the property group.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The property group.</returns>
        private static string GetPropertyGroup(DisplayAttribute attribute)
        {
            return attribute?.GetGroupName();
        }

        /// <summary>
        /// Gets the regular expression.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The regular expression.</returns>
        private static string GetRegularExpression(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<RegularExpressionAttribute>();

            return attribute?.Pattern;
        }

        /// <summary>
        /// Gets the UI hint.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The UI hint.</returns>
        private static string GetUIHint(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<UIHintAttribute>();

            return attribute?.UIHint;
        }
    }
}