// <copyright file="TypeExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Extensions
{
    using Jet.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypeExtensionsTest : TestBase
    {
        [TestMethod]
        public void IsDocumentType()
        {
            Assert.IsTrue(typeof(Models.DocumentType).IsDocumentType());
        }

        [TestMethod]
        public void IsNotDocumentType()
        {
            Assert.IsFalse(typeof(object).IsDocumentType());
        }

        [TestMethod]
        public void IsMediaType()
        {
            Assert.IsTrue(typeof(Models.MediaType).IsMediaType());
        }

        [TestMethod]
        public void IsNotMediaType()
        {
            Assert.IsFalse(typeof(object).IsMediaType());
        }

        [TestMethod]
        public void IsDataType()
        {
            Assert.IsTrue(typeof(Models.DataType).IsDataType());
        }

        [TestMethod]
        public void IsNotDataType()
        {
            Assert.IsFalse(typeof(object).IsDataType());
        }

        [TestMethod]
        public void IsMemberType()
        {
            Assert.IsTrue(typeof(Models.MemberType).IsMemberType());
        }

        [TestMethod]
        public void IsNotMemberType()
        {
            Assert.IsFalse(typeof(object).IsMemberType());
        }
    }
}