// <copyright file="ContentMapper.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Converters;

    /// <summary>
    /// The <see cref="ContentMapper" /> class.
    /// </summary>
    public class ContentMapper
    {
        /// <summary>
        /// Maps the property.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="propertyName" /> is <c>null</c> or white space.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="model" /> is <c>null</c>.</exception>
        public void MapProperty(object model, string propertyName, object propertyValue)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Property name cannot be null or white space.", nameof(propertyName));
            }

            var property = model.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null || !property.CanWrite)
            {
                return;
            }

            if (propertyValue == null && Nullable.GetUnderlyingType(property.PropertyType) != null)
            {
                property.SetValue(model, null);
            }
            else if (propertyValue != null)
            {
                if (property.PropertyType.IsInstanceOfType(propertyValue))
                {
                    property.SetValue(model, propertyValue);
                }
                else
                {
                    var uiHint = GetPropertyUIHint(property);
                    var converter = GetPropertyConverter(uiHint, propertyValue.GetType(), property.PropertyType);

                    if (converter != null)
                    {
                        property.SetValue(model, converter.Convert(propertyValue, property.PropertyType));
                    }
                }
            }
        }

        /// <summary>
        /// Gets a property converter.
        /// </summary>
        /// <param name="uiHint">The UI hint.</param>
        /// <param name="from">The type to convert from.</param>
        /// <param name="to">The type to convert to.</param>
        /// <returns>A converter.</returns>
        private static IPropertyValueConverter GetPropertyConverter(string uiHint, Type from, Type to)
        {
            return PropertyValueConverters.GetConverter(uiHint, from, to);
        }

        /// <summary>
        /// Gets the UI hint for the specified property.
        /// </summary>
        /// <param name="property">The property to get the UI hint for.</param>
        /// <returns>The UI hint.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="property" /> is <c>null</c>.</exception>
        private string GetPropertyUIHint(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var attribute = property.GetCustomAttribute<UIHintAttribute>();

            var uiHint = attribute?.UIHint;

            return uiHint;
        }
    }
}