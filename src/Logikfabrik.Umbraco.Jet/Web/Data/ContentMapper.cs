// <copyright file="ContentMapper.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Converters;
    using EnsureThat;

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
        public void MapProperty(object model, string propertyName, object propertyValue)
        {
            Ensure.That(model).IsNotNull();
            Ensure.That(propertyName).IsNotNullOrWhiteSpace();

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

        private static IPropertyValueConverter GetPropertyConverter(string uiHint, Type from, Type to)
        {
            return PropertyValueConverters.GetConverter(uiHint, from, to);
        }

        // ReSharper disable once InconsistentNaming
        private static string GetPropertyUIHint(MemberInfo property)
        {
            var attribute = property.GetCustomAttribute<UIHintAttribute>();

            var uiHint = attribute?.UIHint;

            return uiHint;
        }
    }
}