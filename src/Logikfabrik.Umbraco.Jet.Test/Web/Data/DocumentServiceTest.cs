// <copyright file="DocumentServiceTest.cs" company="Logikfabrik">
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
    public class DocumentServiceTest : TestBase
    {
        [TestMethod]
        public void CanGetDocumentIdByConvention()
        {
            const int id = 123;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Id).Returns(id);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            Assert.AreEqual(id, document.Id);
        }

        [TestMethod]
        public void CanGetDocumentUrlByConvention()
        {
            const int id = 123;
            const string url = "/umbraco/jet";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Url).Returns(url);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            Assert.AreEqual(url, document.Url);
        }

        [TestMethod]
        public void CanGetDocumentNameByConvention()
        {
            const int id = 123;
            const string name = "Umbraco Jet";

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Name).Returns(name);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var document = new DocumentService(umbracoHelperWrapperMock.Object).GetDocument<Models.DocumentType>(id);

            Assert.AreEqual(name, document.Name);
        }

        [TestMethod]
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

            Assert.AreEqual(createDate, document.CreateDate);
        }

        [TestMethod]
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

            Assert.AreEqual(updateDate, document.UpdateDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotGetDocumentForInvalidDocumentType()
        {
            var type = TypeUtility.GetTypeBuilder("MyType", TypeUtility.GetTypeAttributes()).CreateType();

            var contentMock = new Mock<IPublishedContent>();

            var service = new DocumentService(new Mock<IUmbracoHelperWrapper>().Object);

            service.GetDocument(contentMock.Object, type);
        }

        [TestMethod]
        public void CanGetDocumentForValidDocumentType()
        {
            var type = DocumentTypeUtility.GetTypeBuilder().CreateType();

            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(new List<IPublishedProperty>());

            var service = new DocumentService(new Mock<IUmbracoHelperWrapper>().Object);

            var document = service.GetDocument(contentMock.Object, type);

            Assert.IsNotNull(document);
        }

        [TestMethod]
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

            Assert.AreEqual(stringPropertyValue, document.StringProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(integerPropertyValue, document.IntegerProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(floatingBinaryPointPropertyValue, document.FloatingBinaryPointProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(floatingDecimalPointPropertyValue, document.FloatingDecimalPointProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(booleanPropertyValue, document.BooleanProperty);
        }

        [TestMethod]
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

            Assert.AreEqual(dateTimePropertyValue, document.DateTimeProperty);
        }
    }
}
