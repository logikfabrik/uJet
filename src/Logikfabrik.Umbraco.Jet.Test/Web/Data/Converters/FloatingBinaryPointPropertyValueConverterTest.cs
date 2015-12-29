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
        public void CanConvertStringToFloat()
        {
            const float value = 1.1f;

            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(value, converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(float)));
        }

        [TestMethod]
        public void CanConvertStringToNullableFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(null, converter.Convert(null, typeof(float?)));
        }

        [TestMethod]
        public void CanConvertStringToDouble()
        {
            const double value = 1.1;

            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(value, converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(double)));
        }

        [TestMethod]
        public void CanConvertStringToNullableDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(null, converter.Convert(null, typeof(double?)));
        }

        [TestMethod]
        public void CanConvertValueReturnsTrueForFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(float)));
        }

        [TestMethod]
        public void CanConvertValueReturnsTrueForNullableFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(float?)));
        }

        [TestMethod]
        public void CanConvertValueReturnsTrueForDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(double)));
        }

        [TestMethod]
        public void CanConvertValueReturnsTrueForNullableDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(double?)));
        }
    }
}
