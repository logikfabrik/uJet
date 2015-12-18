// <copyright file="StringExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Extensions
{
    using Jet.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="TypeExtensionsTest" /> class.
    /// </summary>
    [TestClass]
    public class StringExtensionsTest : TestBase
    {
        /// <summary>
        /// Test to get alias for 1 letter lowercase word.
        /// </summary>
        [TestMethod]
        public void CanGetAliasFor1LetterLowercaseWord()
        {
            Assert.AreEqual("u", "u".Alias());
        }

        /// <summary>
        /// Test to get alias for 1 letter uppercase word.
        /// </summary>
        [TestMethod]
        public void CanGetAliasFor1LetterUppercaseWord()
        {
            Assert.AreEqual("u", "U".Alias());
        }

        /// <summary>
        /// Test to get get alias for1 letter first letter not lowercase and first letter is not changed.
        /// </summary>
        [TestMethod]
        public void CanGetAliasFor1LetterFirstLetterNotLowercase()
        {
            Assert.AreEqual("u", "u".Alias(false));
        }

        /// <summary>
        /// Test to get get alias for1 letter first letter is uppercase and first letter is not changed.
        /// </summary>
        [TestMethod]
        public void CanGetAliasFor1LetterFirstLetterIsLowercase()
        {
            Assert.AreEqual("U", "U".Alias(false));
        }

        /// <summary>
        /// Test to get alias for word with valid characters.
        /// </summary>
        [TestMethod]
        public void CanGetAliasForWordWithValidCharacters()
        {
            Assert.AreEqual("abc123", "abc123".Alias());
        }

        /// <summary>
        /// Test to get alias for word with invalid characters.
        /// </summary>
        [TestMethod]
        public void CanGetAliasForWordWithInvalidCharacters()
        {
            Assert.AreEqual("abc", "abc;:_".Alias());
        }
    }
}
