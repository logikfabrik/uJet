// <copyright file="MediaTypeSynchronizerTest.cs" company="Logikfabrik">
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
    public class MediaTypeSynchronizerTest
    {
        [TestMethod]
        public void CanCreateMediaTypeWithAndWithoutId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));
            var mediaTypeWithoutId = new Jet.MediaType(typeof(MediaTypeWithoutId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(Array.AsReadOnly(new[] { mediaTypeWithId, mediaTypeWithoutId }));

            var mediaTypes = new List<IMediaType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(mediaTypes);
            contentTypeServiceMock.Setup(m => m.Save(It.IsAny<IMediaType>(), 0)).Callback((IMediaType mediaType, int userId) => { mediaTypes.Add(mediaType); });

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizer>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Run();

            mediaTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(It.IsAny<Jet.MediaType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateMediaTypeWithId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(Array.AsReadOnly(new[] { mediaTypeWithId }));

            var mediaTypes = new List<IMediaType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(mediaTypes);
            contentTypeServiceMock.Setup(m => m.Save(It.IsAny<IMediaType>(), 0)).Callback((IMediaType mediaType, int userId) => { mediaTypes.Add(mediaType); });

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizer>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Run();

            mediaTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(mediaTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateMediaTypeWithoutId()
        {
            var mediaTypeWithoutId = new Jet.MediaType(typeof(MediaTypeWithoutId));

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(Array.AsReadOnly(new[] { mediaTypeWithoutId }));

            var mediaTypes = new List<IMediaType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(mediaTypes);
            contentTypeServiceMock.Setup(m => m.Save(It.IsAny<IMediaType>(), 0)).Callback((IMediaType mediaType, int userId) => { mediaTypes.Add(mediaType); });

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizer>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Run();

            mediaTypeSynchronizationServiceMock.Verify(m => m.CreateContentType(mediaTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateMediaTypeWithId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(Array.AsReadOnly(new[] { mediaTypeWithId }));

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithId.Alias)).Returns(mediaTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(mediaTypeWithId.Id.Value)).Returns(mediaTypeMock.Object.Id);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizer>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Run();

            mediaTypeSynchronizationServiceMock.Verify(m => m.UpdateContentType(mediaTypeMock.Object, mediaTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForMediaTypeWithId()
        {
            var mediaTypeWithId = new Jet.MediaType(typeof(MediaTypeWithId));

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(Array.AsReadOnly(new[] { mediaTypeWithId }));

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithId.Alias)).Returns(mediaTypeMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetContentTypeId(mediaTypeWithId.Id.Value)).Returns(mediaTypeMock.Object.Id);

            var mediaTypeSynchronizationService = new MediaTypeSynchronizer(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object);

            mediaTypeSynchronizationService.Run();

            mediaTypeMock.VerifySet(m => m.Name = mediaTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateMediaTypeWithoutId()
        {
            var mediaTypeWithoutId = new Jet.MediaType(typeof(MediaTypeWithoutId));

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();
            mediaTypeMock.Object.Alias = mediaTypeWithoutId.Alias;

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.MediaTypes).Returns(Array.AsReadOnly(new[] { mediaTypeWithoutId }));

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(mediaTypeWithoutId.Alias)).Returns(mediaTypeMock.Object);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizer>(
                contentTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Run();

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
