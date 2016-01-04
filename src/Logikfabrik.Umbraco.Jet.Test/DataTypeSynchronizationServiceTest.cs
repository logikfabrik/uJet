// <copyright file="DataTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DataTypeSynchronizationServiceTest : TestBase
    {
        [TestMethod]
        public void CanCreateDataTypeWithAndWithoutId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();
            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithId, dataTypeWithoutId });

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock.Verify(m => m.CreateDataTypeDefinition(It.IsAny<DataType>()), Times.Exactly(2));
        }

        [TestMethod]
        public void CanCreateDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithId });

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock.Verify(m => m.CreateDataTypeDefinition(dataTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanCreateDataTypeWithoutId()
        {
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithoutId });

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock.Verify(m => m.CreateDataTypeDefinition(dataTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType(dataTypeWithId, It.IsAny<IDataTypeDefinition[]>())).Returns(dataTypeDefinitionWithIdMock.Object);

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataTypeDefinition(dataTypeDefinitionWithIdMock.Object, dataTypeWithId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateNameForDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType(dataTypeWithId, It.IsAny<IDataTypeDefinition[]>())).Returns(dataTypeDefinitionWithIdMock.Object);

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object);

            dataTypeSynchronizationService.Synchronize();

            dataTypeDefinitionWithIdMock.VerifySet(m => m.Name = dataTypeWithId.Name, Times.Once);
        }

        [TestMethod]
        public void CanUpdateEditorForDataTypeWithId()
        {
            var dataTypeWithId = new DataType(typeof(DataTypeWithId));

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithId });
            typeResolverMock.Setup(m => m.ResolveType(dataTypeWithId, It.IsAny<IDataTypeDefinition[]>())).Returns(dataTypeDefinitionWithIdMock.Object);

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithId.Name)).Returns(dataTypeDefinitionWithIdMock.Object);

            var typeRepositoryMock = new Mock<Jet.Data.ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetDefinitionId(dataTypeWithId.Id.Value)).Returns(dataTypeDefinitionWithIdMock.Object.Id);

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                typeRepositoryMock.Object);

            dataTypeSynchronizationService.Synchronize();

            dataTypeDefinitionWithIdMock.VerifySet(m => m.PropertyEditorAlias = dataTypeWithId.Editor, Times.Once);
        }

        [TestMethod]
        public void CanUpdateDataTypeWithoutId()
        {
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithoutIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithoutId });
            typeResolverMock.Setup(m => m.ResolveType(dataTypeWithoutId, It.IsAny<IDataTypeDefinition[]>())).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithoutIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataTypeDefinition(dataTypeDefinitionWithoutIdMock.Object, dataTypeWithoutId), Times.Once);
        }

        [TestMethod]
        public void CanUpdateEditorForDataTypeWithoutId()
        {
            var dataTypeWithoutId = new DataType(typeof(DataTypeWithoutId));

            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionWithoutIdMock.SetupAllProperties();

            var typeResolverMock = new Mock<ITypeResolver>();

            typeResolverMock.Setup(m => m.DataTypes).Returns(new[] { dataTypeWithoutId });
            typeResolverMock.Setup(m => m.ResolveType(dataTypeWithoutId, It.IsAny<IDataTypeDefinition[]>())).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionWithoutIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByName(dataTypeWithoutId.Name)).Returns(dataTypeDefinitionWithoutIdMock.Object);

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeServiceMock.Object,
                typeResolverMock.Object,
                new Mock<Jet.Data.ITypeRepository>().Object);

            dataTypeSynchronizationService.Synchronize();

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
