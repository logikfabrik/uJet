// <copyright file="DataTypeSynchronizationServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    [TestClass]
    public class DataTypeSynchronizationServiceTest
    {
        private const string IdForDataTypeWithId = "D7B9B7F5-B2ED-4C2F-8239-9A2F50D14054";
        private const string NameForDataTypeWithId = "DataTypeWithId";
        private const string NameForDataTypeWithoutId = "DataTypeWithoutId";
        private const string EditorForDataTypeWithId = "EditorForDataTypeWithId";
        private const string EditorForDataTypeWithoutId = "EditorForDataTypeWithoutId";

        [TestMethod]
        public void CanCreateDataTypeWithAndWithoutId()
        {
            var dataTypeService = new Mock<IDataTypeService>();
            var dataTypeRepository = new Mock<IDataTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withIdDataTypeDefinition = new Mock<IDataTypeDefinition>();
            var withoutIdDataTypeDefinition = new Mock<IDataTypeDefinition>();

            withIdDataTypeDefinition.SetupAllProperties();
            withoutIdDataTypeDefinition.SetupAllProperties();

            dataTypeService.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });
            dataTypeService.Setup(m => m.Save(It.IsAny<IDataTypeDefinition>(), It.IsAny<int>()))
                .Callback<IDataTypeDefinition, int>((dtd, userId) =>
                {
                    switch (dtd.PropertyEditorAlias)
                    {
                        case EditorForDataTypeWithId:
                            withIdDataTypeDefinition.Object.Name = dtd.Name;
                            break;

                        case EditorForDataTypeWithoutId:
                            withoutIdDataTypeDefinition.Object.Name = dtd.Name;
                            break;
                    }
                });
            dataTypeService.Setup(
                m => m.GetDataTypeDefinitionByPropertyEditorAlias(It.Is<string>(v => v == EditorForDataTypeWithId)))
                .Returns(new[] { withIdDataTypeDefinition.Object });
            dataTypeService.Setup(
                m => m.GetDataTypeDefinitionByPropertyEditorAlias(It.Is<string>(v => v == EditorForDataTypeWithoutId)))
                .Returns(new[] { withoutIdDataTypeDefinition.Object });
            dataTypeRepository.Setup(m => m.GetDefinitionId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId), typeof(DataTypeWithoutId) });

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeService.Object,
                dataTypeRepository.Object,
                typeService.Object);

            dataTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDataTypeWithId, withIdDataTypeDefinition.Object.Name);
            Assert.AreEqual(NameForDataTypeWithoutId, withoutIdDataTypeDefinition.Object.Name);
        }

        [TestMethod]
        public void CanCreateDataTypeWithId()
        {
            var dataTypeService = new Mock<IDataTypeService>();
            var dataTypeRepository = new Mock<IDataTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withIdDataTypeDefinition = new Mock<IDataTypeDefinition>();

            withIdDataTypeDefinition.SetupAllProperties();

            dataTypeService.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });
            dataTypeService.Setup(m => m.Save(It.IsAny<IDataTypeDefinition>(), It.IsAny<int>()))
                .Callback<IDataTypeDefinition, int>((dtd, userId) =>
                {
                    withIdDataTypeDefinition.Object.Name = dtd.Name;
                });
            dataTypeService.Setup(
                m => m.GetDataTypeDefinitionByPropertyEditorAlias(It.Is<string>(v => v == EditorForDataTypeWithId)))
                .Returns(new[] { withIdDataTypeDefinition.Object });
            dataTypeRepository.Setup(m => m.GetDefinitionId(It.IsAny<Guid>())).Returns((int?)null);
            typeService.SetupGet(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId) });

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeService.Object,
                dataTypeRepository.Object,
                typeService.Object);

            dataTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDataTypeWithId, withIdDataTypeDefinition.Object.Name);
        }

        [TestMethod]
        public void CanCreateDataTypeWithoutId()
        {
            var dataTypeService = new Mock<IDataTypeService>();
            var dataTypeRepository = new Mock<IDataTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withoutIdDataTypeDefinition = new Mock<IDataTypeDefinition>();

            withoutIdDataTypeDefinition.SetupAllProperties();

            dataTypeService.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new IDataTypeDefinition[] { });
            dataTypeService.Setup(m => m.Save(It.IsAny<IDataTypeDefinition>(), It.IsAny<int>()))
                .Callback<IDataTypeDefinition, int>((dtd, userId) =>
                {
                    withoutIdDataTypeDefinition.Object.Name = dtd.Name;
                });
            dataTypeService.Setup(
                m => m.GetDataTypeDefinitionByPropertyEditorAlias(It.Is<string>(v => v == EditorForDataTypeWithoutId)))
                .Returns(new[] { withoutIdDataTypeDefinition.Object });
            typeService.SetupGet(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithoutId) });

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeService.Object,
                dataTypeRepository.Object,
                typeService.Object);

            dataTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDataTypeWithoutId, withoutIdDataTypeDefinition.Object.Name);
        }

        [TestMethod]
        public void CanUpdateNameForDataTypeWithId()
        {
            var dataTypeService = new Mock<IDataTypeService>();
            var dataTypeRepository = new Mock<IDataTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withIdDataTypeDbDefinition = new Mock<IDataTypeDefinition>();
            var withIdDataTypeDefinition = new Mock<IDataTypeDefinition>();

            withIdDataTypeDbDefinition.SetupAllProperties();
            withIdDataTypeDbDefinition.Object.Id = 1234;
            withIdDataTypeDbDefinition.Object.Name = "DbDataTypeWithId";

            withIdDataTypeDefinition.SetupAllProperties();

            dataTypeService.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { withIdDataTypeDbDefinition.Object });
            dataTypeService.Setup(m => m.Save(It.IsAny<IDataTypeDefinition>(), It.IsAny<int>()))
                .Callback<IDataTypeDefinition, int>((dtd, userId) =>
                {
                    withIdDataTypeDefinition.Object.Name = dtd.Name;
                });
            dataTypeService.Setup(
                m => m.GetDataTypeDefinitionByPropertyEditorAlias(It.Is<string>(v => v == EditorForDataTypeWithId)))
                .Returns(new[] { withIdDataTypeDefinition.Object });
            dataTypeRepository.Setup(m => m.GetDefinitionId(It.Is<Guid>(v => v.ToString() == IdForDataTypeWithId)))
                .Returns(withIdDataTypeDbDefinition.Object.Id);
            typeService.SetupGet(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithId) });

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeService.Object,
                dataTypeRepository.Object,
                typeService.Object);

            dataTypeSynchronizationService.Synchronize();

            Assert.AreEqual(NameForDataTypeWithId, withIdDataTypeDefinition.Object.Name);
        }

        [TestMethod]
        public void CanUpdateEditorForDataTypeWithoutId()
        {
            var dataTypeService = new Mock<IDataTypeService>();
            var dataTypeRepository = new Mock<IDataTypeRepository>();
            var typeService = new Mock<ITypeService>();
            var withoutIdDataTypeDbDefinition = new Mock<IDataTypeDefinition>();
            var withoutIdDataTypeDefinition = new Mock<IDataTypeDefinition>();

            withoutIdDataTypeDbDefinition.SetupAllProperties();
            withoutIdDataTypeDbDefinition.Object.Id = 1234;
            withoutIdDataTypeDbDefinition.Object.PropertyEditorAlias = "EditorForDbDataTypeWithId";

            withoutIdDataTypeDefinition.SetupAllProperties();

            dataTypeService.Setup(m => m.GetAllDataTypeDefinitions()).Returns(new[] { withoutIdDataTypeDbDefinition.Object });
            dataTypeService.Setup(m => m.Save(It.IsAny<IDataTypeDefinition>(), It.IsAny<int>()))
                .Callback<IDataTypeDefinition, int>((dtd, userId) =>
                {
                    withoutIdDataTypeDefinition.Object.PropertyEditorAlias = dtd.PropertyEditorAlias;
                });
            dataTypeService.Setup(
                m =>
                    m.GetDataTypeDefinitionByPropertyEditorAlias(
                        It.Is<string>(v => v == withoutIdDataTypeDbDefinition.Object.PropertyEditorAlias)))
                .Returns(new[] { withoutIdDataTypeDefinition.Object });
            typeService.SetupGet(m => m.DataTypes).Returns(new[] { typeof(DataTypeWithoutId) });

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(
                dataTypeService.Object,
                dataTypeRepository.Object,
                typeService.Object);

            dataTypeSynchronizationService.Synchronize();

            Assert.AreEqual(EditorForDataTypeWithoutId, withoutIdDataTypeDefinition.Object.PropertyEditorAlias);
        }

        [DataType(IdForDataTypeWithId, typeof(int), EditorForDataTypeWithId)]
        public class DataTypeWithId
        {
        }

        [DataType(typeof(int), EditorForDataTypeWithoutId)]
        public class DataTypeWithoutId
        {
        }
    }
}
