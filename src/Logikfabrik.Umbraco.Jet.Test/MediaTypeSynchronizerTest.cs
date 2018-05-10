// <copyright file="MediaTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Data;
    using Moq;
    using Moq.AutoMock;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class MediaTypeSynchronizerTest
    {
        [Theory]
        [CustomAutoData]
        public void CanCreateModelWithoutId(Jet.MediaType model)
        {
            var mocker = new AutoMocker();

            var mediaTypeSynchronizer = mocker.CreateInstance<MediaTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<IModelService>();

            typeResolverMock
                .Setup(m => m.MediaTypes)
                .Returns(new[] { model });

            var mediaTypes = new List<IMediaType>();

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllMediaTypes())
                .Returns(mediaTypes);

            contentTypeServiceMock
                .Setup(m => m.Save(It.Is<IMediaType>(mediaType => mediaType.Id == 0), 0))
                .Callback((IMediaType mediaType, int userId) => { mediaTypes.Add(mediaType); })
                .Verifiable();

            mediaTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanCreateModelWithId(string typeName, Guid id, string name)
        {
            var modelType = new MediaTypeModelTypeBuilder(typeName, id, name).Create(Scope.Public);

            var model = new Jet.MediaType(modelType);

            var mocker = new AutoMocker();

            var mediaTypeSynchronizer = mocker.CreateInstance<MediaTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<IModelService>();

            typeResolverMock
                .Setup(m => m.MediaTypes)
                .Returns(new[] { model });

            var mediaTypes = new List<IMediaType>();

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllMediaTypes())
                .Returns(mediaTypes);

            contentTypeServiceMock
                .Setup(m => m.Save(It.Is<IMediaType>(mediaType => mediaType.Id == 0), 0))
                .Callback((IMediaType mediaType, int userId) => { mediaTypes.Add(mediaType); })
                .Verifiable();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.SetContentTypeId(id, It.IsAny<int>())).Verifiable();

            mediaTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithoutId(Jet.MediaType model)
        {
            var mocker = new AutoMocker();

            var mediaTypeSynchronizer = mocker.CreateInstance<MediaTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<IModelService>();

            typeResolverMock
                .Setup(m => m.MediaTypes)
                .Returns(new[] { model });

            var mediaTypeMock = mocker.GetMock<IMediaType>();

            mediaTypeMock.Setup(m => m.Alias).Returns(model.Alias);

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllMediaTypes())
                .Returns(new[] { mediaTypeMock.Object });

            contentTypeServiceMock.Setup(m => m.Save(mediaTypeMock.Object, 0)).Verifiable();

            mediaTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithId(string typeName, Guid id, string name)
        {
            var modelType = new MediaTypeModelTypeBuilder(typeName, id, name).Create(Scope.Public);

            var model = new Jet.MediaType(modelType);

            var mocker = new AutoMocker();

            var mediaTypeSynchronizer = mocker.CreateInstance<MediaTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<IModelService>();

            typeResolverMock
                .Setup(m => m.MediaTypes)
                .Returns(new[] { model });

            var mediaTypeMock = mocker.GetMock<IMediaType>();

            mediaTypeMock.Setup(m => m.Alias).Returns(model.Alias);

            var contentTypeServiceMock = mocker.GetMock<IContentTypeService>();

            contentTypeServiceMock
                .Setup(m => m.GetAllMediaTypes())
                .Returns(new[] { mediaTypeMock.Object });

            contentTypeServiceMock.Setup(m => m.Save(mediaTypeMock.Object, 0)).Verifiable();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.SetContentTypeId(id, mediaTypeMock.Object.Id)).Verifiable();

            mediaTypeSynchronizer.Run();

            mocker.VerifyAll();
        }
    }
}
