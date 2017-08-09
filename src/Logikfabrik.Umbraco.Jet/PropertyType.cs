// <copyright file="PropertyType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// The <see cref="PropertyType" /> class.
    /// </summary>
    public class PropertyType
    {
        private readonly bool _hasDefaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyType" /> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="property" /> is <c>null</c>.</exception>
        public PropertyType(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Type = property.PropertyType;
            Id = property.GetCustomAttribute<IdAttribute>()?.Id;
            Mandatory = property.GetCustomAttribute<RequiredAttribute>() != null;
            Alias = GetAlias(property);
            RegularExpression = property.GetCustomAttribute<RegularExpressionAttribute>()?.Pattern;
            UIHint = property.GetCustomAttribute<UIHintAttribute>()?.UIHint;
            DefaultValue = GetDefaultValue(property, out _hasDefaultValue);

            var attribute = property.GetCustomAttribute<DisplayAttribute>();

            Name = GetName(property, attribute);
            SortOrder = attribute?.GetOrder();
            Description = attribute?.GetDescription();
            PropertyGroup = attribute?.GetGroupName();
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
        /// Gets a value indicating whether this instance is mandatory.
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
        // ReSharper disable once InconsistentNaming
        public string UIHint { get; }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has a default value.
        /// </summary>
        public bool HasDefaultValue => _hasDefaultValue;

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The alias.</returns>
        /// <remarks>Defaults to property name alias for backwards compatibility.</remarks>
        private static string GetAlias(MemberInfo property)
        {
            var alias = property.GetCustomAttribute<AliasAttribute>()?.Alias;

            return string.IsNullOrEmpty(alias) ? property.Name.Alias() : alias;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The name.</returns>
        private static string GetName(PropertyInfo property, DisplayAttribute attribute)
        {
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
        /// Gets the default value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="hasDefaultValue"><c>true</c> if this instance has a default value; otherwise, <c>false</c>.</param>
        /// <returns>The default value.</returns>
        private static object GetDefaultValue(MemberInfo property, out bool hasDefaultValue)
        {
            var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

            hasDefaultValue = attribute != null;

            return hasDefaultValue ? attribute.Value : null;
        }
    }
}