// <copyright file="DataTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataTypeTest : TestBase
    {
        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var dataType = new DataType(typeof(Models.DataType));

            Assert.AreSame(typeof(object), dataType.Type);
        }

        [TestMethod]
        public void CanGetEditorFromAttribute()
        {
            var dataType = new DataType(typeof(Models.DataType));

            Assert.AreEqual("Editor", dataType.Editor);
        }

        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var dataType = new DataType(typeof(Models.DataType));

            Assert.AreEqual("DataType", dataType.Name);
        }

        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var dataType = new DataType(typeof(Models.DataType));

            Assert.AreEqual(Guid.Parse("443c08bc-e314-4e2d-ba26-66c6a565ad60"), dataType.Id);
        }
    }
}