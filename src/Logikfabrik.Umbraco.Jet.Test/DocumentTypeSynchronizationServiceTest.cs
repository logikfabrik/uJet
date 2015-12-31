// <copyright file="DocumentTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using Data;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The <see cref="DocumentTypeSynchronizationServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class DocumentTypeSynchronizationServiceTest
    {
        private const string IdForDocumentTypeWithId = "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054";
        private const string NameForDocumentTypeWithId = "DocumentTypeWithId";
        private const string NameForDocumentTypeWithoutId = "DocumentTypeWithoutId";
        private const string DefaultTemplateForDocumentTypeWithId = "DefaultTemplateForDocumentTypeWithId";
        private const string DefaultTemplateForDocumentTypeWithoutId = "DefaultTemplateForDocumentTypeWithoutId";

        /// <summary>
        /// Test to create document type with and without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateDocumentTypeWithAndWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithId), typeof(DocumentTypeWithoutId) });

            var contentTypeWithIdMock = new Mock<IContentType>();

            var contentTypeWithoutIdMock = new Mock<IContentType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeServiceMock.Setup(m => m.GetContentType(NameForDocumentTypeWithId.Alias())).Returns(contentTypeWithIdMock.Object);
            contentTypeServiceMock.Setup(m => m.GetContentType(NameForDocumentTypeWithoutId.Alias())).Returns(contentTypeWithoutIdMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock
                .Verify(m => m.CreateContentType(It.IsAny<DocumentType>()), Times.Exactly(2));
        }

        /// <summary>
        /// Test to create document type with ID.
        /// </summary>
        [TestMethod]
        public void CanCreateDocumentTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithId) });

            var contentTypeMock = new Mock<IContentType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeServiceMock.Setup(m => m.GetContentType(NameForDocumentTypeWithId.Alias())).Returns(contentTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock
                .Verify(m => m.CreateContentType(It.IsAny<DocumentType>()), Times.Once);
        }

        /// <summary>
        /// Test to create document type without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateDocumentTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithoutId) });

            var contentTypeMock = new Mock<IContentType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeServiceMock.Setup(m => m.GetContentType(NameForDocumentTypeWithoutId.Alias())).Returns(contentTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock
                .Verify(m => m.CreateContentType(It.IsAny<DocumentType>()), Times.Once);
        }

        /// <summary>
        /// Test to update document type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateDocumentTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithId) });

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetContentType(NameForDocumentTypeWithId.Alias())).Returns(contentTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(Guid.Parse(IdForDocumentTypeWithId))).Returns(contentTypeMock.Object.Id);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(contentTypeMock.Object, It.IsAny<DocumentType>()), Times.Once);
        }

        /// <summary>
        /// Test to update name for document type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateNameForDocumentTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithId) });

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetContentType(NameForDocumentTypeWithId.Alias())).Returns(contentTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(Guid.Parse(IdForDocumentTypeWithId))).Returns(contentTypeMock.Object.Id);

            var documentTypeSynchronizationService = new DocumentTypeSynchronizationService(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object,
                new Mock<IFileService>().Object);

            documentTypeSynchronizationService.Synchronize();

            contentTypeMock.VerifySet(m => m.Name = NameForDocumentTypeWithId, Times.Once);
        }

        /// <summary>
        /// Test to update document type without ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateDocumentTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithoutId) });

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.Setup(m => m.Alias).Returns(NameForDocumentTypeWithoutId.Alias());

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetContentType(NameForDocumentTypeWithoutId.Alias())).Returns(contentTypeMock.Object);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                new Mock<IContentTypeRepository>().Object,
                typeServiceMock.Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(contentTypeMock.Object, It.IsAny<DocumentType>()), Times.Once);
        }

        /// <summary>
        /// The <see cref="DocumentTypeWithId" /> class.
        /// </summary>
        [DocumentType(
            IdForDocumentTypeWithId,
            NameForDocumentTypeWithId,
            DefaultTemplate = DefaultTemplateForDocumentTypeWithId)]
        protected class DocumentTypeWithId
        {
        }

        /// <summary>
        /// The <see cref="DocumentTypeWithoutId" /> class.
        /// </summary>
        [DocumentType(
            NameForDocumentTypeWithoutId,
            DefaultTemplate = DefaultTemplateForDocumentTypeWithoutId)]
        protected class DocumentTypeWithoutId
        {
        }
    }
}
