// <copyright file="FloatingBinaryPointPropertyValueConverterTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data.Converters
{
    using System.Globalization;
    using Jet.Web.Data.Converters;
    using Shouldly;
    using Xunit;

    public class FloatingBinaryPointPropertyValueConverterTest
    {
        [Fact]
        public void CanConvertStringToFloat()
        {
            const float value = 1.1f;

            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(float)).ShouldBe(value);
        }

        [Fact]
        public void CanConvertStringToNullableFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.Convert(null, typeof(float?)).ShouldBe(null);
        }

        [Fact]
        public void CanConvertStringToDouble()
        {
            const double value = 1.1;

            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.Convert(value.ToString(CultureInfo.InvariantCulture), typeof(double)).ShouldBe(value);
        }

        [Fact]
        public void CanConvertStringToNullableDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.Convert(null, typeof(double?)).ShouldBe(null);
        }

        [Fact]
        public void CanConvertValueReturnsTrueForFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.CanConvertValue(null, typeof(string), typeof(float)).ShouldBeTrue();
        }

        [Fact]
        public void CanConvertValueReturnsTrueForNullableFloat()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.CanConvertValue(null, typeof(string), typeof(float?)).ShouldBeTrue();
        }

        [Fact]
        public void CanConvertValueReturnsTrueForDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.CanConvertValue(null, typeof(string), typeof(double)).ShouldBeTrue();
        }

        [Fact]
        public void CanConvertValueReturnsTrueForNullableDouble()
        {
            var converter = new FloatingBinaryPointPropertyValueConverter();

            converter.CanConvertValue(null, typeof(string), typeof(double?)).ShouldBeTrue();
        }
    }
}
