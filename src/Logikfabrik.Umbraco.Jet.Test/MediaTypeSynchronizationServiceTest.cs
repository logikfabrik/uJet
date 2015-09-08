// <copyright file="MediaTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

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
    public class MediaTypeSynchronizationServiceTest
    {
        private const string IdForMediaTypeWithId = "D7B9B7F5-B2ED-4C2F-8239-9A2F50D14054";
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

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(contentTypeService.Object,
                contentTypeRepository.Object, typeService.Object);

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

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(contentTypeService.Object,
                contentTypeRepository.Object, typeService.Object);

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

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(contentTypeService.Object,
                contentTypeRepository.Object, typeService.Object);

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

            var mediaTypeSynchronizationService = new MediaTypeSynchronizationService(contentTypeService.Object,
                contentTypeRepository.Object, typeService.Object);

            mediaTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForMediaTypeWithId, withIdMediaType.Object.Name);
        }

        [MediaType(IdForMediaTypeWithId, NameForMediaTypeWithId)]
        public class MediaTypeWithId
        {
        }

        [MediaType(NameForMediaTypeWithoutId)]
        public class MediaTypeWithoutId
        {
        }

        public class MediaTypeWithPropertyWithId
        {
            // TODO: Test.
        }

        public class MediaTypeWithPropertyWithoutId
        {
            // TODO: Test.
        }
    }
}
