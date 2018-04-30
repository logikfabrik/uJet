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
        [InlineData("u", "u")]
        [InlineData("U", "u")]
        [InlineData("umbraco7", "umbraco7")]
        [InlineData("UMBRACO7", "uMBRACO7")]
        [InlineData("umbraco;:_", "umbraco")]
        [InlineData(null, null)]
        public void CanGetAlias(string s, string expected)
        {
            s.Alias().ShouldBe(expected);
        }
    }
}
