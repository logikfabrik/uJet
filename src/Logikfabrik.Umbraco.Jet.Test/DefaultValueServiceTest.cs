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

    [TestClass]
    public class DefaultValueServiceTest : TestBase
    {
        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForStringProperty()
        {
            var documentType = new Models.DocumentType();
            var propertyName = GetPropertyName(() => documentType.StringProperty);

            CanSetDefaultValueForDocumentType<string>(documentType, propertyName);
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForIntegerProperty()
        {
            var documentType = new Models.DocumentType();
            var propertyName = GetPropertyName(() => documentType.IntegerProperty);

            CanSetDefaultValueForDocumentType<int>(documentType, propertyName);
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForFloatingBinaryPointProperty()
        {
            var documentType = new Models.DocumentType();
            var propertyName = GetPropertyName(() => documentType.FloatingBinaryPointProperty);

            CanSetDefaultValueForDocumentType<float>(documentType, propertyName);
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForFloatingDecimalPointProperty()
        {
            var documentType = new Models.DocumentType();
            var propertyName = GetPropertyName(() => documentType.FloatingDecimalPointProperty);

            CanSetDefaultValueForDocumentType<decimal>(documentType, propertyName);
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForBooleanProperty()
        {
            var documentType = new Models.DocumentType();
            var propertyName = GetPropertyName(() => documentType.BooleanProperty);

            CanSetDefaultValueForDocumentType<bool>(documentType, propertyName);
        }

        [TestMethod]
        public void CanSetDocumentTypeDefaultValueForDateTimeProperty()
        {
            var documentType = new Models.DocumentType();
            var propertyName = GetPropertyName(() => documentType.DateTimeProperty);

            CanSetDefaultValueForDocumentType<DateTime>(documentType, propertyName);
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForStringProperty()
        {
            var mediaType = new Models.MediaType();
            var propertyName = GetPropertyName(() => mediaType.StringProperty);

            CanSetDefaultValueForMediaType<string>(mediaType, propertyName);
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForIntegerProperty()
        {
            var mediaType = new Models.MediaType();
            var propertyName = GetPropertyName(() => mediaType.IntegerProperty);

            CanSetDefaultValueForMediaType<int>(mediaType, propertyName);
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForFloatingBinaryPointProperty()
        {
            var mediaType = new Models.MediaType();
            var propertyName = GetPropertyName(() => mediaType.FloatingBinaryPointProperty);

            CanSetDefaultValueForMediaType<float>(mediaType, propertyName);
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForFloatingDecimalPointProperty()
        {
            var mediaType = new Models.MediaType();
            var propertyName = GetPropertyName(() => mediaType.FloatingDecimalPointProperty);

            CanSetDefaultValueForMediaType<decimal>(mediaType, propertyName);
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForDateTimeProperty()
        {
            var mediaType = new Models.MediaType();
            var propertyName = GetPropertyName(() => mediaType.DateTimeProperty);

            CanSetDefaultValueForMediaType<DateTime>(mediaType, propertyName);
        }

        [TestMethod]
        public void CanSetMediaTypeDefaultValueForBooleanProperty()
        {
            var mediaType = new Models.MediaType();
            var propertyName = GetPropertyName(() => mediaType.BooleanProperty);

            CanSetDefaultValueForMediaType<bool>(mediaType, propertyName);
        }

        [TestMethod]
        public void CanSetMemberTypeDefaultValueForStringProperty()
        {
            var memberType = new Models.MemberType();
            var propertyName = GetPropertyName(() => memberType.StringProperty);

            CanSetDefaultValueForMemberType<string>(memberType, propertyName);
        }

        [TestMethod]
        public void CanSetMemberTypeDefaultValueForIntegerProperty()
        {
            var memberType = new Models.MemberType();
            var propertyName = GetPropertyName(() => memberType.IntegerProperty);

            CanSetDefaultValueForMemberType<int>(memberType, propertyName);
        }

        [TestMethod]
        public void CanSetMemberTypeDefaultValueForFloatingBinaryPointProperty()
        {
            var memberType = new Models.MemberType();
            var propertyName = GetPropertyName(() => memberType.FloatingBinaryPointProperty);

            CanSetDefaultValueForMemberType<float>(memberType, propertyName);
        }

        [TestMethod]
        public void CanSetMemberTypeDefaultValueForFloatingDecimalPointProperty()
        {
            var memberType = new Models.MemberType();
            var propertyName = GetPropertyName(() => memberType.FloatingDecimalPointProperty);

            CanSetDefaultValueForMemberType<decimal>(memberType, propertyName);
        }

        [TestMethod]
        public void CanSetMemberTypeDefaultValueForBooleanProperty()
        {
            var memberType = new Models.MemberType();
            var propertyName = GetPropertyName(() => memberType.BooleanProperty);

            CanSetDefaultValueForMemberType<bool>(memberType, propertyName);
        }

        [TestMethod]
        public void CanSetMemberTypeDefaultValueForDateTimeProperty()
        {
            var memberType = new Models.MemberType();
            var propertyName = GetPropertyName(() => memberType.DateTimeProperty);

            CanSetDefaultValueForMemberType<DateTime>(memberType, propertyName);
        }

        private static void CanSetDefaultValueForDocumentType<TPropertyType>(Models.DocumentType documentType, string propertyName)
        {
            var contentTypeMock = new Mock<IContentType>();

            contentTypeMock.Setup(m => m.Alias).Returns(documentType.GetType().Name.Alias());

            var contentMock = new Mock<IContent>();

            contentMock.Setup(m => m.ContentType).Returns(contentTypeMock.Object);

            new DefaultValueService(GetTypeServiceMock(documentType.GetType()).Object).SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), GetPropertyDefaultValue<TPropertyType>(documentType.GetType(), propertyName)));
        }

        private static void CanSetDefaultValueForMediaType<TPropertyType>(Models.MediaType mediaType, string propertyName)
        {
            var contentTypeMock = new Mock<IMediaType>();

            contentTypeMock.Setup(m => m.Alias).Returns(mediaType.GetType().Name.Alias());

            var contentMock = new Mock<IMedia>();

            contentMock.Setup(m => m.ContentType).Returns(contentTypeMock.Object);

            new DefaultValueService(GetTypeServiceMock(mediaType.GetType()).Object).SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), GetPropertyDefaultValue<TPropertyType>(mediaType.GetType(), propertyName)));
        }

        private static void CanSetDefaultValueForMemberType<TPropertyType>(Models.MemberType memberType, string propertyName)
        {
            var contentTypeMock = new Mock<IMemberType>();

            contentTypeMock.Setup(m => m.Alias).Returns(memberType.GetType().Name.Alias());

            var contentMock = new Mock<IMember>();

            contentMock.Setup(m => m.ContentType).Returns(contentTypeMock.Object);

            new DefaultValueService(GetTypeServiceMock(memberType.GetType()).Object).SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), GetPropertyDefaultValue<TPropertyType>(memberType.GetType(), propertyName)));
        }

        private static T GetPropertyDefaultValue<T>(Type type, string propertyName)
        {
            var property = type.GetProperties().Single(p => p.Name == propertyName);

            var attribute = (DefaultValueAttribute)Attribute.GetCustomAttribute(property, typeof(DefaultValueAttribute));

            return (T)attribute.Value;
        }
    }
}