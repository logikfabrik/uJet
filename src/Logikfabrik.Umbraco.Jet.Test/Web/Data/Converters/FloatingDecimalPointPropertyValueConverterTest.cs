// <copyright file="FloatingDecimalPointPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System.Globalization;
    using Jet.Web.Data.Converters;
    using Shouldly;
    using Xunit;

    public class FloatingDecimalPointPropertyValueConverterTest
    {
        [Fact]
        public void CanConvertStringToDecimal()
        {
            const decimal value = 1.1m;

            var converter = new FloatingDecimalPointPropertyValueConverter();

            converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(decimal)).ShouldBe(value);
        }

        [Fact]
        public void CanConvertStringToNullableDecimal()
        {
            var converter = new FloatingDecimalPointPropertyValueConverter();

            converter.Convert(null, typeof(decimal?)).ShouldBe(null);
        }

        [Fact]
        public void CanConvertValueReturnsTrueForDecimal()
        {
            var converter = new FloatingDecimalPointPropertyValueConverter();

            converter.CanConvertValue(null, typeof(string), typeof(decimal)).ShouldBeTrue();
        }

        [Fact]
        public void CanConvertValueReturnsTrueForNullableDecimal()
        {
            var converter = new FloatingDecimalPointPropertyValueConverter();

            converter.CanConvertValue(null, typeof(string), typeof(decimal?)).ShouldBeTrue();
        }
    }
}
