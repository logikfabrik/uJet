// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    /// <summary>
    /// The <see cref="TypeServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class TypeServiceTest : TestBase
    {
        /// <summary>
        /// Test to get document types.
        /// </summary>
        [TestMethod]
        public void CanGetDocumentTypes()
        {
            var type = DocumentTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.AreEqual(1, documentTypes.Count());
        }

        /// <summary>
        /// Test to get abstract document types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractDocumentTypes()
        {
            var type = DocumentTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var documentTypes = typeServiceMock.Object.DocumentTypes;

            Assert.IsFalse(documentTypes.Any());
        }

        /// <summary>
        /// Test to get document types without public default constructor.
        /// </summary>
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

        /// <summary>
        /// Test to get data types.
        /// </summary>
        [TestMethod]
        public void CanGetDataTypes()
        {
            var type = DataTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.AreEqual(1, dataTypes.Count());
        }

        /// <summary>
        /// Test to get abstract data types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractDataTypes()
        {
            var type = DataTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var dataTypes = typeServiceMock.Object.DataTypes;

            Assert.IsFalse(dataTypes.Any());
        }

        /// <summary>
        /// Test to get data types without public default constructor.
        /// </summary>
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

        /// <summary>
        /// Test to get media types.
        /// </summary>
        [TestMethod]
        public void CanGetMediaTypes()
        {
            var type = MediaTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.AreEqual(1, mediaTypes.Count());
        }

        /// <summary>
        /// Test to get abstract media types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractMediaTypes()
        {
            var type = MediaTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var mediaTypes = typeServiceMock.Object.MediaTypes;

            Assert.IsFalse(mediaTypes.Any());
        }

        /// <summary>
        /// Test to get media types without public default constructor.
        /// </summary>
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

        /// <summary>
        /// Test to get member types.
        /// </summary>
        [TestMethod]
        public void CanGetMemberTypes()
        {
            var type = MemberTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.AreEqual(1, memberTypes.Count());
        }

        /// <summary>
        /// Test to get abstract member types.
        /// </summary>
        [TestMethod]
        public void CannotGetAbstractMemberTypes()
        {
            var type = MemberTypeUtility.GetTypeBuilder(TypeAttributes.Abstract).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var memberTypes = typeServiceMock.Object.MemberTypes;

            Assert.IsFalse(memberTypes.Any());
        }

        /// <summary>
        /// Test to get member types without public default constructor.
        /// </summary>
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
