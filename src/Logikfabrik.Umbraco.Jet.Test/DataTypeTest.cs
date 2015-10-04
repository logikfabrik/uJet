// <copyright file="DataTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="DataTypeTest" /> class.
    /// </summary>
    [TestClass]
    public class DataTypeTest : TestBase
    {
        /// <summary>
        /// Test to get type for data type.
        /// </summary>
        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var dataType = new Jet.DataType(typeof(DataType));

            Assert.AreSame(typeof(object), dataType.Type);
        }

        /// <summary>
        /// Test to get editor for data type.
        /// </summary>
        [TestMethod]
        public void CanGetEditorFromAttribute()
        {
            var dataType = new Jet.DataType(typeof(DataType));

            Assert.AreEqual("Editor", dataType.Editor);
        }

        /// <summary>
        /// Test to get name for data type.
        /// </summary>
        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var dataType = new Jet.DataType(typeof(DataType));

            Assert.AreEqual("DataType", dataType.Name);
        }

        /// <summary>
        /// Test to get ID for data type.
        /// </summary>
        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var dataType = new Jet.DataType(typeof(DataType));

            Assert.AreEqual(Guid.Parse("443c08bc-e314-4e2d-ba26-66c6a565ad60"), dataType.Id);
        }

        /// <summary>
        /// The <see cref="DataType" /> class.
        /// </summary>
        [DataType(
            "443c08bc-e314-4e2d-ba26-66c6a565ad60",
            typeof(object),
            "Editor")]
        protected class DataType
        {
        }
    }
}
