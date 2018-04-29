// <copyright file="DataTypeRepositoryTest.cs" company="Logikfabrik">
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

    public class DataTypeRepositoryTest
    {
        [Theory]
        [AutoData]
        public void CanGetDefinitionTypeModelId(int definitionId)
        {
            var mocker = new AutoMocker();

            var dataTypeRepository = mocker.CreateInstance<DataTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<DataType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<DataType>(It.IsAny<Sql>())).Returns(new DataType());

            dataTypeRepository.GetDefinitionTypeModelId(definitionId).ShouldNotBeNull();
        }

        [Theory]
        [AutoData]
        public void CanGetDefinitionTypeModelIdFromCache(int definitionId)
        {
            var mocker = new AutoMocker();

            var dataTypeRepository = mocker.CreateInstance<DataTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<DataType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<DataType>(It.IsAny<Sql>())).Returns(new DataType());

            dataTypeRepository.GetDefinitionTypeModelId(definitionId);
            dataTypeRepository.GetDefinitionTypeModelId(definitionId);

            databaseWrapperMock.Verify(m => m.Get<DataType>(It.IsAny<Sql>()), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanGetDefinitionId(Guid id, int definitionId)
        {
            var mocker = new AutoMocker();

            var dataTypeRepository = mocker.CreateInstance<DataTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<DataType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<DataType>(id)).Returns(new DataType { DefinitionId = definitionId });

            dataTypeRepository.GetDefinitionId(id).ShouldBe(definitionId);
        }

        [Theory]
        [AutoData]
        public void CanGetDefinitionIdFromCache(Guid id, int definitionId)
        {
            var mocker = new AutoMocker();

            var dataTypeRepository = mocker.CreateInstance<DataTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            databaseWrapperMock.Setup(m => m.SyntaxProvider).Returns(mocker.Get<ISqlSyntaxProvider>());
            databaseWrapperMock.Setup(m => m.TableExists<DataType>()).Returns(true);
            databaseWrapperMock.Setup(m => m.Get<DataType>(id)).Returns(new DataType { DefinitionId = definitionId });

            dataTypeRepository.GetDefinitionId(id);
            dataTypeRepository.GetDefinitionId(id);

            databaseWrapperMock.Verify(m => m.Get<DataType>(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void CanSetDefinitionId(Guid id, int definitionId)
        {
            var mocker = new AutoMocker();

            var dataTypeRepository = mocker.CreateInstance<DataTypeRepository>();

            var databaseWrapperMock = mocker.GetMock<IDatabaseWrapper>();

            dataTypeRepository.SetDefinitionId(id, definitionId);

            databaseWrapperMock.Verify(m => m.Insert(It.IsAny<DataType>(), id), Times.Once);
        }
    }
}