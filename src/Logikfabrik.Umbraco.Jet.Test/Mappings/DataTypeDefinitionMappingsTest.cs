// <copyright file="DataTypeDefinitionMappingsTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using global::Umbraco.Core.Models;
    using Jet.Mappings;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DataTypeDefinitionMappingsTest
    {
        [TestMethod]
        public void CanGetDataTypeDefinitionForBoolean()
        {
            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(bool));

            Assert.IsTrue(mapping.CanMapToDefinition(typeof(bool)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForDateTime()
        {
            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(DateTime));

            Assert.IsTrue(mapping.CanMapToDefinition(typeof(DateTime)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForFloatingBinary()
        {
            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(float));

            Assert.IsTrue(mapping.CanMapToDefinition(typeof(float)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForFloatingDecimal()
        {
            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(decimal));

            Assert.IsTrue(mapping.CanMapToDefinition(typeof(decimal)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForInteger()
        {
            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(int));

            Assert.IsTrue(mapping.CanMapToDefinition(typeof(int)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForString()
        {
            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(string));

            Assert.IsTrue(mapping.CanMapToDefinition(typeof(string)));
        }

        [TestMethod]
        public void CanAddDataTypeDefinitionMapping()
        {
            var mappingMock = new Mock<IDataTypeDefinitionMapping>();

            mappingMock.Setup(m => m.CanMapToDefinition(It.IsAny<Type>())).Returns((Type t) => t == typeof(object));
            mappingMock.Setup(m => m.GetMappedDefinition(It.IsAny<Type>())).Returns(new Mock<IDataTypeDefinition>().Object);

            DataTypeDefinitionMappings.Mappings.Add(typeof(object), mappingMock.Object);

            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(object));

            Assert.IsTrue(mapping.CanMapToDefinition(typeof(object)));
        }

        [TestMethod]
        public void CanRemoveDataTypeDefinitionMapping()
        {
            var mappingMock = new Mock<IDataTypeDefinitionMapping>();

            mappingMock.Setup(m => m.CanMapToDefinition(It.IsAny<Type>())).Returns((Type t) => t == typeof(object));
            mappingMock.Setup(m => m.GetMappedDefinition(It.IsAny<Type>())).Returns(new Mock<IDataTypeDefinition>().Object);

            var mapping = DataTypeDefinitionMappings.GetDefinitionMapping(mappingMock.Object.GetType());

            if (mapping != null)
            {
                DataTypeDefinitionMappings.Mappings.Remove(mappingMock.Object.GetType());
            }

            mapping = DataTypeDefinitionMappings.GetDefinitionMapping(mappingMock.Object.GetType());

            Assert.IsNull(mapping);
        }
    }
}
