// <copyright file="FloatingDecimalPointPropertyValueConverter.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// The <see cref="FloatingDecimalPointPropertyValueConverter" /> class for type <see cref="decimal" />.
    /// </summary>
    public class FloatingDecimalPointPropertyValueConverter : IPropertyValueConverter
    {
        /// <summary>
        /// Determines whether this instance can convert between types.
        /// </summary>
        /// <param name="uiHint">The UI hint.</param>
        /// <param name="from">The type to convert from.</param>
        /// <param name="to">The type to convert to.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert between types; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="from" />, or <paramref name="to" /> are <c>null</c>.</exception>
        public bool CanConvertValue(string uiHint, Type from, Type to)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            var validTypes = new[] { typeof(decimal), typeof(decimal?) };

            return from == typeof(string) && validTypes.Contains(to);
        }

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="to">The type to convert to.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type to)
        {
            if (value == null)
            {
                return null;
            }

            decimal result;

            if (decimal.TryParse(
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
