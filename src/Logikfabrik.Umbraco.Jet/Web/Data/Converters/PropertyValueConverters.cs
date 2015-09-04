//----------------------------------------------------------------------------------
// <copyright file="PropertyValueConverters.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="PropertyValueConverters" /> class.
    /// </summary>
    public class PropertyValueConverters
    {
        /// <summary>
        /// The shared converters.
        /// </summary>
        private static readonly PropertyValueConverterDictionary SharedConverters = GetDefaultConverters();

        /// <summary>
        /// Gets the converters.
        /// </summary>
        /// <value>
        /// The converters.
        /// </value>
        public static PropertyValueConverterDictionary Converters
        {
            get { return SharedConverters; }
        }

        /// <summary>
        /// Gets the converter.
        /// </summary>
        /// <param name="uiHint">The UI hint.</param>
        /// <param name="from">From type.</param>
        /// <param name="to">To type.</param>
        /// <returns>The converter</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if from or to are null.</exception>
        internal static IPropertyValueConverter GetConverter(string uiHint, Type from, Type to)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            IEnumerable<IPropertyValueConverter> converters;

            return !SharedConverters.TryGetValue(to, out converters)
                ? null
                : converters.FirstOrDefault(c => c.CanConvertValue(uiHint, @from, to));
        }

        /// <summary>
        /// Gets the default converters.
        /// </summary>
        /// <returns>The default converters.</returns>
        private static PropertyValueConverterDictionary GetDefaultConverters()
        {
            return new PropertyValueConverterDictionary
                {
                    { typeof(string), new[] { new HtmlStringPropertyValueConverter() } },
                    { typeof(decimal), new[] { new FloatingDecimalPointPropertyValueConverter() } },
                    { typeof(decimal?), new[] { new FloatingDecimalPointPropertyValueConverter() } },
                    { typeof(float), new[] { new FloatingBinaryPointPropertyValueConverter() } },
                    { typeof(float?), new[] { new FloatingBinaryPointPropertyValueConverter() } },
                    { typeof(double), new[] { new FloatingBinaryPointPropertyValueConverter() } },
                    { typeof(double?), new[] { new FloatingBinaryPointPropertyValueConverter() } }
                };
        }
    }
}
