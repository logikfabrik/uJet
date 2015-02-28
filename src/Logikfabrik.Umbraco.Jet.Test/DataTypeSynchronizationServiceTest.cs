// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Logikfabrik.Umbraco.Jet.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet.Test
{
    [TestClass]
    public class DataTypeSynchronizationServiceTest
    {
        private const string IdForDataTypeWithId = "D7B9B7F5-B2ED-4C2F-8239-9A2F50D14054";
        private const string NameForDataTypeWithId = "DataTypeWithId";
        private const string NameForDataTypeWithoutId = "DataTypeWithoutId";
        private const string EditorForDataTypeWithId = "EditorForDataTypeWithId";
        private const string EditorForDataTypeWithoutId = "EditorForDataTypeWithoutId";

        [DataType(IdForDataTypeWithId, typeof(int), EditorForDataTypeWithId)]
        public class DataTypeWithId
        {
        }

        [DataType(typeof(int), EditorForDataTypeWithoutId)]
        public class DataTypeWithoutId
        {
        }

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

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(dataTypeService.Object,
                dataTypeRepository.Object, typeService.Object);

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

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(dataTypeService.Object,
                dataTypeRepository.Object, typeService.Object);

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

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(dataTypeService.Object,
                dataTypeRepository.Object, typeService.Object);

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

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(dataTypeService.Object,
                dataTypeRepository.Object, typeService.Object);

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

            var dataTypeSynchronizationService = new DataTypeSynchronizationService(dataTypeService.Object,
                dataTypeRepository.Object, typeService.Object);

            dataTypeSynchronizationService.Synchronize();

            Assert.AreEqual(EditorForDataTypeWithoutId, withoutIdDataTypeDefinition.Object.PropertyEditorAlias);
        }
    }
}
