// <copyright file="MediaServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using Jet.Web.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Utilities;

    [TestClass]
    public class MediaServiceTest : TestBase
    {
        [TestMethod]
        public void CanGetMediaIdByConvention()
        {
            const int id = 123;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Id).Returns(id);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(id, media.Id);
        }

        [TestMethod]
        public void CanGetMediaUrlByConvention()
        {
            const int id = 123;
            const string url = "/umbraco/jet";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Url).Returns(url);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(url, media.Url);
        }

        [TestMethod]
        public void CanGetMediaNameByConvention()
        {
            const int id = 123;
            const string name = "Umbraco Jet";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Name).Returns(name);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(name, media.Name);
        }

        [TestMethod]
        public void CanGetMediaCreateDateByConvention()
        {
            const int id = 123;
            var createDate = new DateTime(2015, 1, 1);

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.CreateDate).Returns(createDate);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(createDate, media.CreateDate);
        }

        [TestMethod]
        public void CanGetMediaUpdateDateByConvention()
        {
            const int id = 123;
            var updateDate = new DateTime(2015, 1, 1);

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.UpdateDate).Returns(updateDate);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(updateDate, media.UpdateDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotGetMediaForInvalidMediaType()
        {
            var type = TypeUtility.GetTypeBuilder("MyType", TypeUtility.GetTypeAttributes()).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            var service = new MediaService(new Mock<IUmbracoHelperWrapper>().Object);

            service.GetMedia(contentMock.Object, type);
        }

        [TestMethod]
        public void CanGetMediaForValidMediaType()
        {
            var type = MediaTypeUtility.GetTypeBuilder().CreateType();

            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(new List<IPublishedProperty>());

            var service = new MediaService(new Mock<IUmbracoHelperWrapper>().Object);

            var media = service.GetMedia(contentMock.Object, type);

            Assert.IsNotNull(media);
        }

        [TestMethod]
        public void CanGetMediaForMediaTypeWithStringProperty()
        {
            const int id = 123;
            const string stringPropertyName = "stringProperty";
            const string stringPropertyValue = "StringProperty";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(stringPropertyName);
                property.Setup(m => m.Value).Returns(stringPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(stringPropertyValue, media.StringProperty);
        }

        [TestMethod]
        public void CanGetMediaForMediaTypeWithIntegerProperty()
        {
            const int id = 123;
            const string integerPropertyName = "integerProperty";
            const int integerPropertyValue = 7;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(integerPropertyName);
                property.Setup(m => m.Value).Returns(integerPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(integerPropertyValue, media.IntegerProperty);
        }

        [TestMethod]
        public void CanGetMediaForMediaTypeWithFloatingBinaryPointProperty()
        {
            const int id = 123;
            const string floatingBinaryPointPropertyName = "FloatingBinaryPointProperty";
            const float floatingBinaryPointPropertyValue = 2.2f;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(floatingBinaryPointPropertyName);
                property.Setup(m => m.Value).Returns(floatingBinaryPointPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(floatingBinaryPointPropertyValue, media.FloatingBinaryPointProperty);
        }

        [TestMethod]
        public void CanGetMediaForMediaTypeWithFloatingDecimalPointProperty()
        {
            const int id = 123;
            const string floatingDecimalPointPropertyName = "FloatingDecimalPointProperty";
            const decimal floatingDecimalPointPropertyValue = 2.2m;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(floatingDecimalPointPropertyName);
                property.Setup(m => m.Value).Returns(floatingDecimalPointPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(floatingDecimalPointPropertyValue, media.FloatingDecimalPointProperty);
        }

        [TestMethod]
        public void CanGetMediaForMediaTypeWithBooleanProperty()
        {
            const int id = 123;
            const string booleanPropertyName = "BooleanProperty";
            const bool booleanPropertyValue = true;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(booleanPropertyName);
                property.Setup(m => m.Value).Returns(booleanPropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(booleanPropertyValue, media.BooleanProperty);
        }

        [TestMethod]
        public void CanGetMediaForMediaTypeWithDateTimeProperty()
        {
            const int id = 123;
            const string dateTimePropertyName = "DateTimeProperty";
            var dateTimePropertyValue = DateTime.Now;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = new Mock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(dateTimePropertyName);
                property.Setup(m => m.Value).Returns(dateTimePropertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            Assert.AreEqual(dateTimePropertyValue, media.DateTimeProperty);
        }
    }
}