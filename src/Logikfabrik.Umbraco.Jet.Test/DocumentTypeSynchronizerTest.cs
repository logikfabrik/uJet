// <copyright file="DocumentTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DocumentTypeSynchronizerTest : TestBase
    {
        [TestMethod]
        public void CanCreateDocumentTypeWithAndWithoutId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));
            var documentTypeWithoutId = new DocumentType(typeof(DocumentTypeWithoutId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(Array.AsReadOnly(new[] { documentTypeWithId, documentTypeWithoutId }));

            var contentTypes = new List<IContentType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(contentTypes);
            contentTypeServiceMock.Setup(m => m.Save(It.IsAny<IContentType>(), 0)).Callback((IContentType contentType, int userId) => { contentTypes.Add(contentType); });

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizer>(
                contentTypeServiceMock.Object,
                new Mock<IFileService>().Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Run();

            documentTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(It.IsAny<DocumentType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateDocumentTypeWithId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(Array.AsReadOnly(new[] { documentTypeWithId }));

            var contentTypes = new List<IContentType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(contentTypes);
            contentTypeServiceMock.Setup(m => m.Save(It.IsAny<IContentType>(), 0)).Callback((IContentType contentType, int userId) => { contentTypes.Add(contentType); });

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizer>(
                contentTypeServiceMock.Object,
                new Mock<IFileService>().Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Run();

            documentTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(documentTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateDocumentTypeWithoutId()
        {
            var documentTypeWithoutId = new DocumentType(typeof(DocumentTypeWithoutId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(Array.AsReadOnly(new[] { documentTypeWithoutId }));

            var contentTypes = new List<IContentType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(contentTypes);
            contentTypeServiceMock.Setup(m => m.Save(It.IsAny<IContentType>(), 0)).Callback((IContentType contentType, int userId) => { contentTypes.Add(contentType); });

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizer>(
                contentTypeServiceMock.Object,
                new Mock<IFileService>().Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Run();

            documentTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(documentTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateDocumentTypeWithId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(Array.AsReadOnly(new[] { documentTypeWithId }));

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(documentTypeWithId.Id.Value)).Returns(contentTypeMock.Object.Id);

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizer>(
                contentTypeServiceMock.Object,
                new Mock<IFileService>().Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Run();

            documentTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(contentTypeMock.Object, documentTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForDocumentTypeWithId()
        {
            var documentTypeWithId = new DocumentType(typeof(DocumentTypeWithId));

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(Array.AsReadOnly(new[] { documentTypeWithId }));

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(documentTypeWithId.Id.Value)).Returns(contentTypeMock.Object.Id);

            var documentTypeSynchronizationService = new DocumentTypeSynchronizer(
                contentTypeServiceMock.Object,
                new Mock<IFileService>().Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object);

            documentTypeSynchronizationService.Run();

            contentTypeMock.VerifySet(m => m.Name = documentTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateDocumentTypeWithoutId()
        {
            var documentTypeWithoutId = new DocumentType(typeof(DocumentTypeWithoutId));

            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.SetupAllProperties();
            contentTypeMock.Object.Alias = documentTypeWithoutId.Alias;

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DocumentTypes).Returns(Array.AsReadOnly(new[] { documentTypeWithoutId }));

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllContentTypes()).Returns(new[] { contentTypeMock.Object });

            var documentTypeSynchronizationServiceMock = new Mock<DocumentTypeSynchronizer>(
                contentTypeServiceMock.Object,
                new Mock<IFileService>().Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            documentTypeSynchronizationServiceMock.Object.Run();

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