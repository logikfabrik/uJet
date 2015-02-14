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

using Logikfabrik.Umbraco.Jet.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Umbraco.Core.Models;

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    [TestClass]
    public class DataTypeDefinitionMappingsTest
    {
        [TestMethod]
        public void CanGetDataTypeDefinitionForBoolean()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(bool));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(bool)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForDateTime()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(DateTime));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(DateTime)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForFloatingBinary()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(float));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(float)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForFloatingDecimal()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(decimal));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(decimal)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForInteger()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(int));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(int)));
        }

        [TestMethod]
        public void CanGetDataTypeDefinitionForString()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(string));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(string)));
        }

        [TestMethod]
        public void CanAddDataTypeDefinitionMapping()
        {
            DataTypeDefinitionMappings.Mappings.Add(typeof(Custom), new CustomDataTypeDefinitionMapping());

            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(Custom));

            Assert.IsTrue(dtdm.CanMapToDefinition(typeof(Custom)));
        }

        [TestMethod]
        public void CanRemoveDataTypeDefinitionMapping()
        {
            var dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(Custom));

            if (dtdm != null)
                DataTypeDefinitionMappings.Mappings.Remove(typeof(Custom));

            dtdm = DataTypeDefinitionMappings.GetDefinitionMapping(null, typeof(Custom));

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
