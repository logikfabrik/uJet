// <copyright file="TypeRepositoryTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Data
{
    using System;
    using AutoFixture.Xunit2;
    using Jet.Data;
    using Moq;
    using Moq.AutoMock;
    using Xunit;

    public class TypeRepositoryTest
    {
        [Theory]
        [AutoData]
        public void CanGetContentTypeModelId(int contentTypeId)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var contentTypeRepositoryMock = mocker.GetMock<IContentTypeRepository>();

            typeRepository.GetContentTypeModelId(contentTypeId);

            contentTypeRepositoryMock.Verify(m => m.GetContentTypeModelId(contentTypeId), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetPropertyTypeModelId(int propertyTypeId)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var contentTypeRepositoryMock = mocker.GetMock<IContentTypeRepository>();

            typeRepository.GetPropertyTypeModelId(propertyTypeId);

            contentTypeRepositoryMock.Verify(m => m.GetPropertyTypeModelId(propertyTypeId), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetDefinitionTypeModelId(int definitionId)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var dataTypeRepositoryMock = mocker.GetMock<IDataTypeRepository>();

            typeRepository.GetDefinitionTypeModelId(definitionId);

            dataTypeRepositoryMock.Verify(m => m.GetDefinitionTypeModelId(definitionId), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetContentTypeId(Guid id)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var contentTypeRepositoryMock = mocker.GetMock<IContentTypeRepository>();

            typeRepository.GetContentTypeId(id);

            contentTypeRepositoryMock.Verify(m => m.GetContentTypeId(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetPropertyTypeId(Guid id)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var contentTypeRepositoryMock = mocker.GetMock<IContentTypeRepository>();

            typeRepository.GetPropertyTypeId(id);

            contentTypeRepositoryMock.Verify(m => m.GetPropertyTypeId(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetDefinitionId(Guid id)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var dataTypeRepositoryMock = mocker.GetMock<IDataTypeRepository>();

            typeRepository.GetDefinitionId(id);

            dataTypeRepositoryMock.Verify(m => m.GetDefinitionId(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanSetContentTypeId(Guid id, int contentTypeId)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var contentTypeRepositoryMock = mocker.GetMock<IContentTypeRepository>();

            typeRepository.SetContentTypeId(id, contentTypeId);

            contentTypeRepositoryMock.Verify(m => m.SetContentTypeId(id, contentTypeId), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanSetPropertyTypeId(Guid id, int propertyTypeId)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var contentTypeRepositoryMock = mocker.GetMock<IContentTypeRepository>();

            typeRepository.SetPropertyTypeId(id, propertyTypeId);

            contentTypeRepositoryMock.Verify(m => m.SetPropertyTypeId(id, propertyTypeId), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanSetDefinitionId(Guid id, int definitionId)
        {
            var mocker = new AutoMocker();

            var typeRepository = mocker.CreateInstance<TypeRepository>();

            var dataTypeRepositoryMock = mocker.GetMock<IDataTypeRepository>();

            typeRepository.SetDefinitionId(id, definitionId);

            dataTypeRepositoryMock.Verify(m => m.SetDefinitionId(id, definitionId), Times.Once);
        }
    }
}
