// <copyright file="DefaultValueServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel;
    using System.Reflection.Emit;
    using AutoFixture.Xunit2;
    using Logikfabrik.Umbraco.Jet.Extensions;
    using Moq;
    using Moq.AutoMock;
    using Utilities;
    using Xunit;

    public class DefaultValueServiceTest
    {
        [Theory]
        [InlineAutoData(typeof(string), "abc123")]
        [InlineAutoData(typeof(int), "1")]
        [InlineAutoData(typeof(float), "1.1")]
        [InlineAutoData(typeof(decimal), "1.1")]
        [InlineAutoData(typeof(bool), "true")]
        [InlineAutoData(typeof(DateTime), "2016-01-01T09:30:00Z")]
        public void CanSetPropertyDefaultValueForDocumentType(Type propertyType, string propertyDefaultValue, string propertyName, string typeName, string name)
        {
            var typeBuilder = new DocumentTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            // ReSharper disable once AssignNullToNotNullAttribute
            typeBuilder.AddPublicProperty(propertyName, propertyType, new[] { new CustomAttributeBuilder(typeof(DefaultValueAttribute).GetConstructor(new[] { typeof(Type), typeof(string) }), new object[] { propertyType, propertyDefaultValue }) });

            var modelType = typeBuilder.CreateType();

            var model = new DocumentType(modelType);

            var mocker = new AutoMocker();

            var contentType = mocker.Get<global::Umbraco.Core.Models.IContentType>();

            var contentMock = mocker.GetMock<global::Umbraco.Core.Models.IContent>();

            contentMock.Setup(m => m.ContentType).Returns(contentType);

            var documentTypeModelFinderMock = mocker.GetMock<IContentTypeModelFinder<DocumentType, DocumentTypeAttribute, global::Umbraco.Core.Models.IContentType>>();

            documentTypeModelFinderMock.Setup(m => m.Find(contentType, It.IsAny<DocumentType[]>())).Returns(new[] { model });

            var defaultValueService = mocker.CreateInstance<DefaultValueService>();

            defaultValueService.SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), It.Is<object>(value => value.ToString() == new DefaultValueAttribute(propertyType, propertyDefaultValue).Value.ToString())), Times.Once);
        }

        [Theory]
        [InlineAutoData(typeof(string), "abc123")]
        [InlineAutoData(typeof(int), "1")]
        [InlineAutoData(typeof(float), "1.1")]
        [InlineAutoData(typeof(decimal), "1.1")]
        [InlineAutoData(typeof(bool), "true")]
        [InlineAutoData(typeof(DateTime), "2016-01-01T09:30:00Z")]
        public void CanSetPropertyDefaultValueForMediaType(Type propertyType, string propertyDefaultValue, string propertyName, string typeName, string name)
        {
            var typeBuilder = new MediaTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            // ReSharper disable once AssignNullToNotNullAttribute
            typeBuilder.AddPublicProperty(propertyName, propertyType, new[] { new CustomAttributeBuilder(typeof(DefaultValueAttribute).GetConstructor(new[] { typeof(Type), typeof(string) }), new object[] { propertyType, propertyDefaultValue }) });

            var modelType = typeBuilder.CreateType();

            var model = new MediaType(modelType);

            var mocker = new AutoMocker();

            var contentType = mocker.Get<global::Umbraco.Core.Models.IMediaType>();

            var contentMock = mocker.GetMock<global::Umbraco.Core.Models.IMedia>();

            contentMock.Setup(m => m.ContentType).Returns(contentType);

            var documentTypeModelFinderMock = mocker.GetMock<IContentTypeModelFinder<MediaType, MediaTypeAttribute, global::Umbraco.Core.Models.IMediaType>>();

            documentTypeModelFinderMock.Setup(m => m.Find(contentType, It.IsAny<MediaType[]>())).Returns(new[] { model });

            var defaultValueService = mocker.CreateInstance<DefaultValueService>();

            defaultValueService.SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), It.Is<object>(value => value.ToString() == new DefaultValueAttribute(propertyType, propertyDefaultValue).Value.ToString())), Times.Once);
        }

        [Theory]
        [InlineAutoData(typeof(string), "abc123")]
        [InlineAutoData(typeof(int), "1")]
        [InlineAutoData(typeof(float), "1.1")]
        [InlineAutoData(typeof(decimal), "1.1")]
        [InlineAutoData(typeof(bool), "true")]
        [InlineAutoData(typeof(DateTime), "2016-01-01T09:30:00Z")]
        public void CanSetPropertyDefaultValueForMemberType(Type propertyType, string propertyDefaultValue, string propertyName, string typeName, string name)
        {
            var typeBuilder = new MemberTypeModelTypeBuilder(typeName, name).GetTypeBuilder();

            // ReSharper disable once AssignNullToNotNullAttribute
            typeBuilder.AddPublicProperty(propertyName, propertyType, new[] { new CustomAttributeBuilder(typeof(DefaultValueAttribute).GetConstructor(new[] { typeof(Type), typeof(string) }), new object[] { propertyType, propertyDefaultValue }) });

            var modelType = typeBuilder.CreateType();

            var model = new MemberType(modelType);

            var mocker = new AutoMocker();

            var contentType = mocker.Get<global::Umbraco.Core.Models.IMemberType>();

            var contentMock = mocker.GetMock<global::Umbraco.Core.Models.IMember>();

            contentMock.Setup(m => m.ContentType).Returns(contentType);

            var documentTypeModelFinderMock = mocker.GetMock<IContentTypeModelFinder<MemberType, MemberTypeAttribute, global::Umbraco.Core.Models.IMemberType>>();

            documentTypeModelFinderMock.Setup(m => m.Find(contentType, It.IsAny<MemberType[]>())).Returns(new[] { model });

            var defaultValueService = mocker.CreateInstance<DefaultValueService>();

            defaultValueService.SetDefaultValues(contentMock.Object);

            contentMock.Verify(m => m.SetValue(propertyName.Alias(), It.Is<object>(value => value.ToString() == new DefaultValueAttribute(propertyType, propertyDefaultValue).Value.ToString())), Times.Once);
        }
    }
}