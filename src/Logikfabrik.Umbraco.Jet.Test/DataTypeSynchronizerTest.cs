// <copyright file="DataTypeSynchronizerTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DataTypeSynchronizerTest : TestBase
    {
        [TestMethod]
        public void CanCreateDataTypeWithAndWithoutId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();
            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId, dataTypeWithoutId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizer>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Run();

            dataTypeSynchronizationServiceMock.Verify(m => m.CreateDataType(It.IsAny<DataType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizer>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Run();

            dataTypeSynchronizationServiceMock.Verify(m => m.CreateDataType(dataTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateDataTypeWithoutId()
        {
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithoutId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizer>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Run();

            dataTypeSynchronizationServiceMock.Verify(m => m.CreateDataType(dataTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizer>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Run();

            dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataTypeDefinition(dataTypeDefinitionWithIdMock.Object, dataTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

            var dataTypeSynchronizationService = new DataTypeSynchronizer(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object);

            dataTypeSynchronizationService.Run();

            dataTypeDefinitionWithIdMock.VerifySet(m => m.Name = dataTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateEditorForDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

            var dataTypeSynchronizationService = new DataTypeSynchronizer(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object);

            dataTypeSynchronizationService.Run();

            dataTypeDefinitionWithIdMock.VerifySet(m => m.PropertyEditorAlias = dataTypeWithId.Editor, Times.Once);
        }

        [TestMethod]
        public void CanUpdateDataTypeWithoutId()
        {
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithoutIdMock.SetupAllProperties();
            dataTypeDefinitionWithoutIdMock.Object.Name = dataTypeWithoutId.Name;

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithoutId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithoutIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizer>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Run();

            dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataTypeDefinition(dataTypeDefinitionWithoutIdMock.Object, dataTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateEditorForDataTypeWithoutId()
        {
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithoutIdMock.SetupAllProperties();
            dataTypeDefinitionWithoutIdMock.Object.Name = dataTypeWithoutId.Name;

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(Array.AsReadOnly(new[] { dataTypeWithoutId }));

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithoutIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationService = new DataTypeSynchronizer(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object);

            dataTypeSynchronizationService.Run();

            dataTypeDefinitionWithoutIdMock.VerifySet(m => m.PropertyEditorAlias = dataTypeWithoutId.Editor, Times.Once);
        }

        [DataType(
            "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054",
            typeof(int),
            "EditorForDataTypeWithId")]
        protected class DataTypeWithId
        {
        }

        [DataType(
            typeof(int),
            "EditorForDataTypeWithoutId")]
        protected class DataTypeWithoutId
        {
        }
    }
}
