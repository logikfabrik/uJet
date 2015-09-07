//----------------------------------------------------------------------------------
// <copyright file="DocumentTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using Data;
    using Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    [TestClass]
    public class DocumentTypeSynchronizationServiceTest
    {
        private const string IdForDocumentTypeWithId = "D7B9B7F5-B2ED-4C2F-8239-9A2F50D14054";
        private const string NameForDocumentTypeWithId = "DocumentTypeWithId";
        private const string NameForDocumentTypeWithoutId = "DocumentTypeWithoutId";
        private const string DefaultTemplateForDocumentTypeWithId = "DefaultTemplateForDocumentTypeWithId";
        private const string DefaultTemplateForDocumentTypeWithoutId = "DefaultTemplateForDocumentTypeWithoutId";
        
        [TestMethod]
        public void CanCreateDocumentTypeWithAndWithoutId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var fileService = new Mock<IFileService>();
            var typeService = new Mock<ITypeService>();
            var withIdContentType = new Mock<IContentType>();
            var withoutIdContentType = new Mock<IContentType>();

            withIdContentType.SetupAllProperties();
            withoutIdContentType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeService.Setup(m => m.Save(It.IsAny<IContentType>(), It.IsAny<int>()))
                .Callback<IContentType, int>((ct, userId) =>
                {
                    switch (ct.Name)
                    {
                        case NameForDocumentTypeWithId:
                            withIdContentType.Object.Name = ct.Name;
                            break;

                        case NameForDocumentTypeWithoutId:
                            withoutIdContentType.Object.Name = ct.Name;
                            break;
                    }
                });
            contentTypeService.Setup(m => m.GetContentType(It.Is<string>(v => v == NameForDocumentTypeWithId.Alias())))
                .Returns(withIdContentType.Object);
            contentTypeService.Setup(m => m.GetContentType(It.Is<string>(v => v == NameForDocumentTypeWithoutId.Alias())))
                .Returns(withoutIdContentType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithId), typeof(DocumentTypeWithoutId) });

            var documentTypeSynchronizationService = new DocumentTypeSynchronizationService(
                contentTypeService.Object,
                contentTypeRepository.Object, 
                typeService.Object,
                fileService.Object);

            documentTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDocumentTypeWithId, withIdContentType.Object.Name);
            Assert.AreEqual(NameForDocumentTypeWithoutId, withoutIdContentType.Object.Name);
        }

        [TestMethod]
        public void CanCreateDocumentTypeWithId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var fileService = new Mock<IFileService>();
            var typeService = new Mock<ITypeService>();
            var withIdContentType = new Mock<IContentType>();

            withIdContentType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeService.Setup(m => m.Save(It.IsAny<IContentType>(), It.IsAny<int>()))
                .Callback<IContentType, int>((ct, userId) =>
                {
                    withIdContentType.Object.Name = ct.Name;
                });
            contentTypeService.Setup(m => m.GetContentType(It.Is<string>(v => v == NameForDocumentTypeWithId.Alias())))
                .Returns(withIdContentType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithId) });

            var documentTypeSynchronizationService = new DocumentTypeSynchronizationService(contentTypeService.Object,
                contentTypeRepository.Object, typeService.Object, fileService.Object);

            documentTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDocumentTypeWithId, withIdContentType.Object.Name);
        }

        [TestMethod]
        public void CanCreateDocumentTypeWithoutId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var fileService = new Mock<IFileService>();
            var typeService = new Mock<ITypeService>();
            var withoutIdContentType = new Mock<IContentType>();

            withoutIdContentType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllContentTypes()).Returns(new IContentType[] { });
            contentTypeService.Setup(m => m.Save(It.IsAny<IContentType>(), It.IsAny<int>()))
                .Callback<IContentType, int>((ct, userId) =>
                {
                    withoutIdContentType.Object.Name = ct.Name;
                });
            contentTypeService.Setup(m => m.GetContentType(It.Is<string>(v => v == NameForDocumentTypeWithoutId.Alias())))
                .Returns(withoutIdContentType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithoutId) });

            var documentTypeSynchronizationService = new DocumentTypeSynchronizationService(contentTypeService.Object,
                contentTypeRepository.Object, typeService.Object, fileService.Object);

            documentTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDocumentTypeWithoutId, withoutIdContentType.Object.Name);
        }

        [TestMethod]
        public void CanUpdateNameForDocumentTypeWithId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var fileService = new Mock<IFileService>();
            var typeService = new Mock<ITypeService>();
            var withIdDbContentType = new Mock<IContentType>();
            var withIdContentType = new Mock<IContentType>();

            withIdDbContentType.SetupAllProperties();
            withIdDbContentType.Object.Id = 1234;
            withIdDbContentType.Object.Name = "DbDocumentTypeWithId";

            withIdContentType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllContentTypes()).Returns(new[] { withIdDbContentType.Object });
            contentTypeService.Setup(m => m.Save(It.IsAny<IContentType>(), It.IsAny<int>()))
                .Callback<IContentType, int>((ct, userId) =>
                {
                    withIdContentType.Object.Name = ct.Name;
                });
            contentTypeService.Setup(m => m.GetContentType(It.Is<string>(v => v == NameForDocumentTypeWithId.Alias())))
                .Returns(withIdContentType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.DocumentTypes).Returns(new[] { typeof(DocumentTypeWithId) });

            var documentTypeSynchronizationService = new DocumentTypeSynchronizationService(contentTypeService.Object,
                contentTypeRepository.Object, typeService.Object, fileService.Object);

            documentTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDocumentTypeWithId, withIdContentType.Object.Name);
        }

        [DocumentType(IdForDocumentTypeWithId, NameForDocumentTypeWithId, DefaultTemplate = DefaultTemplateForDocumentTypeWithId)]
        public class DocumentTypeWithId
        {
        }

        [DocumentType(NameForDocumentTypeWithoutId, DefaultTemplate = DefaultTemplateForDocumentTypeWithoutId)]
        public class DocumentTypeWithoutId
        {
        }

        public class DocumentTypeWithPropertyWithId
        {
            // TODO: Test.
        }

        public class DocumentTypeWithPropertyWithoutId
        {
            // TODO: Test.
        }
    }
}
