// <copyright file="HtmlStringPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System.Web;
    using Jet.Web.Data.Converters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
