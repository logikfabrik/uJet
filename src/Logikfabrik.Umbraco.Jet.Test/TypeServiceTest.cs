// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    [TestClass]
    public class TypeServiceTest : TestBase
    {
        [TestMethod]
        public void CanGetDocumentTypes()
        {
            var type = DocumentTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.AreEqual(1, documentTypes.Count());
        }

        [TestMethod]
        public void CannotGetAbstractDocumentTypes()
        {
            var type = DocumentTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.IsFalse(documentTypes.Any());
        }

        [TestMethod]
        public void CannotGetDocumentTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = DataTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.IsFalse(documentTypes.Any());
        }

        [TestMethod]
        public void CanGetDataTypes()
        {
            var type = DataTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.AreEqual(1, dataTypes.Count());
        }

        [TestMethod]
        public void CannotGetAbstractDataTypes()
        {
            var type = DataTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        [TestMethod]
        public void CannotGetDataTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = DataTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        [TestMethod]
        public void CanGetMediaTypes()
        {
            var type = MediaTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.AreEqual(1, mediaTypes.Count());
        }

        [TestMethod]
        public void CannotGetAbstractMediaTypes()
        {
            var type = MediaTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        [TestMethod]
        public void CannotGetMediaTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = MediaTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        [TestMethod]
        public void CanGetMemberTypes()
        {
            var type = MemberTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.AreEqual(1, memberTypes.Count());
        }

        [TestMethod]
        public void CannotGetAbstractMemberTypes()
        {
            var type = MemberTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }

        [TestMethod]
        public void CannotGetMemberTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = MemberTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }
    }
}