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

using Logikfabrik.Umbraco.Jet.Web.Data.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    [TestClass]
    public class FloatingBinaryPointPropertyValueConverterTest
    {
        [TestMethod]
        public void CanConvertValue()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(float)));
            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(float?)));
            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(double)));
            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(double?)));

            const float value1 = 1.1f;
            float? value2 = null;
            const double value3 = 1.1;
            double? value4 = null;
            var propertyValue1 = value1.ToString(CultureInfo.InvariantCulture);
            // ReSharper disable once ExpressionIsAlwaysNull
            var propertyValue2 = value2;
            var propertyValue3 = value3.ToString(CultureInfo.InvariantCulture);
            // ReSharper disable once ExpressionIsAlwaysNull
            var propertyValue4 = value4;

            var convertedValue1 = converter.Convert(propertyValue1, typeof(float));
            // ReSharper disable once ExpressionIsAlwaysNull
            var convertedValue2 = converter.Convert(propertyValue2, typeof(float?));

            Assert.AreEqual(value1, convertedValue1);
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.AreEqual(value2, convertedValue2);

            var convertedValue3 = converter.Convert(propertyValue3, typeof(double));
            // ReSharper disable once ExpressionIsAlwaysNull
            var convertedValue4 = converter.Convert(propertyValue4, typeof(double?));

            Assert.AreEqual(value3, convertedValue3);
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.AreEqual(value4, convertedValue4);
        }
    }
}
