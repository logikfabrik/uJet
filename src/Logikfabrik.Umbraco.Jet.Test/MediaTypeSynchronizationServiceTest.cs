// <copyright file="MediaTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using Data;
    using Extensions;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The <see cref="DataTypeSynchronizationServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class MediaTypeSynchronizationServiceTest
    {
        private const string IdForMediaTypeWithId = "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054";
        private const string NameForMediaTypeWithId = "MediaTypeWithId";
        private const string NameForMediaTypeWithoutId = "MediaTypeWithoutId";

        [TestMethod]
        public void CanCreateMediaTypeWithAndWithoutId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withIdMediaType = new Mock<IMediaType>();
            var withoutIdMediaType = new Mock<IMediaType>();

            withIdMediaType.SetupAllProperties();
            withoutIdMediaType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeService.Setup(m => m.Save(It.IsAny<IMediaType>(), It.IsAny<int>()))
                .Callback<IMediaType, int>((ct, userId) =>
                {
                    switch (ct.Name)
                    {
                        case NameForMediaTypeWithId:
                            withIdMediaType.Object.Name = ct.Name;
                            break;

                        case NameForMediaTypeWithoutId:
                            withoutIdMediaType.Object.Name = ct.Name;
                            break;
                    }
                });
            contentTypeService.Setup(m => m.GetMediaType(It.Is<string>(v => v == NameForMediaTypeWithId.Alias())))
                .Returns(withIdMediaType.Object);
            contentTypeService.Setup(m => m.GetMediaType(It.Is<string>(v => v == NameForMediaTypeWithoutId.Alias())))
                .Returns(withoutIdMediaType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithId), typeof(MediaTypeWithoutId) });

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(
                contentTypeService.Object, contentTypeRepository.Object, typeService.Object);

            mediaTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForMediaTypeWithId, withIdMediaType.Object.Name);
            Assert.AreEqual(NameForMediaTypeWithoutId, withoutIdMediaType.Object.Name);
        }

        [TestMethod]
        public void CanCreateMediaTypeWithId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withIdMediaType = new Mock<IMediaType>();

            withIdMediaType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeService.Setup(m => m.Save(It.IsAny<IMediaType>(), It.IsAny<int>()))
                .Callback<IMediaType, int>((ct, userId) =>
                {
                    withIdMediaType.Object.Name = ct.Name;
                });
            contentTypeService.Setup(m => m.GetMediaType(It.Is<string>(v => v == NameForMediaTypeWithId.Alias())))
                .Returns(withIdMediaType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithId) });

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(
                contentTypeService.Object, contentTypeRepository.Object, typeService.Object);

            mediaTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForMediaTypeWithId, withIdMediaType.Object.Name);
        }

        [TestMethod]
        public void CanCreateMediaTypeWithoutId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withoutIdMediaType = new Mock<IMediaType>();

            withoutIdMediaType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllMediaTypes()).Returns(new IMediaType[] { });
            contentTypeService.Setup(m => m.Save(It.IsAny<IMediaType>(), It.IsAny<int>()))
                .Callback<IMediaType, int>((ct, userId) =>
                {
                    withoutIdMediaType.Object.Name = ct.Name;
                });
            contentTypeService.Setup(m => m.GetMediaType(It.Is<string>(v => v == NameForMediaTypeWithoutId.Alias())))
                .Returns(withoutIdMediaType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithoutId) });

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(
                contentTypeService.Object, contentTypeRepository.Object, typeService.Object);

            mediaTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForMediaTypeWithoutId, withoutIdMediaType.Object.Name);
        }

        [TestMethod]
        public void CanUpdateNameForMediaTypeWithId()
        {
            var contentTypeService = new Mock<IContentTypeService>();
            var contentTypeRepository = new Mock<IContentTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withIdDbMediaType = new Mock<IMediaType>();
            var withIdMediaType = new Mock<IMediaType>();

            withIdDbMediaType.SetupAllProperties();
            withIdDbMediaType.Object.Id = 1234;
            withIdDbMediaType.Object.Name = "DbMediaTypeWithId";

            withIdMediaType.SetupAllProperties();

            contentTypeService.Setup(m => m.GetAllMediaTypes()).Returns(new[] { withIdDbMediaType.Object });
            contentTypeService.Setup(m => m.Save(It.IsAny<IMediaType>(), It.IsAny<int>()))
                .Callback<IMediaType, int>((ct, userId) =>
                {
                    withIdMediaType.Object.Name = ct.Name;
                });
            contentTypeService.Setup(m => m.GetMediaType(It.Is<string>(v => v == NameForMediaTypeWithId.Alias())))
                .Returns(withIdMediaType.Object);
            contentTypeRepository.Setup(m => m.GetContentTypeId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.MediaTypes).Returns(new[] { typeof(MediaTypeWithId) });

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(
                contentTypeService.Object, contentTypeRepository.Object, typeService.Object);

            mediaTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForMediaTypeWithId, withIdMediaType.Object.Name);
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
