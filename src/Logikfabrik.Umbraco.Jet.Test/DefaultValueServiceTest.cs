// <copyright file="DefaultValueServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using global::Umbraco.Core.Models;
    using Jet.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The <see cref="DefaultValueServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class DefaultValueServiceTest : TestBase
    {
        /// <summary>
        /// Test to set default value for a document type string property.
        /// </summary>
        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForStringProperty()
        {
            var documentType = new DocumentType();
            var propertyName = GetPropertyName(() => documentType.StringProperty);

            CanSetDefaultValueForDocumentType<string>(documentType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a document type integer property.
        /// </summary>
        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForIntegerProperty()
        {
            var documentType = new DocumentType();
            var propertyName = GetPropertyName(() => documentType.IntegerProperty);

            CanSetDefaultValueForDocumentType<int>(documentType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a document type floating binary point property.
        /// </summary>
        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForFloatingBinaryPointProperty()
        {
            var documentType = new DocumentType();
            var propertyName = GetPropertyName(() => documentType.FloatingBinaryPointProperty);

            CanSetDefaultValueForDocumentType<float>(documentType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a document type floating decimal point property.
        /// </summary>
        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForFloatingDecimalPointProperty()
        {
            var documentType = new DocumentType();
            var propertyName = GetPropertyName(() => documentType.FloatingDecimalPointProperty);

            CanSetDefaultValueForDocumentType<decimal>(documentType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a document type boolean property.
        /// </summary>
        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForBooleanProperty()
        {
            var documentType = new DocumentType();
            var propertyName = GetPropertyName(() => documentType.BooleanProperty);

            CanSetDefaultValueForDocumentType<bool>(documentType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a media type string property.
        /// </summary>
        [TestMethod]
        public void CanSetMediaTypeDefaultValueForStringProperty()
        {
            var mediaType = new MediaType();
            var propertyName = GetPropertyName(() => mediaType.StringProperty);

            CanSetDefaultValueForMediaType<string>(mediaType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a media type integer property.
        /// </summary>
        [TestMethod]
        public void CanSetMediaTypeDefaultValueForIntegerProperty()
        {
            var mediaType = new MediaType();
            var propertyName = GetPropertyName(() => mediaType.IntegerProperty);

            CanSetDefaultValueForMediaType<int>(mediaType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a media type floating binary point property.
        /// </summary>
        [TestMethod]
        public void CanSetMediaTypeDefaultValueForFloatingBinaryPointProperty()
        {
            var mediaType = new MediaType();
            var propertyName = GetPropertyName(() => mediaType.FloatingBinaryPointProperty);

            CanSetDefaultValueForMediaType<float>(mediaType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a media type floating decimal point property.
        /// </summary>
        [TestMethod]
        public void CanSetMediaTypeDefaultValueForFloatingDecimalPointProperty()
        {
            var mediaType = new MediaType();
            var propertyName = GetPropertyName(() => mediaType.FloatingDecimalPointProperty);

            CanSetDefaultValueForMediaType<decimal>(mediaType, propertyName);
        }

        /// <summary>
        /// Test to set default value for a media type boolean property.
        /// </summary>
        [TestMethod]
        public void CanSetMediaTypeDefaultValueForBooleanProperty()
        {
            var mediaType = new MediaType();
            var propertyName = GetPropertyName(() => mediaType.BooleanProperty);

            CanSetDefaultValueForMediaType<bool>(mediaType, propertyName);
        }

        private static void CanSetDefaultValueForDocumentType<TPropertyType>(ContentType documentType, string propertyName)
        {
            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.Setup(m => m.Alias).Returns(documentType.GetType().Name.Alias());

            var contentMock = new Mock<IContent>();

            contentMock.Setup(m => m.ContentType).Returns(contentTypeMock.Object);

            new DefaultValueService(GetTypeServiceMock(documentType.GetType()).Object).SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), GetPropertyDefaultValue<TPropertyType>(documentType.GetType(), propertyName)));
        }

        private static void CanSetDefaultValueForMediaType<TPropertyType>(ContentType mediaType, string propertyName)
        {
            var contentTypeMock = new Mock<IMediaType>();

            contentTypeMock.Setup(m => m.Alias).Returns(mediaType.GetType().Name.Alias());

            var contentMock = new Mock<IMedia>();

            contentMock.Setup(m => m.ContentType).Returns(contentTypeMock.Object);

            new DefaultValueService(GetTypeServiceMock(mediaType.GetType()).Object).SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), GetPropertyDefaultValue<TPropertyType>(mediaType.GetType(), propertyName)));
        }

        private static T GetPropertyDefaultValue<T>(Type type, string propertyName)
        {
            var property = type.GetProperties().Single(p => p.Name == propertyName);

            var attribute = (DefaultValueAttribute)Attribute.GetCustomAttribute(property, typeof(DefaultValueAttribute));

            return (T)attribute.Value;
        }

        /// <summary>
        /// The <see cref="ContentType" /> class.
        /// </summary>
        protected abstract class ContentType
        {
            /// <summary>
            /// Gets or sets the string property value.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            [DefaultValue("Default")]
            public string StringProperty { get; set; }

            /// <summary>
            /// Gets or sets the integer property value.
            /// </summary>
            /// <value>
            /// The integer property value.
            /// </value>
            [DefaultValue(1)]
            public int IntegerProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating binary point property value.
            /// </summary>
            /// <value>
            /// The floating binary point property value.
            /// </value>
            [DefaultValue(1.1f)]
            public float FloatingBinaryPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating decimal point property value.
            /// </summary>
            /// <value>
            /// The floating decimal point property value.
            /// </value>
            [DefaultValue(typeof(decimal), "1.1")]
            public decimal FloatingDecimalPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the boolean property value.
            /// </summary>
            /// <value>
            /// The boolean property value.
            /// </value>
            [DefaultValue(true)]
            public bool BooleanProperty { get; set; }
        }

        /// <summary>
        /// The <see cref="DocumentType" /> class.
        /// </summary>
        [DocumentType("DocumentType")]
        protected class DocumentType : ContentType
        {
        }

        /// <summary>
        /// The <see cref="MediaType" /> class.
        /// </summary>
        [MediaType("MediaType")]
        protected class MediaType : ContentType
        {
        }
    }
}