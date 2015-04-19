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

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System.ComponentModel;
    using Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using global::Umbraco.Core.Models;

    [TestClass]
    public class DefaultValueServiceTest
    {
        #region Document type

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForStringProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IContent>();
            var contentType = new Moq.Mock<IContentType>();

            content.Setup(m => m.GetValue("stringProperty")).Returns(null);
            content.Setup(m => m.SetValue("stringProperty", "Default")).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestDocumentType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForIntegerProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IContent>();
            var contentType = new Moq.Mock<IContentType>();

            content.Setup(m => m.GetValue("integerProperty")).Returns(null);
            content.Setup(m => m.SetValue("integerProperty", 1)).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestDocumentType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForFloatingBinaryProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IContent>();
            var contentType = new Moq.Mock<IContentType>();

            content.Setup(m => m.GetValue("floatingBinaryProperty")).Returns(null);
            content.Setup(m => m.SetValue("floatingBinaryProperty", 1.1f)).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestDocumentType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForBooleanProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IContent>();
            var contentType = new Moq.Mock<IContentType>();

            content.Setup(m => m.GetValue("booleanProperty")).Returns(null);
            content.Setup(m => m.SetValue("booleanProperty", true)).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestDocumentType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }
        
        #endregion

        #region Media type

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForStringProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IMedia>();
            var contentType = new Moq.Mock<IMediaType>();

            content.Setup(m => m.GetValue("stringProperty")).Returns(null);
            content.Setup(m => m.SetValue("stringProperty", "Default")).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestMediaType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForIntegerProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IMedia>();
            var contentType = new Moq.Mock<IMediaType>();

            content.Setup(m => m.GetValue("integerProperty")).Returns(null);
            content.Setup(m => m.SetValue("integerProperty", 1)).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestMediaType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForFloatingBinaryProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IMedia>();
            var contentType = new Moq.Mock<IMediaType>();

            content.Setup(m => m.GetValue("floatingBinaryProperty")).Returns(null);
            content.Setup(m => m.SetValue("floatingBinaryProperty", 1.1f)).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestMediaType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForBooleanProperty()
        {
            var service = new DefaultValueService();
            var content = new Moq.Mock<IMedia>();
            var contentType = new Moq.Mock<IMediaType>();

            content.Setup(m => m.GetValue("booleanProperty")).Returns(null);
            content.Setup(m => m.SetValue("booleanProperty", true)).Verifiable();
            content.Setup(m => m.ContentType).Returns(contentType.Object);
            contentType.Setup(m => m.Alias).Returns(typeof(DefaultValueServiceTestMediaType).Name.Alias());

            service.SetDefaultValues(content.Object);

            content.VerifyAll();
        }
        
        #endregion

        [DocumentType("DefaultValueServiceTestDocumentType")]
        public class DefaultValueServiceTestDocumentType
        {
            [DefaultValue("Default")]
            public string StringProperty { get; set; }

            [DefaultValue(1)]
            public int IntegerProperty { get; set; }

            [DefaultValue(1.1f)]
            public float FloatingBinaryProperty { get; set; }

            [DefaultValue(true)]
            public bool BooleanProperty { get; set; }
        }

        [MediaType("DefaultValueServiceTestMediaType")]
        public class DefaultValueServiceTestMediaType
        {
            [DefaultValue("Default")]
            public string StringProperty { get; set; }

            [DefaultValue(1)]
            public int IntegerProperty { get; set; }

            [DefaultValue(1.1f)]
            public float FloatingBinaryProperty { get; set; }

            [DefaultValue(true)]
            public bool BooleanProperty { get; set; }
        }
    }
}