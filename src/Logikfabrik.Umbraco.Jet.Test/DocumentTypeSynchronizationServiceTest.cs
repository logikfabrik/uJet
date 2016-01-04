// <copyright file="DocumentTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DocumentTypeSynchronizationServiceTest : TestBase
    {
        [TestMethod]
        public void CanCreateDocumentTypeWithAndWithoutId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));
            var documentTypeWithoutId = new DocumentType(typeof(DocumentTypeWithoutId));

            var contentTypeWithIdMock = new Mock<IContentType>();
            var contentTypeWithoutIdMock = new Mock<IContentType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(new[] { documentTypeWithId, documentTypeWithoutId });

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeServiceMock.Setup(m => m.GetContentType(documentTypeWithId.Alias)).Returns(contentTypeWithIdMock.Object);
            contentTypeServiceMock.Setup(m => m.GetContentType(documentTypeWithoutId.Alias)).Returns(contentTypeWithoutIdMock.Object);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(It.IsAny<DocumentType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateDocumentTypeWithId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));

            var contentTypeMock = new Mock<IContentType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(new[] { documentTypeWithId });

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeServiceMock.Setup(m => m.GetContentType(documentTypeWithId.Alias)).Returns(contentTypeMock.Object);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(documentTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateDocumentTypeWithoutId()
        {
            var documentTypeWithoutId = new DocumentType(typeof(DocumentTypeWithoutId));

            var contentTypeMock = new Mock<IContentType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(new[] { documentTypeWithoutId });

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeServiceMock.Setup(m => m.GetContentType(documentTypeWithoutId.Alias)).Returns(contentTypeMock.Object);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(documentTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateDocumentTypeWithId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(new[] { documentTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType<DocumentType, DocumentTypeAttribute>(documentTypeWithId, It.IsAny<IContentTypeBase[]>())).Returns(contentTypeMock.Object);

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetContentType(documentTypeWithId.Alias)).Returns(contentTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(documentTypeWithId.Id.Value)).Returns(contentTypeMock.Object.Id);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(contentTypeMock.Object, documentTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForDocumentTypeWithId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(new[] { documentTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType<DocumentType, DocumentTypeAttribute>(documentTypeWithId, It.IsAny<IContentTypeBase[]>())).Returns(contentTypeMock.Object);

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetContentType(documentTypeWithId.Alias)).Returns(contentTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(documentTypeWithId.Id.Value)).Returns(contentTypeMock.Object.Id);

            var documentTypeSynchronizationService = new DocumentTypeSynchronizationService(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object,
                new Mock<IFileService>().Object);

            documentTypeSynchronizationService.Synchronize();

            contentTypeMock.VerifySet(m => m.Name = documentTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateDocumentTypeWithoutId()
        {
            var documentTypeWithoutId = new DocumentType(typeof(DocumentTypeWithoutId));

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(new[] { documentTypeWithoutId });
            typeResolverMock.Setup(m => m.ResolveType<DocumentType, DocumentTypeAttribute>(documentTypeWithoutId, It.IsAny<IContentTypeBase[]>())).Returns(contentTypeMock.Object);

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetContentType(documentTypeWithoutId.Alias)).Returns(contentTypeMock.Object);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object,
                new Mock<IFileService>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Synchronize();

            documentTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(contentTypeMock.Object, documentTypeWithoutId), Times.Once);
        }

        [DocumentType(
            "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054",
            "DocumentTypeWithId",
            DefaultTemplate = "DefaultTemplateForDocumentTypeWithId")]
        protected class DocumentTypeWithId
        {
        }

        [DocumentType(
            "DocumentTypeWithoutId",
            DefaultTemplate = "DefaultTemplateForDocumentTypeWithoutId")]
        protected class DocumentTypeWithoutId
        {
        }
    }
}
