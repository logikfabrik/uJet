// <copyright file="ContentTypeRepositoryTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Data
{
    using System;
    using Jet.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ContentTypeRepositoryTest
    {
        [TestMethod]
        public void CanGetContentTypeModelId()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeByContentTypeId(5)).Returns(new ContentType { ContentTypeId = 5, Id = modelId });

            Assert.AreEqual(modelId, contentTypeRepositoryMock.Object.GetContentTypeModelId(5));
        }

        [TestMethod]
        public void CanGetContentTypeModelIdFromCache()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeByContentTypeId(5)).Returns(new ContentType { ContentTypeId = 5, Id = modelId });

            contentTypeRepositoryMock.Object.GetContentTypeModelId(5);
            contentTypeRepositoryMock.Object.GetContentTypeModelId(5);

            contentTypeRepositoryMock.Verify(m => m.GetContentTypeByContentTypeId(5), Times.Once);
        }

        [TestMethod]
        public void CanGetPropertyTypeModelId()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetPropertyTypeByPropertyTypeId(5)).Returns(new PropertyType { PropertyTypeId = 5, Id = modelId });

            Assert.AreEqual(modelId, contentTypeRepositoryMock.Object.GetPropertyTypeModelId(5));
        }

        [TestMethod]
        public void CanGetPropertyTypeModelIdFromCache()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetPropertyTypeByPropertyTypeId(5)).Returns(new PropertyType { PropertyTypeId = 5, Id = modelId });

            contentTypeRepositoryMock.Object.GetPropertyTypeModelId(5);
            contentTypeRepositoryMock.Object.GetPropertyTypeModelId(5);

            contentTypeRepositoryMock.Verify(m => m.GetPropertyTypeByPropertyTypeId(5), Times.Once);
        }

        [TestMethod]
        public void CanGetContentTypeId()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeById(modelId)).Returns(new ContentType { ContentTypeId = 5, Id = modelId });

            Assert.AreEqual(5, contentTypeRepositoryMock.Object.GetContentTypeId(modelId));
        }

        [TestMethod]
        public void CanGetContentTypeIdFromCache()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetContentTypeById(modelId)).Returns(new ContentType { ContentTypeId = 5, Id = modelId });

            contentTypeRepositoryMock.Object.GetContentTypeId(modelId);
            contentTypeRepositoryMock.Object.GetContentTypeId(modelId);

            contentTypeRepositoryMock.Verify(m => m.GetContentTypeById(modelId), Times.Once);
        }

        [TestMethod]
        public void CanGetPropertyTypeId()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetPropertyTypeById(modelId)).Returns(new PropertyType { PropertyTypeId = 5, Id = modelId });

            Assert.AreEqual(5, contentTypeRepositoryMock.Object.GetPropertyTypeId(modelId));
        }

        [TestMethod]
        public void CanGetPropertyTypeIdFromCache()
        {
            var modelId = Guid.Parse("22f44f53-81de-4c2e-a9b9-186ea053d234");

            var contentTypeRepositoryMock = new Mock<ContentTypeRepository>(new Mock<IDatabaseWrapper>().Object)
            {
                CallBase = true
            };

            contentTypeRepositoryMock.Setup(m => m.GetPropertyTypeById(modelId)).Returns(new PropertyType { PropertyTypeId = 5, Id = modelId });

            contentTypeRepositoryMock.Object.GetPropertyTypeId(modelId);
            contentTypeRepositoryMock.Object.GetPropertyTypeId(modelId);

            contentTypeRepositoryMock.Verify(m => m.GetPropertyTypeById(modelId), Times.Once);
        }
    }
}