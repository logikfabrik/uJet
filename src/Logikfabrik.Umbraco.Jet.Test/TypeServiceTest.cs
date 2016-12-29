// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
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

            var typeService = GetTypeService(type);

            var documentTypes = typeService.DocumentTypes;

            Assert.AreEqual(1, documentTypes.Count);
        }

        [TestMethod]
        public void CannotGetAbstractDocumentTypes()
        {
            var type = DocumentTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeService = GetTypeService(type);

            var documentTypes = typeService.DocumentTypes;

            Assert.IsFalse(documentTypes.Any());
        }

        [TestMethod]
        public void CannotGetDocumentTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = DataTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeService = GetTypeService(type);

            var documentTypes = typeService.DocumentTypes;

            Assert.IsFalse(documentTypes.Any());
        }

        [TestMethod]
        public void CanGetDataTypes()
        {
            var type = DataTypeUtility.GetTypeBuilder().CreateType();

            var typeService = GetTypeService(type);

            var dataTypes = typeService.DataTypes;

            Assert.AreEqual(1, dataTypes.Count);
        }

        [TestMethod]
        public void CannotGetAbstractDataTypes()
        {
            var type = DataTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeService = GetTypeService(type);

            var dataTypes = typeService.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        [TestMethod]
        public void CannotGetDataTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = DataTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeService = GetTypeService(type);

            var dataTypes = typeService.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        [TestMethod]
        public void CanGetMediaTypes()
        {
            var type = MediaTypeUtility.GetTypeBuilder().CreateType();

            var typeService = GetTypeService(type);

            var mediaTypes = typeService.MediaTypes;

            Assert.AreEqual(1, mediaTypes.Count);
        }

        [TestMethod]
        public void CannotGetAbstractMediaTypes()
        {
            var type = MediaTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeService = GetTypeService(type);

            var mediaTypes = typeService.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        [TestMethod]
        public void CannotGetMediaTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = MediaTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeService = GetTypeService(type);

            var mediaTypes = typeService.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        [TestMethod]
        public void CanGetMemberTypes()
        {
            var type = MemberTypeUtility.GetTypeBuilder().CreateType();

            var typeService = GetTypeService(type);

            var memberTypes = typeService.MemberTypes;

            Assert.AreEqual(1, memberTypes.Count);
        }

        [TestMethod]
        public void CannotGetAbstractMemberTypes()
        {
            var type = MemberTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeService = GetTypeService(type);

            var memberTypes = typeService.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }

        [TestMethod]
        public void CannotGetMemberTypesWithoutPublicDefaultConstructor()
        {
            var typeBuilder = MemberTypeUtility.GetTypeBuilder();

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            var type = typeBuilder.CreateType();

            var typeService = GetTypeService(type);

            var memberTypes = typeService.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }
    }
}