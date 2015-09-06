// <copyright file="HtmlStringPropertyValueConverter.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Web;

    /// <summary>
    /// The <see cref="HtmlStringPropertyValueConverter" /> class.
    /// </summary>
    public class HtmlStringPropertyValueConverter : IPropertyValueConverter
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="from" /> or <paramref name="to" /> are <c>null</c>.</exception>
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

            return from == typeof(HtmlString) && to == typeof(string);
        }

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To type.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type to)
        {
            return ((HtmlString)value)?.ToHtmlString();
        }
    }
}
