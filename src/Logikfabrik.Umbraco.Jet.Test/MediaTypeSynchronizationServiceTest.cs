// <copyright file="MediaTypeSynchronizationServiceTest.cs" company="Logikfabrik">
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
    /// The <see cref="MediaTypeSynchronizationServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class MediaTypeSynchronizationServiceTest
    {
        private const string IdForMediaTypeWithId = "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054";
        private const string NameForMediaTypeWithId = "MediaTypeWithId";
        private const string NameForMediaTypeWithoutId = "MediaTypeWithoutId";

        /// <summary>
        /// Test to create media type with and without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateMediaTypeWithAndWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithId), typeof(MediaTypeWithoutId) });

            var mediaTypeWithIdMock = new Mock<IMediaType>();

            var mediaTypeWithoutIdMock = new Mock<IMediaType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeServiceMock.Setup(m => m.GetMediaType(NameForMediaTypeWithId.Alias())).Returns(mediaTypeWithIdMock.Object);
            contentTypeServiceMock.Setup(m => m.GetMediaType(NameForMediaTypeWithoutId.Alias())).Returns(mediaTypeWithoutIdMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock
                .Verify(m => m.CreateMediaType(It.IsAny<Jet.MediaType>()), Times.Exactly(2));
        }

        /// <summary>
        /// Test to create media type with ID.
        /// </summary>
        [TestMethod]
        public void CanCreateMediaTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithId) });

            var mediaTypeMock = new Mock<IMediaType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeServiceMock.Setup(m => m.GetMediaType(NameForMediaTypeWithId.Alias())).Returns(mediaTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock
                .Verify(m => m.CreateMediaType(It.IsAny<Jet.MediaType>()), Times.Once);
        }

        /// <summary>
        /// Test to create media type without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateMediaTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithoutId) });

            var mediaTypeMock = new Mock<IMediaType>();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeServiceMock.Setup(m => m.GetMediaType(NameForMediaTypeWithoutId.Alias())).Returns(mediaTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock
                .Verify(m => m.CreateMediaType(It.IsAny<Jet.MediaType>()), Times.Once);
        }

        /// <summary>
        /// Test to update media type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateMediaTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithId) });

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(NameForMediaTypeWithId.Alias())).Returns(mediaTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(Guid.Parse(IdForMediaTypeWithId))).Returns(mediaTypeMock.Object.Id);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock.Verify(m => m.UpdateMediaType(mediaTypeMock.Object, It.IsAny<Jet.MediaType>()), Times.Once);
        }

        /// <summary>
        /// Test to update name for media type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateNameForMediaTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithId) });

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.SetupAllProperties();

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(NameForMediaTypeWithId.Alias())).Returns(mediaTypeMock.Object);

            var contentTypeRepositoryMock = new Mock<IContentTypeRepository>();

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeId(Guid.Parse(IdForMediaTypeWithId))).Returns(mediaTypeMock.Object.Id);

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(
                contentTypeServiceMock.Object,
                contentTypeRepositoryMock.Object,
                typeServiceMock.Object);

            mediaTypeSynchronizationService.Synchronize();

            mediaTypeMock.VerifySet(m => m.Name = NameForMediaTypeWithId, Times.Once);
        }

        /// <summary>
        /// Test to update media type without ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateMediaTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithoutId) });

            var mediaTypeMock = new Mock<IMediaType>();

            mediaTypeMock.Setup(m => m.Alias).Returns(NameForMediaTypeWithoutId.Alias());

            var contentTypeServiceMock = new Mock<IContentTypeService>();

            contentTypeServiceMock.Setup(m => m.GetAllMediaTypes()).Returns(new[] { mediaTypeMock.Object });
            contentTypeServiceMock.Setup(m => m.GetMediaType(NameForMediaTypeWithoutId.Alias())).Returns(mediaTypeMock.Object);

            var mediaTypeSynchronizationServiceMock = new Mock<MediaTypeSynchronizationService>(
                contentTypeServiceMock.Object,
                new Mock<IContentTypeRepository>().Object,
                typeServiceMock.Object)
            { CallBase = true };

            mediaTypeSynchronizationServiceMock.Object.Synchronize();

            mediaTypeSynchronizationServiceMock.Verify(m => m.UpdateMediaType(mediaTypeMock.Object, It.IsAny<Jet.MediaType>()), Times.Once);
        }

        /// <summary>
        /// The <see cref="MediaTypeWithId" /> class.
        /// </summary>
        [MediaType(
            IdForMediaTypeWithId,
            NameForMediaTypeWithId)]
        protected class MediaTypeWithId
        {
        }

        /// <summary>
        /// The <see cref="MediaTypeWithoutId" /> class.
        /// </summary>
        [MediaType(
            NameForMediaTypeWithoutId)]
        protected class MediaTypeWithoutId
        {
        }
    }
}
