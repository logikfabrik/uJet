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
using System.Web;

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    [TestClass]
    public class HtmlStringPropertyValueConverterTest
    {
        [TestMethod]
        public void CanConvertValue()
        {
            var converter = new HtmlStringPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(HtmlString), typeof(string)));

            const string value1 = "Test";
            string value2 = null;
            var propertyValue1 = new HtmlString(value1);
            // ReSharper disable once ExpressionIsAlwaysNull
            var propertyValue2 = value2;

            var convertedValue1 = converter.Convert(propertyValue1, typeof(string));
            // ReSharper disable once ExpressionIsAlwaysNull
            var convertedValue2 = converter.Convert(propertyValue2, typeof(string));

            Assert.AreEqual(value1, convertedValue1);
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.AreEqual(value2, convertedValue2);
        }
    }
}
