// <copyright file="DataTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using Data;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The <see cref="DataTypeSynchronizationServiceTest" /> class.
    /// </summary>
    [TestClass]
    public class DataTypeSynchronizationServiceTest : TestBase
    {
        private const string IdForDataTypeWithId = "d7b9b7f5-b2ed-4c2f-8239-9a2f50d14054";
        private const string NameForDataTypeWithId = "DataTypeWithId";
        private const string NameForDataTypeWithoutId = "DataTypeWithoutId";
        private const string EditorForDataTypeWithId = "EditorForDataTypeWithId";
        private const string EditorForDataTypeWithoutId = "EditorForDataTypeWithoutId";

        /// <summary>
        /// Test to create data type with and without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateDataTypeWithAndWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId), typeof(DataTypeWithoutId) });

            var dataTypeDefinitionWithIdMock = new Mock<IDataTypeDefinition>();

            var dataTypeDefinitionWithoutIdMock = new Mock<IDataTypeDefinition>();

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByPropertyEditorAlias(EditorForDataTypeWithId))
                .Returns(new[] { dataTypeDefinitionWithIdMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByPropertyEditorAlias(EditorForDataTypeWithoutId))
                .Returns(new[] { dataTypeDefinitionWithoutIdMock.Object });

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                new Mock<IDataTypeRepository>().Object,
                typeServiceMock.Object);

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock
                .Verify(m => m.SynchronizeById(It.IsAny<IEnumerable<IDataTypeDefinition>>(), It.IsAny<DataType>()), Times.Once);

            dataTypeSynchronizationServiceMock
                .Verify(m => m.SynchronizeByName(It.IsAny<IEnumerable<IDataTypeDefinition>>(), It.IsAny<DataType>()), Times.Once);
        }

        /// <summary>
        /// Test to create data type with ID.
        /// </summary>
        [TestMethod]
        public void CanCreateDataTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId) });

            var dataTypeDefinitionMock = new Mock<IDataTypeDefinition>();

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByPropertyEditorAlias(EditorForDataTypeWithId))
                .Returns(new[] { dataTypeDefinitionMock.Object });

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                new Mock<IDataTypeRepository>().Object,
                typeServiceMock.Object);

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock
                .Verify(m => m.SynchronizeById(It.IsAny<IEnumerable<IDataTypeDefinition>>(), It.IsAny<DataType>()), Times.Once);
        }

        /// <summary>
        /// Test to create data type without ID.
        /// </summary>
        [TestMethod]
        public void CanCreateDataTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithoutId) });

            var dataTypeDefinitionMock = new Mock<IDataTypeDefinition>();

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByPropertyEditorAlias(EditorForDataTypeWithoutId))
                .Returns(new[] { dataTypeDefinitionMock.Object });

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                new Mock<IDataTypeRepository>().Object,
                typeServiceMock.Object);

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock
                .Verify(m => m.SynchronizeByName(It.IsAny<IEnumerable<IDataTypeDefinition>>(), It.IsAny<DataType>()), Times.Once);
        }

        /// <summary>
        /// Test to update data type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateDataTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId) });

            var dataTypeDefinitionMock = new Mock<IDataTypeDefinition>();

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionMock.Object });

            var dataTypeRepositoryMock = new Mock<IDataTypeRepository>();

            dataTypeRepositoryMock.Setup(m => m.GetDefinitionId(Guid.Parse(IdForDataTypeWithId)))
                .Returns(dataTypeDefinitionMock.Object.Id);

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                dataTypeRepositoryMock.Object,
                typeServiceMock.Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataType(dataTypeDefinitionMock.Object, It.IsAny<DataType>()), Times.Once);
        }

        /// <summary>
        /// Test to update name for data type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateNameForDataTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId) });

            var dataTypeDefinitionMock = new Mock<IDataTypeDefinition>();

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionMock.Object });

            var dataTypeRepositoryMock = new Mock<IDataTypeRepository>();

            dataTypeRepositoryMock.Setup(m => m.GetDefinitionId(Guid.Parse(IdForDataTypeWithId)))
                .Returns(dataTypeDefinitionMock.Object.Id);

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeServiceMock.Object,
                dataTypeRepositoryMock.Object,
                typeServiceMock.Object);

            dataTypeSynchronizationService.Synchronize();

            dataTypeDefinitionMock.VerifySet(m => m.Name = NameForDataTypeWithId, Times.Once);
        }

        /// <summary>
        /// Test to update editor for data type with ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateEditorForDataTypeWithId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId) });

            var dataTypeDefinitionMock = new Mock<IDataTypeDefinition>();

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionMock.Object });

            var dataTypeRepositoryMock = new Mock<IDataTypeRepository>();

            dataTypeRepositoryMock.Setup(m => m.GetDefinitionId(Guid.Parse(IdForDataTypeWithId)))
                .Returns(dataTypeDefinitionMock.Object.Id);

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeServiceMock.Object,
                dataTypeRepositoryMock.Object,
                typeServiceMock.Object);

            dataTypeSynchronizationService.Synchronize();

            dataTypeDefinitionMock.VerifySet(m => m.PropertyEditorAlias = EditorForDataTypeWithId, Times.Once);
        }

        /// <summary>
        /// Test to update data type without ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateDataTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithoutId) });

            var dataTypeDefinitionMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionMock.Setup(m => m.Name).Returns(NameForDataTypeWithoutId);

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByPropertyEditorAlias(dataTypeDefinitionMock.Object.PropertyEditorAlias))
                .Returns(new[] { dataTypeDefinitionMock.Object });

            var dataTypeSynchronizationServiceMock = new Mock<DataTypeSynchronizationService>(
                dataTypeServiceMock.Object,
                new Mock<IDataTypeRepository>().Object,
                typeServiceMock.Object)
            { CallBase = true };

            dataTypeSynchronizationServiceMock.Object.Synchronize();

            dataTypeSynchronizationServiceMock.Verify(m => m.UpdateDataType(dataTypeDefinitionMock.Object, It.IsAny<DataType>()), Times.Once);
        }

        /// <summary>
        /// Test to update editor for data type without ID.
        /// </summary>
        [TestMethod]
        public void CanUpdateEditorForDataTypeWithoutId()
        {
            var typeServiceMock = new Mock<ITypeService>();

            typeServiceMock.Setup(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithoutId) });

            var dataTypeDefinitionMock = new Mock<IDataTypeDefinition>();

            dataTypeDefinitionMock.Setup(m => m.Name).Returns(NameForDataTypeWithoutId);

            var dataTypeServiceMock = new Mock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { dataTypeDefinitionMock.Object });
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionByPropertyEditorAlias(dataTypeDefinitionMock.Object.PropertyEditorAlias))
                .Returns(new[] { dataTypeDefinitionMock.Object });

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeServiceMock.Object,
                new Mock<IDataTypeRepository>().Object,
                typeServiceMock.Object);

            dataTypeSynchronizationService.Synchronize();

            dataTypeDefinitionMock.VerifySet(m => m.PropertyEditorAlias = EditorForDataTypeWithoutId, Times.Once);
        }

        /// <summary>
        /// The <see cref="DataTypeWithId" /> class.
        /// </summary>
        [DataType(
            IdForDataTypeWithId,
            typeof(int),
            EditorForDataTypeWithId)]
        protected class DataTypeWithId
        {
        }

        /// <summary>
        /// The <see cref="DataTypeWithoutId" /> class.
        /// </summary>
        [DataType(
            typeof(int),
            EditorForDataTypeWithoutId)]
        protected class DataTypeWithoutId
        {
        }
    }
}
