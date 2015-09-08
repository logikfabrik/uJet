// <copyright file="FloatingBinaryPointPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System.Globalization;
    using Jet.Web.Data.Converters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
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
