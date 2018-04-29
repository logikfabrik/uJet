// <copyright file="HtmlStringPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System;
    using System.Web;
    using Jet.Web.Data.Converters;
    using Shouldly;
    using Xunit;

    public class HtmlStringPropertyValueConverterTest
    {
        [Fact]
        public void CanConvertNullHtmlStringToNullString()
        {
            var converter = new HtmlStringPropertyValueConverter();

            converter.Convert(new HtmlString(null), typeof(string)).ShouldBe(null);
        }

        [Fact]
        public void CanConvertEmptyHtmlStringToEmptyString()
        {
            var converter = new HtmlStringPropertyValueConverter();

            converter.Convert(new HtmlString(string.Empty), typeof(string)).ShouldBe(string.Empty);
        }

        [Fact]
        public void CanConvertHtmlStringToString()
        {
            var value = Guid.NewGuid().ToString();

            var converter = new HtmlStringPropertyValueConverter();

            converter.Convert(new HtmlString(value), typeof(string)).ShouldBe(value);
        }

        [Fact]
        public void CanConvertValueReturnsTrueForHtmlString()
        {
            var converter = new HtmlStringPropertyValueConverter();

            converter.CanConvertValue(null, typeof(HtmlString), typeof(string)).ShouldBeTrue();
        }
    }
}
