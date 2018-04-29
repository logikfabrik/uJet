// <copyright file="StringExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Extensions
{
    using AutoFixture.Xunit2;
    using Jet.Extensions;
    using Shouldly;
    using Xunit;

    public class StringExtensionsTest : TestBase
    {
        [Theory]
        [InlineAutoData("u", "u")]
        [InlineAutoData("U", "u")]
        [InlineAutoData("umbraco7", "umbraco7")]
        [InlineAutoData("UMBRACO7", "uMBRACO7")]
        [InlineAutoData("umbraco;:_", "umbraco")]
        [InlineAutoData(null, null)]
        public void CanGetAlias(string s, string expected)
        {
            s.Alias().ShouldBe(expected);
        }
    }
}
