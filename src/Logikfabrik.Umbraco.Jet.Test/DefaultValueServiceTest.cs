// <copyright file="DefaultValueServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

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