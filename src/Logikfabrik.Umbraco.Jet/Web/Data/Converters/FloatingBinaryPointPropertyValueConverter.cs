// <copyright file="FloatingBinaryPointPropertyValueConverter.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// The <see cref="FloatingBinaryPointPropertyValueConverter" /> class.
    /// </summary>
    public class FloatingBinaryPointPropertyValueConverter : IPropertyValueConverter
    {
        /// <summary>
        /// Determines whether this instance can convert between types.
        /// </summary>
        /// <param name="uiHint">The UI hint.</param>
        /// <param name="from">From type.</param>
        /// <param name="to">To type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert between types; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="from" />, or <paramref name="to" /> are <c>null</c>.</exception>
        public bool CanConvertValue(string uiHint, Type from, Type to)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(@from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            var validTypes = new[] { typeof(float), typeof(float?), typeof(double), typeof(double?) };

            return from == typeof(string) && validTypes.Contains(to);
        }

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To type.</param>
        /// <returns>The converted value.</returns>
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

            float result;

            if (float.TryParse(
                    value.ToString(),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture.NumberFormat,
                    out result))
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

            double result;

            if (double.TryParse(
                    value.ToString(),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture.NumberFormat,
                    out result))
            {
                return result;
            }

            return null;
        }
    }
}
