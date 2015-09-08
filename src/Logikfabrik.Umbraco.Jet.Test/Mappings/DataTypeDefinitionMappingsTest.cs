// <copyright file="DataTypeDefinitionMappingsTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using Jet.Mappings;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using global::Umbraco.Core.Models;

    [TestClass]
    public class DataTypeDefinitionMappingsTest
    {
        [TestMethod]
        public void CanGetDataTypeDefinitionForBoolean()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(bool));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(bool)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForDateTime()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(DateTime));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(DateTime)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForFloatingBinary()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(float));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(float)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForFloatingDecimal()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(decimal));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(decimal)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForInteger()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(int));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(int)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForString()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(string));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(string)));
        }

        [TestMethod]
        public void CanAddDataTypeDefinitionMapping()
        {
            DataTypeDefinitionMappings.Mappings.Add(typeof(Custom), new CustomDataTypeDefinitionMapping());

            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(Custom));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(Custom)));
        }

        [TestMethod]
        public void CanRemoveDataTypeDefinitionMapping()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(Custom));

            if (dtdm != null)
            {
                DataTypeDefinitionMappings.Mappings.Remove(typeof(Custom));
            }
                
            dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(typeof(Custom));

            Assert.IsNull(dtdm);
        }

        public class CustomDataTypeDefinitionMapping : IDataTypeDefinitionMapping
        {
            public bool CanMapToDefinition(Type fromType)
            {
                return fromType == typeof(Custom);
            }

            public IDataTypeDefinition GetMappedDefinition(Type fromType)
            {
                return new Mock<IDataTypeDefinition>().Object;
            }
        }

        public class Custom
        {
        }
    }
}
