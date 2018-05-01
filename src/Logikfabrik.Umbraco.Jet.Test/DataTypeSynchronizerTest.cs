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
    using Logikfabrik.Umbraco.Jet.Data;
    using Moq;
    using Moq.AutoMock;
    using Utilities;
    using Xunit;

    public class DataTypeSynchronizerTest
    {
        [Theory]
        [AutoData]
        public void CanCreateModelWithoutId(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new Jet.DataType(modelType);

            var mocker = new AutoMocker();

            var dataTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            dataTypeSynchronizer.Run();

            // TODO: Verify stuff.
        }

        [Theory]
        [AutoData]
        public void CanCreateModelWithId(string typeName, Guid id, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id.ToString(), type, editor).CreateType();

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
                        .Setup(m => m.GetDataTypeDefinitionByName(It.Is<string>(name => name == dataTypeDefinition.Name)))
                        .Returns(dataTypeDefinition)
                        .Verifiable();

                    var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

                    typeRepositoryMock
                        .Setup(m => m.SetDefinitionId(id, It.Is<int>(definitionId => definitionId == dataTypeDefinition.Id)))
                        .Verifiable();
                })
                .Verifiable();

            dataTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [AutoData]
        public void CanUpdateModelWithoutId(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new Jet.DataType(modelType);

            var mocker = new AutoMocker();

            var mediaTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            var dataTypeDefinitionMock = mocker.GetMock<IDataTypeDefinition>();

            dataTypeDefinitionMock.Setup(m => m.Name).Returns(model.Name);

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock
                .Setup(m => m.GetAllDataTypeDefinitions())
                .Returns(new[] { dataTypeDefinitionMock.Object });

            dataTypeServiceMock.Setup(m => m.Save(dataTypeDefinitionMock.Object, 0)).Verifiable();

            mediaTypeSynchronizer.Run();

            mocker.VerifyAll();
        }

        [Theory]
        [AutoData]
        public void CanUpdateModelWithId(string typeName, Guid id, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id.ToString(), type, editor).CreateType();

            var model = new Jet.DataType(modelType);

            var mocker = new AutoMocker();

            var dataTypeSynchronizer = mocker.CreateInstance<DataTypeSynchronizer>();

            var typeResolverMock = mocker.GetMock<ITypeResolver>();

            typeResolverMock
                .Setup(m => m.DataTypes)
                .Returns(new[] { model });

            var dataTypeDefinitionMock = mocker.GetMock<IDataTypeDefinition>();

            dataTypeDefinitionMock.Setup(m => m.Name).Returns(model.Name);

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.Save(dataTypeDefinitionMock.Object, 0)).Verifiable();

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeDefinitionMock.Object.Name)).Returns(dataTypeDefinitionMock.Object).Verifiable();
            
            dataTypeSynchronizer.Run();

            mocker.VerifyAll();
        }





        //[Fact]
        //public void CanUpdateDataTypeWithId()
        //{
        //    var dataTypeWithId = new DataType(typeof(DataTypeWithId));

        //    var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

        //    dataTypeDefinitionWithIdMock.SetupAllProperties();

        //    var typeResolverMock = new Mock<ITypeResolver>();

        //    typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId }));

        //    var dataTypeServiceMock = new Mock<IDataTypeService>();

        //    dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
        //    dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);
        //    dataTypeServiceMock.Setup(m => m.GetPreValuesCollectionByDataTypeId(It.IsAny<int>())).Returns(new PreValueCollection(new Dictionary<string, PreValue>()));

        //    var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

        //    typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

        //    var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizer>(
        //        dataTypeServiceMock.Object,
        //        typeResolverMock.Object,
        //        typeRepositoryMock.Object)
        //    { CallBase = true };

        //    dataTypeSynchronizationServiceMock.Object.Run();

        //    dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataTypeDefinition(dataTypeDefinitionWithIdMock.Object, dataTypeWithId), Times.Once);
        //}

        //[Fact]
        //public void CanUpdateNameForDataTypeWithId()
        //{
        //    var dataTypeWithId = new DataType(typeof(DataTypeWithId));

        //    var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

        //    dataTypeDefinitionWithIdMock.SetupAllProperties();

        //    var typeResolverMock = new Mock<ITypeResolver>();

        //    typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId }));

        //    var dataTypeServiceMock = new Mock<IDataTypeService>();

        //    dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
        //    dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);
        //    dataTypeServiceMock.Setup(m => m.GetPreValuesCollectionByDataTypeId(It.IsAny<int>())).Returns(new PreValueCollection(new Dictionary<string, PreValue>()));

        //    var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

        //    typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

        //    var dataTypeSynchronizationService = new DataTypeSynchronizer(
        //        dataTypeServiceMock.Object,
        //        typeResolverMock.Object,
        //        typeRepositoryMock.Object);

        //    dataTypeSynchronizationService.Run();

        //    dataTypeDefinitionWithIdMock.VerifySet(m => m.Name = dataTypeWithId.Name, Times.Once);
        //}

        //[Fact]
        //public void CanUpdateEditorForDataTypeWithId()
        //{
        //    var dataTypeWithId = new DataType(typeof(DataTypeWithId));

        //    var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

        //    dataTypeDefinitionWithIdMock.SetupAllProperties();

        //    var typeResolverMock = new Mock<ITypeResolver>();

        //    typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId }));

        //    var dataTypeServiceMock = new Mock<IDataTypeService>();

        //    dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
        //    dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);
        //    dataTypeServiceMock.Setup(m => m.GetPreValuesCollectionByDataTypeId(It.IsAny<int>())).Returns(new PreValueCollection(new Dictionary<string, PreValue>()));

        //    var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

        //    typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

        //    var dataTypeSynchronizationService = new DataTypeSynchronizer(
        //        dataTypeServiceMock.Object,
        //        typeResolverMock.Object,
        //        typeRepositoryMock.Object);

        //    dataTypeSynchronizationService.Run();

        //    dataTypeDefinitionWithIdMock.VerifySet(m => m.PropertyEditorAlias = dataTypeWithId.Editor, Times.Once);
        //}

        //[Fact]
        //public void CanUpdateDataTypeWithoutId()
        //{
        //    var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

        //    var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

        //    dataTypeDefinitionWithoutIdMock.SetupAllProperties();
        //    dataTypeDefinitionWithoutIdMock.Object.Name = dataTypeWithoutId.Name;

        //    var typeResolverMock = new Mock<ITypeResolver>();

        //    typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithoutId }));

        //    var dataTypeServiceMock = new Mock<IDataTypeService>();

        //    dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithoutIdMock.Object });
        //    dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);
        //    dataTypeServiceMock.Setup(m => m.GetPreValuesCollectionByDataTypeId(It.IsAny<int>())).Returns(new PreValueCollection(new Dictionary<string, PreValue>()));

        //    var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizer>(
        //        dataTypeServiceMock.Object,
        //        typeResolverMock.Object,
        //        new Mock<Jet.Data.ITypeRepository>().Object)
        //    { CallBase = true };

        //    dataTypeSynchronizationServiceMock.Object.Run();

        //    dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataTypeDefinition(dataTypeDefinitionWithoutIdMock.Object, dataTypeWithoutId), Times.Once);
        //}

        //[Fact]
        //public void CanUpdateEditorForDataTypeWithoutId()
        //{
        //    var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

        //    var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

        //    dataTypeDefinitionWithoutIdMock.SetupAllProperties();
        //    dataTypeDefinitionWithoutIdMock.Object.Name = dataTypeWithoutId.Name;

        //    var typeResolverMock = new Mock<ITypeResolver>();

        //    typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithoutId }));

        //    var dataTypeServiceMock = new Mock<IDataTypeService>();

        //    dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithoutIdMock.Object });
        //    dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);
        //    dataTypeServiceMock.Setup(m => m.GetPreValuesCollectionByDataTypeId(It.IsAny<int>())).Returns(new PreValueCollection(new Dictionary<string, PreValue>()));

        //    var dataTypeSynchronizationService = new DataTypeSynchronizer(
        //        dataTypeServiceMock.Object,
        //        typeResolverMock.Object,
        //        new Mock<Jet.Data.ITypeRepository>().Object);

        //    dataTypeSynchronizationService.Run();

        //    dataTypeDefinitionWithoutIdMock.VerifySet(m => m.PropertyEditorAlias = dataTypeWithoutId.Editor, Times.Once);
        //}

        //[DataType(
        //    "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054",
        //    typeof(int),
        //    "EditorForDataTypeWithId")]
        //protected class DataTypeWithId
        //{
        //}

        //[DataType(
        //    typeof(int),
        //    "EditorForDataTypeWithoutId")]
        //protected class DataTypeWithoutId
        //{
        //}
    }
}
