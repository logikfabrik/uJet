// <copyright file="StringExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Extensions
{
    using Jet.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringExtensionsTest : TestBase
    {
        [TestMethod]
        public void CanGetAliasFor1LetterLowercaseWord()
        {
            Assert.AreEqual("u", "u".Alias());
        }

        [TestMethod]
        public void CanGetAliasFor1LetterUppercaseWord()
        {
            Assert.AreEqual("u", "U".Alias());
        }

        [TestMethod]
        public void CanGetAliasForWordWithValidCharacters()
        {
            Assert.AreEqual("abc123", "abc123".Alias());
        }

        [TestMethod]
        public void CanGetAliasForWordWithInvalidCharacters()
        {
            Assert.AreEqual("abc", "abc;:_".Alias());
        }

        [TestMethod]
        public void CanGetAliasForNullWord()
        {
            Assert.IsNull(((string)null).Alias());
        }
    }
}
