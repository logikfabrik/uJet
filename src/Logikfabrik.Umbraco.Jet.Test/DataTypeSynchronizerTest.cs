// <copyright file="DataTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using AutoFixture.Xunit2;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Moq;
    using Moq.AutoMock;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DataTypeSynchronizerTest
    {
        [Theory]
        [AutoData]
        public void CanCreateModelWithoutId(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new DataType(modelType);

            CanCreateModel(model);
        }

        [Theory]
        [AutoData]
        public void CanCreateModelWithId(string typeName, Guid id, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id, type, editor).CreateType();

            var model = new DataType(modelType);

            CanCreateModel(model);
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithoutId(string typeName, Type type, string editor, IDataTypeDefinition definition)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new DataType(modelType);

            CanUpdateModel(model, definition);
        }

        [Theory]
        [CustomAutoData]
        public void CanUpdateModelWithId(string typeName, Guid id, Type type, string editor, IDataTypeDefinition definition)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id, type, editor).CreateType();

            var model = new DataType(modelType);

            CanUpdateModel(model, definition);
        }

        private static void CanCreateModel(DataType model)
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

        private static void CanUpdateModel(DataType model, IDataTypeDefinition definition)
        {
            var mocker = new AutoMocker();

            var dataTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            var dataTypeDefinitionFinderMock = mocker.GetMock<IDataTypeDefinitionFinder>();

            dataTypeDefinitionFinderMock.Setup(m => m.Find(model, It.IsAny<IDataTypeDefinition[]>())).Returns(new[] { definition });

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.Save(definition, 0)).Verifiable();

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(model.Name)).Returns(definition).Verifiable();

            dataTypeSynchronizer.Run();

            mocker.VerifyAll();
        }
    }
}
