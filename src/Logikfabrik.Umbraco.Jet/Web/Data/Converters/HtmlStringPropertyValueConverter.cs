// <copyright file="HtmlStringPropertyValueConverter.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Web;
    using EnsureThat;

    /// <summary>
    /// The <see cref="HtmlStringPropertyValueConverter" /> class for types implementing interface <see cref="IHtmlString" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class HtmlStringPropertyValueConverter : IPropertyValueConverter
    {
        /// <inheritdoc />
        public bool CanConvertValue(string uiHint, Type from, Type to)
        {
            EnsureArg.IsNotNull(from);
            EnsureArg.IsNotNull(to);

            return typeof(IHtmlString).IsAssignableFrom(from) && to == typeof(string);
        }

        /// <inheritdoc />
        public object Convert(object value, Type to)
        {
            return ((IHtmlString)value)?.ToHtmlString();
        }
    }
}
