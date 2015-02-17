// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Globalization;
using System.Linq;

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    public class FloatingBinaryPointPropertyValueConverter : IPropertyValueConverter
    {
        public bool CanConvertValue(string uiHint, Type from, Type to)
        {
            if (from == null)
                throw new ArgumentNullException("from");

            if (to == null)
                throw new ArgumentNullException("to");

            var validTypes = new[] { typeof(float), typeof(float?), typeof(double), typeof(double?) };

            return from == typeof(string) && (validTypes.Contains(to));
        }

        public object Convert(object propertyValue, Type to)
        {
            if (propertyValue == null)
                return null;

            if (to == typeof(float) || to == typeof(float?))
                return ConvertToFloat(propertyValue);

            if (to == typeof(double) || to == typeof(double?))
                return ConvertToDouble(propertyValue);

            return null;
        }

        private static object ConvertToFloat(object propertyValue)
        {
            if (propertyValue == null)
                return null;

            float result;

            if (float.TryParse(propertyValue.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out result))
                return result;

            return null;
        }

        private static object ConvertToDouble(object propertyValue)
        {
            if (propertyValue == null)
                return null;

            double result;

            if (double.TryParse(propertyValue.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out result))
                return result;

            return null;
        }
    }
}
