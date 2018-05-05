// <copyright file="DocumentTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using AutoFixture.Xunit2;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Data;
    using Moq;
    using Moq.AutoMock;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DocumentTypeSynchronizerTest
    {
        [Theory]
        [CustomAutoData]
        public void CanCreateModelWithoutId(DocumentType model)
        {
            var mocker = new AutoMocker();

            var documentTypeSynchronizer = mocker.CreateInstance<DocumentTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DocumentTypes)
                .Returns(new[] { model });

            var contentTypes = new List<IContentType>();

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllContentTypes())
                .Returns(contentTypes);

            contentTypeServiceMock
                .Setup(m => m.Save(It.Is<IContentType>(contentType => contentType.Id == 0), 0))
                .Callback((IContentType contentType, int userId) => { contentTypes.Add(contentType); })
                .Verifiable();

            documentTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [AutoData]
        public void CanCreateModelWithId(string typeName, Guid id, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, id, name).CreateType();

            var model = new DocumentType(modelType);

            var mocker = new AutoMocker();

            var documentTypeSynchronizer = mocker.CreateInstance<DocumentTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DocumentTypes)
                .Returns(new[] { model });

            var contentTypes = new List<IContentType>();

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllContentTypes())
                .Returns(contentTypes);

            contentTypeServiceMock
                .Setup(m => m.Save(It.Is<IContentType>(contentType => contentType.Id == 0), 0))
                .Callback((IContentType contentType, int userId) => { contentTypes.Add(contentType); })
                .Verifiable();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.SetContentTypeId(id, It.IsAny<int>())).Verifiable();

            documentTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithoutId(DocumentType model)
        {
            var mocker = new AutoMocker();

            var documentTypeSynchronizer = mocker.CreateInstance<DocumentTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DocumentTypes)
                .Returns(new[] { model });

            var contentTypeMock = mocker.GetMock<IContentType>();

            contentTypeMock.Setup(m => m.Alias).Returns(model.Alias);

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllContentTypes())
                .Returns(new[] { contentTypeMock.Object });

            contentTypeServiceMock.Setup(m => m.Save(contentTypeMock.Object, 0)).Verifiable();

            documentTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [AutoData]
        public void CanUpdateModelWithId(string typeName, Guid id, string name)
        {
            var modelType = new DocumentTypeModelTypeBuilder(typeName, id, name).CreateType();

            var model = new DocumentType(modelType);

            var mocker = new AutoMocker();

            var documentTypeSynchronizer = mocker.CreateInstance<DocumentTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DocumentTypes)
                .Returns(new[] { model });

            var contentTypeMock = mocker.GetMock<IContentType>();

            contentTypeMock.Setup(m => m.Alias).Returns(model.Alias);

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllContentTypes())
                .Returns(new[] { contentTypeMock.Object });

            contentTypeServiceMock.Setup(m => m.Save(contentTypeMock.Object, 0)).Verifiable();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.SetContentTypeId(id, contentTypeMock.Object.Id)).Verifiable();

            documentTypeSynchronizer.Run();

            mocker.VerifyAll();
        }
    }
}