// <copyright file="DocumentServiceTest.cs" company="Logikfabrik">
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

    public class DocumentServiceTest : TestBase
    {
        [Theory]
        [AutoData]
        public void CanGetDocumentIdByConvention(int id)
        {
            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Id).Returns(id);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.Id.ShouldBe(id);
        }

        [Theory]
        [AutoData]
        public void CanGetDocumentUrlByConvention(int id, Uri url)
        {
            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Url).Returns(url.ToString);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.Url.ShouldBe(url.ToString());
        }

        [Theory]
        [AutoData]
        public void CanGetDocumentNameByConvention(int id, string name)
        {
            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Name).Returns(name);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.Name.ShouldBe(name);
        }

        [Fact]
        public void CanGetDocumentCreateDateByConvention()
        {
            const int id = 123;
            var createDate = new DateTime(2015, 1, 1);

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.CreateDate).Returns(createDate);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.CreateDate.ShouldBe(createDate);
        }

        [Fact]
        public void CanGetDocumentUpdateDateByConvention()
        {
            const int id = 123;
            var updateDate = new DateTime(2015, 1, 1);

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.UpdateDate).Returns(updateDate);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.UpdateDate.ShouldBe(updateDate);
        }

        [Fact]
        public void CanNotGetDocumentForInvalidDocumentType()
        {
            var type = TypeUtility.GetTypeBuilder("MyType", TypeUtility.GetTypeAttributes()).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            var service = new DocumentService(new Mock<IUmbracoHelperWrapper>().Object);

            Assert.Throws<ArgumentException>(() => service.GetDocument(contentMock.Object, type));
        }

        [Theory]
        [AutoData]
        public void CanGetDocumentForValidDocumentType(string typeName, string name)
        {
            var type = new DocumentTypeModelTypeBuilder(typeName, name).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(new List<IPublishedProperty>());

            var service = new DocumentService(new Mock<IUmbracoHelperWrapper>().Object);

            var document = service.GetDocument(contentMock.Object, type);

            document.ShouldNotBeNull();
        }

        [Fact]
        public void CanGetDocumentForDocumentTypeWithStringProperty()
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

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.StringProperty.ShouldBe(stringPropertyValue);
        }

        [Fact]
        public void CanGetDocumentForDocumentTypeWithIntegerProperty()
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

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.IntegerProperty.ShouldBe(integerPropertyValue);
        }

        [Fact]
        public void CanGetDocumentForDocumentTypeWithFloatingBinaryPointProperty()
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

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.FloatingBinaryPointProperty.ShouldBe(floatingBinaryPointPropertyValue);
        }

        [Fact]
        public void CanGetDocumentForDocumentTypeWithFloatingDecimalPointProperty()
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

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.FloatingDecimalPointProperty.ShouldBe(floatingDecimalPointPropertyValue);
        }

        [Fact]
        public void CanGetDocumentForDocumentTypeWithBooleanProperty()
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

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.BooleanProperty.ShouldBe(booleanPropertyValue);
        }

        [Fact]
        public void CanGetDocumentForDocumentTypeWithDateTimeProperty()
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

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            document.DateTimeProperty.ShouldBe(dateTimePropertyValue);
        }
    }
}
