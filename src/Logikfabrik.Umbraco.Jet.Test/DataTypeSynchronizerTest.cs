// <copyright file="DataTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Logikfabrik.Umbraco.Jet.Data;
    using Moq;
    using Moq.AutoMock;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DataTypeSynchronizerTest
    {
        [Theory]
        [CustomAutoData]
        public void CanCreateModelWithoutId(Jet.DataType model)
        {
            var mocker = new AutoMocker();

            var dataTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock
                .Setup(m => m.SaveDataTypeAndPreValues(It.Is<IDataTypeDefinition>(dataTypeDefinition => dataTypeDefinition.Id == 0), It.IsAny<IDictionary<string, PreValue>>(), 0))
                .Callback((IDataTypeDefinition dataTypeDefinition, IDictionary<string, PreValue> values, int userId) =>
                {
                    dataTypeServiceMock
                        .Setup(m => m.GetDataTypeDefinitionByName(dataTypeDefinition.Name))
                        .Returns(dataTypeDefinition)
                        .Verifiable();
                })
                .Verifiable();

            dataTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanCreateModelWithId(string typeName, Guid id, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id, type, editor).Create(Scope.Public);

            var model = new Jet.DataType(modelType);

            var mocker = new AutoMocker();

            var dataTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock
                .Setup(m => m.SaveDataTypeAndPreValues(It.Is<IDataTypeDefinition>(dataTypeDefinition => dataTypeDefinition.Id == 0), It.IsAny<IDictionary<string, PreValue>>(), 0))
                .Callback((IDataTypeDefinition dataTypeDefinition, IDictionary<string, PreValue> values, int userId) =>
                {
                    dataTypeServiceMock
                        .Setup(m => m.GetDataTypeDefinitionByName(dataTypeDefinition.Name))
                        .Returns(dataTypeDefinition)
                        .Verifiable();

                    var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

                    typeRepositoryMock.Setup(m => m.SetDefinitionId(id, dataTypeDefinition.Id)).Verifiable();
                })
                .Verifiable();

            dataTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithoutId(Jet.DataType model, IDataTypeDefinition dataTypeDefinition)
        {
            var mocker = new AutoMocker();

            var dataTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            var dataTypeDefinitionFinderMock = mocker.GetMock<IDataTypeDefinitionFinder>();

            dataTypeDefinitionFinderMock.Setup(m => m.Find(model, It.IsAny<IDataTypeDefinition[]>())).Returns(new[] { dataTypeDefinition });

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.Save(dataTypeDefinition, 0)).Verifiable();

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(model.Name)).Returns(dataTypeDefinition).Verifiable();

            dataTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithId(string typeName, Guid id, Type type, string editor, IDataTypeDefinition dataTypeDefinition)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id, type, editor).Create(Scope.Public);

            var model = new Jet.DataType(modelType);

            var mocker = new AutoMocker();

            var dataTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            var dataTypeDefinitionFinderMock = mocker.GetMock<IDataTypeDefinitionFinder>();

            dataTypeDefinitionFinderMock.Setup(m => m.Find(model, It.IsAny<IDataTypeDefinition[]>())).Returns(new[] { dataTypeDefinition });

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.Save(dataTypeDefinition, 0)).Verifiable();

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(model.Name)).Returns(dataTypeDefinition).Verifiable();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.SetDefinitionId(id, dataTypeDefinition.Id)).Verifiable();

            dataTypeSynchronizer.Run();

            mocker.VerifyAll();
        }
    }
}
