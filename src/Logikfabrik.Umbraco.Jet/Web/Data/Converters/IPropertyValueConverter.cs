// <copyright file="IPropertyValueConverter.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;

    /// <summary>
    /// The <see cref="IPropertyValueConverter" /> interface.
    /// </summary>
    public interface IPropertyValueConverter
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
        bool CanConvertValue(string uiHint, Type from, Type to);

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="to">The type to convert to.</param>
        /// <returns>The converted value.</returns>
        object Convert(object value, Type to);
    }
}
