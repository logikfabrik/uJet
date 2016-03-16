// <copyright file="TypeExtensionsTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
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
            Assert.IsTrue(typeof(Models.DocumentType).IsModelType<DocumentTypeAttribute>());
        }

        [TestMethod]
        public void IsNotDocumentType()
        {
            Assert.IsFalse(typeof(object).IsModelType<DocumentTypeAttribute>());
        }

        [TestMethod]
        public void IsMediaType()
        {
            Assert.IsTrue(typeof(Models.MediaType).IsModelType<MediaTypeAttribute>());
        }

        [TestMethod]
        public void IsNotMediaType()
        {
            Assert.IsFalse(typeof(object).IsModelType<MediaTypeAttribute>());
        }

        [TestMethod]
        public void IsDataType()
        {
            Assert.IsTrue(typeof(Models.DataType).IsModelType<DataTypeAttribute>());
        }

        [TestMethod]
        public void IsNotDataType()
        {
            Assert.IsFalse(typeof(object).IsModelType<DataTypeAttribute>());
        }

        [TestMethod]
        public void IsMemberType()
        {
            Assert.IsTrue(typeof(Models.MemberType).IsModelType<MemberTypeAttribute>());
        }

        [TestMethod]
        public void IsNotMemberType()
        {
            Assert.IsFalse(typeof(object).IsModelType<MemberTypeAttribute>());
        }
    }
}