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

    /// <summary>
    /// The <see cref="DocumentServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class DocumentServiceTest : TestBase
    {
        /// <summary>
        /// Test to get document with ID mapped by convention.
        /// </summary>
        [TestMethod]
        public void CanGetDocumentIdByConvention()
        {
            const int id = 123;

            var publishedContentMock = new Mock<IPublishedContent>();

            publishedContentMock.Setup(m => m.Id).Returns(id);
            publishedContentMock.Setup(m => m.Properties).Returns(new IPublishedProperty[] { });

            var umbracoHelperWrapperMock = new Mock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(publishedContentMock.Object);

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(id, document.Id);
        }

        /// <summary>
        /// Test to get document with URL mapped by convention.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(url, document.Url);
        }

        /// <summary>
        /// Test to get document with name mapped by convention.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(name, document.Name);
        }

        /// <summary>
        /// Test to get document with create date mapped by convention.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(createDate, document.CreateDate);
        }

        /// <summary>
        /// Test to get document with update date mapped by convention.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(updateDate, document.UpdateDate);
        }

        /// <summary>
        /// Test to get document for invalid document type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotGetDocumentForInvalidDocumentType()
        {
            var type = TypeUtility.GetTypeBuilder("MyType", TypeUtility.GetTypeAttributes()).CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var contentMock = new Mock<IPublishedContent>();

            var service = new DocumentService(new Mock<IUmbracoHelperWrapper>().Object, typeServiceMock.Object);

            service.GetDocument(contentMock.Object, type);
        }

        /// <summary>
        /// Test to get document for valid document type.
        /// </summary>
        [TestMethod]
        public void CanGetDocumentForValidDocumentType()
        {
            var type = DocumentTypeUtility.GetTypeBuilder().CreateType();

            var typeServiceMock = GetTypeServiceMock(type);

            var contentMock = new Mock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(new List<IPublishedProperty>());

            var service = new DocumentService(new Mock<IUmbracoHelperWrapper>().Object, typeServiceMock.Object);

            var document = service.GetDocument(contentMock.Object, type);

            Assert.IsNotNull(document);
        }

        /// <summary>
        /// Test to get document for document type with <see cref="string" /> property.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(stringPropertyValue, document.StringProperty);
        }

        /// <summary>
        /// Test to get document for document type with <see cref="int" /> property.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(integerPropertyValue, document.IntegerProperty);
        }

        /// <summary>
        /// Test to get document for document type with <see cref="float" /> property.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(floatingBinaryPointPropertyValue, document.FloatingBinaryPointProperty);
        }

        /// <summary>
        /// Test to get document for document type with <see cref="double" /> property.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(floatingDecimalPointPropertyValue, document.FloatingDecimalPointProperty);
        }

        /// <summary>
        /// Test to get document for document type with <see cref="bool" /> property.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(booleanPropertyValue, document.BooleanProperty);
        }

        /// <summary>
        /// Test to get document for document type with <see cref="DateTime" /> property.
        /// </summary>
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

            var typeServiceMock = GetTypeServiceMock(typeof(DocumentType));

            var document = new DocumentService(umbracoHelperWrapperMock.Object, typeServiceMock.Object).GetDocument<DocumentType>(id);

            Assert.AreEqual(dateTimePropertyValue, document.DateTimeProperty);
        }

        /// <summary>
        /// The <see cref="DocumentType" /> class.
        /// </summary>
        [DocumentType("DocumentType")]
        protected class DocumentType
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the URL.
            /// </summary>
            /// <value>
            /// The URL.
            /// </value>
            public string Url { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the create date.
            /// </summary>
            /// <value>
            /// The create date.
            /// </value>
            public DateTime CreateDate { get; set; }

            /// <summary>
            /// Gets or sets the update date.
            /// </summary>
            /// <value>
            /// The update date.
            /// </value>
            public DateTime UpdateDate { get; set; }

            /// <summary>
            /// Gets or sets the string property value.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            public string StringProperty { get; set; }

            /// <summary>
            /// Gets or sets the integer property value.
            /// </summary>
            /// <value>
            /// The integer property value.
            /// </value>
            public int IntegerProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating binary point property value.
            /// </summary>
            /// <value>
            /// The floating binary point property value.
            /// </value>
            public float FloatingBinaryPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating decimal point property value.
            /// </summary>
            /// <value>
            /// The floating decimal point property value.
            /// </value>
            public decimal FloatingDecimalPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the boolean property value.
            /// </summary>
            /// <value>
            /// The boolean property value.
            /// </value>
#pragma warning disable SA1623
            public bool BooleanProperty { get; set; }
#pragma warning restore SA1623

            /// <summary>
            /// Gets or sets the DateTime property value.
            /// </summary>
            /// <value>
            /// The DateTime property value.
            /// </value>
            public DateTime DateTimeProperty { get; set; }
        }
    }
}
