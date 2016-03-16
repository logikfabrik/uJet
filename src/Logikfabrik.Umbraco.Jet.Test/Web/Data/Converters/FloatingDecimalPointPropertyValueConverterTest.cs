// <copyright file="FloatingDecimalPointPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
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
        public void CanConvertStringToDecimal()
        {
            const decimal value = 1.1m;

            var converter = new FloatingDecimalPointPropertyValueConverter();

            Assert.AreEqual(value, converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(decimal)));
        }

        [TestMethod]
        public void CanConvertStringToNullableDecimal()
        {
            var converter = new FloatingDecimalPointPropertyValueConverter();

            Assert.AreEqual(null, converter.Convert(null, typeof(decimal?)));
        }

        [TestMethod]
        public void CanConvertValueReturnsTrueForDecimal()
        {
            var converter = new FloatingDecimalPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(decimal)));
        }

        [TestMethod]
        public void CanConvertValueReturnsTrueForNullableDecimal()
        {
            var converter = new FloatingDecimalPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(decimal?)));
        }
    }
}
