// <copyright file="FloatingBinaryPointPropertyValueConverter.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using EnsureThat;

    /// <summary>
    /// The <see cref="FloatingBinaryPointPropertyValueConverter" /> class for types <see cref="float" /> and <see cref="double" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class FloatingBinaryPointPropertyValueConverter : IPropertyValueConverter
    {
        private readonly Type[] _validTypes = { typeof(float), typeof(float?), typeof(double), typeof(double?) };

        /// <inheritdoc />
        public bool CanConvertValue(string uiHint, Type from, Type to)
        {
            EnsureArg.IsNotNull(from);
            EnsureArg.IsNotNull(to);

            return from == typeof(string) && _validTypes.Contains(to);
        }

        /// <inheritdoc />
        public object Convert(object value, Type to)
        {
            if (value == null)
            {
                return null;
            }

            if (to == typeof(float) || to == typeof(float?))
            {
                return ConvertToFloat(value);
            }

            if (to == typeof(double) || to == typeof(double?))
            {
                return ConvertToDouble(value);
            }

            return null;
        }

        /// <summary>
        /// Converts to float.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        private static object ConvertToFloat(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (float.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Converts to double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        private static object ConvertToDouble(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
