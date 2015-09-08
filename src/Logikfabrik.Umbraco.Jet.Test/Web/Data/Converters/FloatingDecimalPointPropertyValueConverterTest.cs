// <copyright file="FloatingDecimalPointPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System.Globalization;
    using Jet.Web.Data.Converters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class FloatingDecimalPointPropertyValueConverterTest
    {
        [TestMethod]
        public void CanConvertValue()
        {
            var converter = new FloatingDecimalPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(decimal)));
            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(decimal?)));

            const decimal value1 = 1.1m;
            decimal? value2 = null;
            var propertyValue1 = value1.ToString(CultureInfo.InvariantCulture);
            // ReSharper disable once ExpressionIsAlwaysNull
            var propertyValue2 = value2;

            var convertedValue1 = converter.Convert(propertyValue1, typeof(decimal));
            // ReSharper disable once ExpressionIsAlwaysNull
            var convertedValue2 = converter.Convert(propertyValue2, typeof(decimal?));

            Assert.AreEqual(value1, convertedValue1);
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.AreEqual(value2, convertedValue2);
        }
    }
}
