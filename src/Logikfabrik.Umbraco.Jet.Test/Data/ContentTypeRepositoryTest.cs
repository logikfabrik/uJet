// <copyright file="ContentTypeRepositoryTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Data
{
    using System;
    using AutoFixture.Xunit2;
    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.SqlSyntax;
    using Jet.Data;
    using Moq;
    using Moq.AutoMock;
    using Shouldly;
    using Xunit;

    public class ContentTypeRepositoryTest
    {
        [Theory]
        [AutoData]
        public void CanGetContentTypeModelId(int contentTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<ContentType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<ContentType>(It.IsAny<Sql>())).Returns(new ContentType());

            contentTypeRepository.GetContentTypeModelId(contentTypeId).ShouldNotBeNull();
        }

        [Theory]
        [AutoData]
        public void CanGetContentTypeModelIdFromCache(int contentTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<ContentType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<ContentType>(It.IsAny<Sql>())).Returns(new ContentType());

            contentTypeRepository.GetContentTypeModelId(contentTypeId);
            contentTypeRepository.GetContentTypeModelId(contentTypeId);

            databaseWrapperMock.Verify(m => m.Get<ContentType>(It.IsAny<Sql>()), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetPropertyTypeModelId(int propertyTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<PropertyType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<PropertyType>(It.IsAny<Sql>())).Returns(new PropertyType());

            contentTypeRepository.GetPropertyTypeModelId(propertyTypeId).ShouldNotBeNull();
        }

        [Theory]
        [AutoData]
        public void CanGetPropertyTypeModelIdFromCache(int propertyTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<PropertyType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<PropertyType>(It.IsAny<Sql>())).Returns(new PropertyType());

            contentTypeRepository.GetPropertyTypeModelId(propertyTypeId);
            contentTypeRepository.GetPropertyTypeModelId(propertyTypeId);

            databaseWrapperMock.Verify(m => m.Get<PropertyType>(It.IsAny<Sql>()), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetContentTypeId(Guid id, int contentTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<ContentType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<ContentType>(id)).Returns(new ContentType { ContentTypeId = contentTypeId });

            contentTypeRepository.GetContentTypeId(id).ShouldBe(contentTypeId);
        }

        [Theory]
        [AutoData]
        public void CanGetContentTypeIdFromCache(Guid id, int contentTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<ContentType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<ContentType>(id)).Returns(new ContentType { ContentTypeId = contentTypeId });

            contentTypeRepository.GetContentTypeId(id);
            contentTypeRepository.GetContentTypeId(id);

            databaseWrapperMock.Verify(m => m.Get<ContentType>(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetPropertyTypeId(Guid id, int propertyTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<PropertyType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<PropertyType>(id)).Returns(new PropertyType { PropertyTypeId = propertyTypeId });

            contentTypeRepository.GetPropertyTypeId(id).ShouldBe(propertyTypeId);
        }

        [Theory]
        [AutoData]
        public void CanGetPropertyTypeIdFromCache(Guid id, int propertyTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<PropertyType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<PropertyType>(id)).Returns(new PropertyType { PropertyTypeId = propertyTypeId });

            contentTypeRepository.GetPropertyTypeId(id);
            contentTypeRepository.GetPropertyTypeId(id);

            databaseWrapperMock.Verify(m => m.Get<PropertyType>(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanSetContentTypeId(Guid id, int contentTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            contentTypeRepository.SetContentTypeId(id, contentTypeId);

            databaseWrapperMock.Verify(m => m.Insert(It.IsAny<ContentType>(), id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanSetPropertyTypeId(Guid id, int propertyTypeId)
        {
            var mocker = new AutoMocker();

            var contentTypeRepository = mocker.CreateInstance<ContentTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            contentTypeRepository.SetPropertyTypeId(id, propertyTypeId);

            databaseWrapperMock.Verify(m => m.Insert(It.IsAny<PropertyType>(), id), Times.Once);
        }
    }
}