// <copyright file="HtmlStringPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System;
    using System.Web;
    using Jet.Web.Data.Converters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="HtmlStringPropertyValueConverterTest" /> class.
    /// </summary>
    [TestClass]
    public class HtmlStringPropertyValueConverterTest
    {
        /// <summary>
        /// Test for supported type <see cref="string" />.
        /// </summary>
        [TestMethod]
        public void CanConvertNullHtmlStringToNullString()
        {
            var converter = new HtmlStringPropertyValueConverter();

            Assert.AreEqual(null, converter.Convert(new HtmlString(null), typeof(string)));
        }

        /// <summary>
        /// Test for supported type <see cref="string" />.
        /// </summary>
        [TestMethod]
        public void CanConvertEmptyHtmlStringToEmptyString()
        {
            var converter = new HtmlStringPropertyValueConverter();

            Assert.AreEqual(string.Empty, converter.Convert(new HtmlString(string.Empty), typeof(string)));
        }

        /// <summary>
        /// Test for supported type <see cref="string" />.
        /// </summary>
        [TestMethod]
        public void CanConvertHtmlStringToString()
        {
            var value = Guid.NewGuid().ToString();

            var converter = new HtmlStringPropertyValueConverter();

            Assert.AreEqual(value, converter.Convert(new HtmlString(value), typeof(string)));
        }

        /// <summary>
        /// Test for supported type <see cref="string" />.
        /// </summary>
        [TestMethod]
        public void CanConvertValueReturnsTrueForHtmlString()
        {
            var converter = new HtmlStringPropertyValueConverter();

            Assert.IsTrue(converter.CanConvertValue(null, typeof(HtmlString), typeof(string)));
        }
    }
}
