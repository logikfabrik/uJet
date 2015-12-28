// <copyright file="TypeServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

using Logikfabrik.Umbraco.Jet.Extensions;

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

        [TestMethod]
        public void CanGetCompositionForDocumentTypeWithoutBaseClass()
        {
            var typeServiceMock = GetTypeServiceMock(typeof(DocumentTypeA));

            var composition = typeServiceMock.Object.GetComposition(typeof(DocumentTypeA), TypeExtensions.IsDocumentType);

            Assert.AreEqual(1, composition.Count);
            Assert.AreEqual(2, composition[typeof(DocumentTypeA)].Count());
        }

        [TestMethod]
        public void CanGetCompositionForDocumentTypeWithPublicBaseClass()
        {
            var typeServiceMock = GetTypeServiceMock(typeof(DocumentTypeB));

            var composition = typeServiceMock.Object.GetComposition(typeof(DocumentTypeB), TypeExtensions.IsDocumentType);

            Assert.AreEqual(1, composition.Count);
            Assert.AreEqual(3, composition[typeof(DocumentTypeB)].Count());
        }

        [TestMethod]
        public void CanGetCompositionForDocumentTypeWithAbstractBaseClass()
        {
            var typeServiceMock = GetTypeServiceMock(typeof(DocumentTypeC));

            var composition = typeServiceMock.Object.GetComposition(typeof(DocumentTypeC), TypeExtensions.IsDocumentType);

            Assert.AreEqual(1, composition.Count);
            Assert.AreEqual(3, composition[typeof(DocumentTypeC)].Count());
        }

        [TestMethod]
        public void CanGetCompositionForDocumentTypeWithPublicDocumentTypeBaseClass()
        {
            var typeServiceMock = GetTypeServiceMock(typeof(DocumentTypeE));

            var composition = typeServiceMock.Object.GetComposition(typeof(DocumentTypeE), TypeExtensions.IsDocumentType);

            Assert.AreEqual(2, composition.Count);
            Assert.AreEqual(2, composition[typeof(DocumentTypeD)].Count());
            Assert.AreEqual(2, composition[typeof(DocumentTypeE)].Count());
        }

        /// <summary>
        /// The <see cref="DocumentTypeA" /> class.
        /// </summary>
        [DocumentType("DocumentTypeA")]
        public class DocumentTypeA
        {
            public string Property1 { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentTypeBBaseClass" /> class.
        /// </summary>
        public class DocumentTypeBBaseClass
        {
            public string Property1 { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentTypeB" /> class.
        /// </summary>
        [DocumentType("DocumentTypeB")]
        public class DocumentTypeB : DocumentTypeBBaseClass
        {
            public string Property2 { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentTypeCBaseClass" /> class.
        /// </summary>
        public abstract class DocumentTypeCBaseClass
        {
            public string Property1 { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentTypeC" /> class.
        /// </summary>
        [DocumentType("DocumentTypeC")]
        public class DocumentTypeC : DocumentTypeCBaseClass
        {
            public string Property2 { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentTypeD" /> class.
        /// </summary>
        [DocumentType("DocumentTypeD")]
        public class DocumentTypeD
        {
            public string Property1 { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentTypeEBaseClass" /> class.
        /// </summary>
        public abstract class DocumentTypeEBaseClass : DocumentTypeD
        {
            public string Property2 { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentTypeE" /> class.
        /// </summary>
        [DocumentType("DocumentTypeE")]
        public class DocumentTypeE : DocumentTypeEBaseClass
        {
            public string Property3 { get; set; }
        }
    }
}
