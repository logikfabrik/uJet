﻿// <copyright file="DocumentServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Web.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using Jet.Web.Data;
    using Moq.AutoMock;
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DocumentServiceTest
    {
        [Theory]
        [CustomAutoData]
        public void CanGetContentIdByConvention(int id, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("Id", id.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.Id).Returns(id);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("Id").ShouldBe(id);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentUrlByConvention(int id, string url, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("Url", url.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.Url).Returns(url);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("Url").ShouldBe(url);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentNameByConvention(int id, string name, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("Name", name.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.Name).Returns(name);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("Name").ShouldBe(name);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentCreateDateByConvention(int id, DateTime createDate, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("CreateDate", createDate.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.CreateDate).Returns(createDate);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("CreateDate").ShouldBe(createDate);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentUpdateDateByConvention(int id, DateTime updateDate, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("UpdateDate", updateDate.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.UpdateDate).Returns(updateDate);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("UpdateDate").ShouldBe(updateDate);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentCreatorIdByConvention(int id, int creatorId, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("CreatorId", creatorId.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.CreatorId).Returns(creatorId);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("CreatorId").ShouldBe(creatorId);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentCreatorNameByConvention(int id, string creatorName, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("CreatorName", creatorName.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.CreatorName).Returns(creatorName);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("CreatorName").ShouldBe(creatorName);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentWriterIdByConvention(int id, int writerId, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("WriterId", writerId.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.WriterId).Returns(writerId);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("WriterId").ShouldBe(writerId);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentWriterNameByConvention(int id, string writerName, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("WriterName", writerName.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.WriterName).Returns(writerName);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("WriterName").ShouldBe(writerName);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentDocumentTypeIdByConvention(int id, int documentTypeId, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("DocumentTypeId", documentTypeId.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.DocumentTypeId).Returns(documentTypeId);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("DocumentTypeId").ShouldBe(documentTypeId);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentDocumentTypeAliasByConvention(int id, string documentTypeAlias, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty("DocumentTypeAlias", documentTypeAlias.GetType());

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.DocumentTypeAlias).Returns(documentTypeAlias);

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue("DocumentTypeAlias").ShouldBe(documentTypeAlias);
        }

        [Fact]
        public void CanNotGetContentForInvalidType()
        {
            var mocker = new AutoMocker();

            var service = mocker.CreateInstance<DocumentService>();

            Assert.Throws<ArgumentException>(() => service.GetContent(mocker.Get<IPublishedContent>(), typeof(object)));
        }

        [Theory]
        [CustomAutoData]
        public void CanGetContentForValidType(DocumentTypeModelTypeBuilder builder)
        {
            var mocker = new AutoMocker();

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(mocker.Get<IPublishedContent>(), builder.CreateType());

            content.ShouldNotBeNull();
        }

        [Theory]
        [ClassAutoData(typeof(CanGetContentForTypeWithPropertyClassData))]
        public void CanGetContentForTypeWithProperty(Type propertyType, object propertyValue, string propertyName, int id, DocumentTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicProperty(propertyName, propertyType);

            var mocker = new AutoMocker();

            var contentMock = mocker.GetMock<IPublishedContent>();

            contentMock.Setup(m => m.Properties).Returns(() =>
            {
                var property = mocker.GetMock<IPublishedProperty>();

                property.Setup(m => m.PropertyTypeAlias).Returns(propertyName);
                property.Setup(m => m.Value).Returns(propertyValue);

                return new[] { property.Object };
            });

            var umbracoHelperWrapperMock = mocker.GetMock<IUmbracoHelperWrapper>();

            umbracoHelperWrapperMock.Setup(m => m.TypedDocument(id)).Returns(contentMock.Object);

            var service = mocker.CreateInstance<DocumentService>();

            var content = service.GetContent(id, typeBuilder.CreateType());

            content.GetPropertyValue(propertyName).ShouldBe(propertyValue);
        }

        private class CanGetContentForTypeWithPropertyClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { typeof(string), "abc123" };
                yield return new object[] { typeof(int), 1 };
                yield return new object[] { typeof(float), 1.1f };
                yield return new object[] { typeof(decimal), 1.1m };
                yield return new object[] { typeof(bool), true };
                yield return new object[] { typeof(DateTime), DateTime.Now };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}