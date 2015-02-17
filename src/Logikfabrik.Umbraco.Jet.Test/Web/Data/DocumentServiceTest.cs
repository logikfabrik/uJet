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

using Logikfabrik.Umbraco.Jet.Extensions;
using Logikfabrik.Umbraco.Jet.Web.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Globalization;
using Umbraco.Core.Models;

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data
{
    [TestClass]
    public class DocumentServiceTest
    {
        [DocumentType("DocumentTypeWithTheDocumentTypeAttribute")]
        public class DocumentTypeWithTheDocumentTypeAttribute
        {
            // Should be mapped by convention.
            public int Id { get; set; }

            // Should be mapped by convention.
            public string Url { get; set; }

            // Should be mapped by convention.
            public string Name { get; set; }

            // Should be mapped by convention.
            public DateTime CreateDate { get; set; }

            // Should be mapped by convention.
            public DateTime UpdateDate { get; set; }

            public string StringProperty { get; set; }

            public int IntegerProperty { get; set; }

            public decimal FloatingDecimalProperty { get; set; }

            public float FloatingBinaryProperty { get; set; }

            public DateTime DateTimeProperty { get; set; }

            public bool BooleanProperty { get; set; }

            public decimal FloatingDecimalPropertyAsString { get; set; }

            public float FloatingBinaryPropertyAsString { get; set; }
        }

        public class DocumentTypeWithoutTheDocumentTypeAttribute
        {
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void CanNotGetDocumentForInvalidDocumentType()
        {
            const int id = 1234;

            var umbracoHelperWrapper = new Mock<IUmbracoHelperWrapper>();
            var typeService = new Mock<ITypeService>();

            var documentService = new DocumentService(umbracoHelperWrapper.Object, typeService.Object);

            documentService.GetDocument<DocumentTypeWithoutTheDocumentTypeAttribute>(id);
        }

        [TestMethod]
        public void CanGetDocumentForValidDocumentType()
        {
            const int id = 1234;
            const string url = "http://www.logikfabrik.se/umbraco/jet/test";
            const string name = "Test";
            var createDate = new DateTime(2015, 1, 1);
            var updateDate = new DateTime(2015, 2, 1);
            const string stringProperty = "StringProperty";
            const int integerProperty = 1;
            const decimal floatingDecimalProperty = 1.1m;
            const float floatingBinaryProperty = 1.1f;
            var dateTimeProperty = new DateTime(2015, 3, 1);
            const bool booleanProperty = true;
            const decimal floatingDecimalPropertyAsString = 1.1m;
            const float floatingBinaryPropertyAsString = 1.1f;

            var umbracoHelperWrapper = new Mock<IUmbracoHelperWrapper>();
            var typeService = new Mock<ITypeService>();

            // ReSharper disable once ImplicitlyCapturedClosure
            umbracoHelperWrapper.Setup(m => m.TypedDocument(id)).Returns(() =>
            {
                Func<string, object, IPublishedProperty> getProperty = (alias, value) =>
                {
                    var property = new Mock<IPublishedProperty>();

                    property.Setup(m => m.PropertyTypeAlias).Returns(alias);
                    property.Setup(m => m.Value).Returns(value);

                    return property.Object;
                };

                var content = new Mock<IPublishedContent>();

                content.Setup(m => m.Id).Returns(id);
                content.Setup(m => m.Url).Returns(url);
                content.Setup(m => m.Name).Returns(name);
                content.Setup(m => m.CreateDate).Returns(createDate);
                content.Setup(m => m.UpdateDate).Returns(updateDate);
                content.Setup(m => m.Properties).Returns(new[]
                {
                    getProperty("StringProperty".Alias(), stringProperty),
                    getProperty("IntegerProperty".Alias(), integerProperty),
                    getProperty("FloatingDecimalProperty".Alias(), floatingDecimalProperty),
                    getProperty("FloatingBinaryProperty".Alias(), floatingBinaryProperty),
                    getProperty("DateTimeProperty".Alias(), dateTimeProperty),
                    getProperty("BooleanProperty".Alias(), booleanProperty),
                    // Returned as string as the Umbraco data model has no explicit support for floating decimal point types.
                    getProperty("FloatingDecimalPropertyAsString", floatingDecimalPropertyAsString.ToString(CultureInfo.InvariantCulture)),
                    // Returned as string as the Umbraco data model has no explicit support for floating binary point types.
                    getProperty("FloatingBinaryPropertyAsString", floatingDecimalPropertyAsString.ToString(CultureInfo.InvariantCulture))
                });

                return content.Object;
            });

            typeService.Setup(m => m.DocumentTypes)
                .Returns(new[] { typeof(DocumentTypeWithTheDocumentTypeAttribute) });

            var documentService = new DocumentService(umbracoHelperWrapper.Object, typeService.Object);

            var document = documentService.GetDocument<DocumentTypeWithTheDocumentTypeAttribute>(id);

            Assert.AreEqual(id, document.Id);
            Assert.AreEqual(url, document.Url);
            Assert.AreEqual(name, document.Name);
            Assert.AreEqual(createDate, document.CreateDate);
            Assert.AreEqual(updateDate, document.UpdateDate);
            Assert.AreEqual(stringProperty, document.StringProperty);
            Assert.AreEqual(integerProperty, document.IntegerProperty);
            Assert.AreEqual(floatingDecimalProperty, document.FloatingDecimalProperty);
            Assert.AreEqual(floatingBinaryProperty, document.FloatingBinaryProperty);
            Assert.AreEqual(dateTimeProperty, document.DateTimeProperty);
            Assert.AreEqual(booleanProperty, document.BooleanProperty);
            Assert.AreEqual(floatingDecimalPropertyAsString, document.FloatingDecimalPropertyAsString);
            Assert.AreEqual(floatingBinaryPropertyAsString, document.FloatingBinaryPropertyAsString);
        }
    }
}
