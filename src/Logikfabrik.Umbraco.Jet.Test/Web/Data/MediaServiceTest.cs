// <copyright file="MediaServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data
{
    using System;
    using System.Collections.Generic;
    using AutoFixture.Xunit2;
    using global::Umbraco.Core.Models;
    using Jet.Web.Data;
    using Moq;
    using Shouldly;
    using Utilities;
    using Xunit;

    public class MediaServiceTest
    {
        [Fact]
        public void CanGetMediaIdByConvention()
        {
            const int id = 123;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Id).Returns(id);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedMedia(id)).Returns(publishedContentMock.Object);

            var media = new MediaService(umbracoHelperWrapperMock.Object).GetMedia<Models.MediaType>(id);

            media.Id.ShouldBe(id);
        }

        [Fact]
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

            media.Url.ShouldBe(url);
        }

        [Fact]
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

            media.Name.ShouldBe(name);
        }

        [Fact]
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

            media.CreateDate.ShouldBe(createDate);
        }

        [Fact]
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

            media.UpdateDate.ShouldBe(updateDate);
        }

        [Fact]
        public void CanNotGetMediaForInvalidMediaType()
        {
            var type = TypeUtility.GetTypeBuilder("MyType", TypeUtility.GetTypeAttributes()).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            var service = new MediaService(new Mock<IUmbracoHelperWrapper>().Object);

            Assert.Throws<ArgumentException>(() => service.GetMedia(contentMock.Object, type));
        }

        [Theory]
        [AutoData]
        public void CanGetMediaForValidMediaType(string typeName, string name)
        {
            var modelType = new MediaTypeModelTypeBuilder(typeName, name).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(new List<IPublishedProperty>());

            var service = new MediaService(new Mock<IUmbracoHelperWrapper>().Object);

            var media = service.GetMedia(contentMock.Object, modelType);

            media.ShouldNotBeNull();
        }

        [Fact]
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

            media.StringProperty.ShouldBe(stringPropertyValue);
        }

        [Fact]
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

            media.IntegerProperty.ShouldBe(integerPropertyValue);
        }

        [Fact]
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

            media.FloatingBinaryPointProperty.ShouldBe(floatingBinaryPointPropertyValue);
        }

        [Fact]
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

            media.FloatingDecimalPointProperty.ShouldBe(floatingDecimalPointPropertyValue);
        }

        [Fact]
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

            media.BooleanProperty.ShouldBe(booleanPropertyValue);
        }

        [Fact]
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

            media.DateTimeProperty.ShouldBe(dateTimePropertyValue);
        }
    }
}