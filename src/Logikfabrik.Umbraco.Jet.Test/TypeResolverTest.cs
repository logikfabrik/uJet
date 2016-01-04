// <copyright file="TypeResolverTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Utilities;

    [TestClass]
    public class TypeResolverTest : TestBase
    {
        [TestMethod]
        public void CanGetDocumentTypes()
        {
            var type = DocumentTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var typeResolver = new TypeResolver(typeServiceMock.Object, new Mock<Jet.Data.ITypeRepository>().Object);

            var documentTypes = typeResolver.DocumentTypes;

            Assert.AreEqual(1, documentTypes.Count());
        }

        [TestMethod]
        public void CanGetMediaTypes()
        {
            var type = MediaTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var typeResolver = new TypeResolver(typeServiceMock.Object, new Mock<Jet.Data.ITypeRepository>().Object);

            var mediaTypes = typeResolver.MediaTypes;

            Assert.AreEqual(1, mediaTypes.Count());
        }

        [TestMethod]
        public void CanGetMemberTypes()
        {
            var type = MemberTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var typeResolver = new TypeResolver(typeServiceMock.Object, new Mock<Jet.Data.ITypeRepository>().Object);

            var memberTypes = typeResolver.MemberTypes;

            Assert.AreEqual(1, memberTypes.Count());
        }

        [TestMethod]
        public void CanGetDataTypes()
        {
            var type = DataTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var typeResolver = new TypeResolver(typeServiceMock.Object, new Mock<Jet.Data.ITypeRepository>().Object);

            var dataTypes = typeResolver.DataTypes;

            Assert.AreEqual(1, dataTypes.Count());
        }
    }
}
