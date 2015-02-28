// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Logikfabrik.Umbraco.Jet.Data;
using Logikfabrik.Umbraco.Jet.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet.Test
{
    [TestClass]
    public class MediaTypeSynchronizationServiceTest
    {
        private const string IdForMediaTypeWithId = "D7B9B7F5-B2ED-4C2F-8239-9A2F50D14054";
        private const string NameForMediaTypeWithId = "MediaTypeWithId";
        private const string NameForMediaTypeWithoutId = "MediaTypeWithoutId";

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
    }
}
