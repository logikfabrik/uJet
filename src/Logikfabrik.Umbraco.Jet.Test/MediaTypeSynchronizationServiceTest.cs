// <copyright file="MediaTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MediaTypeSynchronizationServiceTest
    {
        [TestMethod]
        public void CanCreateMediaTypeWithAndWithoutId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));
            var mediaTypeWithoutId = new Jet.MediaType(typeof(MediaTypeWithoutId));

            var mediaTypeWithIdMock = new Mock<IMediaType>();
            var mediaTypeWithoutIdMock = new Mock<IMediaType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(new[] { mediaTypeWithId, mediaTypeWithoutId });

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithId.Alias)).Returns(mediaTypeWithIdMock.Object);
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithoutId.Alias)).Returns(mediaTypeWithoutIdMock.Object);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(It.IsAny<Jet.MediaType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateMediaTypeWithId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));

            var mediaTypeMock = new Mock<IMediaType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(new[] { mediaTypeWithId });

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithId.Alias)).Returns(mediaTypeMock.Object);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(mediaTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateMediaTypeWithoutId()
        {
            var mediaTypeWithoutId = new Jet.MediaType(typeof(MediaTypeWithoutId));

            var mediaTypeMock = new Mock<IMediaType>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(new[] { mediaTypeWithoutId });

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithoutId.Alias)).Returns(mediaTypeMock.Object);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(mediaTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateMediaTypeWithId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(new[] { mediaTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType<Jet.MediaType, MediaTypeAttribute>(mediaTypeWithId, It.IsAny<IContentTypeBase[]>())).Returns(mediaTypeMock.Object);

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithId.Alias)).Returns(mediaTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(mediaTypeWithId.Id.Value)).Returns(mediaTypeMock.Object.Id);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(mediaTypeMock.Object, mediaTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForMediaTypeWithId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(new[] { mediaTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType<Jet.MediaType, MediaTypeAttribute>(mediaTypeWithId, It.IsAny<IContentTypeBase[]>())).Returns(mediaTypeMock.Object);

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithId.Alias)).Returns(mediaTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(mediaTypeWithId.Id.Value)).Returns(mediaTypeMock.Object.Id);

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object);

            mediaTypeSynchronizationService.Synchronize();

            mediaTypeMock.VerifySet(m => m.Name = mediaTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateMediaTypeWithoutId()
        {
            var mediaTypeWithoutId = new Jet.MediaType(typeof(MediaTypeWithoutId));

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(new[] { mediaTypeWithoutId });
            typeResolverMock.Setup(m => m.ResolveType<Jet.MediaType, MediaTypeAttribute>(mediaTypeWithoutId, It.IsAny<IContentTypeBase[]>())).Returns(mediaTypeMock.Object);

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithoutId.Alias)).Returns(mediaTypeMock.Object);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(mediaTypeMock.Object, mediaTypeWithoutId), Times.Once);
        }

        [MediaType(
            "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054",
            "MediaTypeWithId")]
        protected class MediaTypeWithId
        {
        }

        [MediaType(
            "MediaTypeWithoutId")]
        protected class MediaTypeWithoutId
        {
        }
    }
}
