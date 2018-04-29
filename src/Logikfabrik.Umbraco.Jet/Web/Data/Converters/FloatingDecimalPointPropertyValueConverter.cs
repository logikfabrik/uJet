// <copyright file="FloatingDecimalPointPropertyValueConverter.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using EnsureThat;

    /// <summary>
    /// The <see cref="FloatingDecimalPointPropertyValueConverter" /> class for type <see cref="decimal" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class FloatingDecimalPointPropertyValueConverter : IPropertyValueConverter
    {
        private readonly Type[] _validTypes = { typeof(decimal), typeof(decimal?) };

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

            if (decimal.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
