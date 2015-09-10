// <copyright file="FloatingBinaryPointPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System.Globalization;
    using Jet.Web.Data.Converters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="FloatingBinaryPointPropertyValueConverterTest" /> class.
    /// </summary>
    [TestClass]
    public class FloatingBinaryPointPropertyValueConverterTest
    {
        /// <summary>
        /// Test for supported type <see cref="float" />.
        /// </summary>
        [TestMethod]
        public void CanConvertStringToFloat()
        {
            const float value = 1.1f;

            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(value, converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(float)));
        }

        /// <summary>
        /// Test for supported type <see cref="float" />.
        /// </summary>
        [TestMethod]
        public void CanConvertStringToNullableFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(null, converter.Convert(null, typeof(float?)));
        }

        /// <summary>
        /// Test for supported type <see cref="double" />.
        /// </summary>
        [TestMethod]
        public void CanConvertStringToDouble()
        {
            const double value = 1.1;

            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(value, converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(double)));
        }

        /// <summary>
        /// Test for supported type <see cref="double" />.
        /// </summary>
        [TestMethod]
        public void CanConvertStringToNullableDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.AreEqual(null, converter.Convert(null, typeof(double?)));
        }

        /// <summary>
        /// Test for supported type <see cref="float" />.
        /// </summary>
        [TestMethod]
        public void CanConvertValueReturnsTrueForFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(float)));
        }

        /// <summary>
        /// Test for supported type <see cref="float" />.
        /// </summary>
        [TestMethod]
        public void CanConvertValueReturnsTrueForNullableFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(float?)));
        }

        /// <summary>
        /// Test for supported type <see cref="double" />.
        /// </summary>
        [TestMethod]
        public void CanConvertValueReturnsTrueForDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(double)));
        }

        /// <summary>
        /// Test for supported type <see cref="double" />.
        /// </summary>
        [TestMethod]
        public void CanConvertValueReturnsTrueForNullableDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(string), typeof(double?)));
        }
    }
}
