// <copyright file="MediaTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MediaTypeTest
    {
        private static MediaType GetMediaType()
        {
            return new MediaType(typeof(MediaTypeTestMediaType));
        }

        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var dt = GetMediaType();

            Assert.AreSame(typeof(MediaTypeTestMediaType), dt.Type);
        }

        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var dt = GetMediaType();

            Assert.AreEqual("MediaTypeTestMediaType", dt.Name);
        }

        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var dt = GetMediaType();

            Assert.AreEqual("mediaTypeTestMediaType", dt.Alias);
        }

        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var dt = GetMediaType();

            Assert.AreEqual("Description", dt.Description);
        }

        [TestMethod]
        public void CanGetAllowedAsRootFromAttribute()
        {
            var dt = GetMediaType();

            Assert.AreEqual(true, dt.AllowedAsRoot);
        }

        [TestMethod]
        public void CanGetAllowedChildNodeTypesFromAttribute()
        {
            var dt = GetMediaType();

            Assert.IsFalse(dt.AllowedChildNodeTypes.Any());
        }

        [TestMethod]
        public void CanGetProperties()
        {
            var dt = GetMediaType();

            Assert.AreEqual(7, dt.Properties.Count());
        }

        [TestMethod]
        public void CanGetStringProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.First(p => p.Name == "StringProperty");

            Assert.AreSame(typeof(string), pt.Type);
        }

        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.First(p => p.Name == "IntegerProperty");

            Assert.AreSame(typeof(int), pt.Type);
        }

        [TestMethod]
        public void CanGetFloatingDecimalProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.First(p => p.Name == "FloatingDecimalProperty");

            Assert.AreSame(typeof(decimal), pt.Type);
        }

        [TestMethod]
        public void CanGetFloatingBinaryProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.First(p => p.Name == "FloatingBinaryProperty");

            Assert.AreSame(typeof(float), pt.Type);
        }

        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.First(p => p.Name == "DateTimeProperty");

            Assert.AreSame(typeof(DateTime), pt.Type);
        }

        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.First(p => p.Name == "BooleanProperty");

            Assert.AreSame(typeof(bool), pt.Type);
        }

        [TestMethod]
        public void CanGetCustomProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.First(p => p.Name == "MediaTypeTestPropertyTypeProperty");

            Assert.AreSame(typeof(MediaTypeTestPropertyType), pt.Type);
        }

        [TestMethod]
        public void CanNotGetNonScaffoldedProperty()
        {
            var dt = GetMediaType();
            var pt = dt.Properties.FirstOrDefault(p => p.Name == "NonScaffoldedProperty");

            Assert.IsNull(pt);
        }

        [MediaType(
            "MediaTypeTestMediaType",
            Description = "Description",
            AllowedAsRoot = true,
            AllowedChildNodeTypes = new Type[] { })]
        public class MediaTypeTestMediaType
        {
            // ReSharper disable once NotAccessedField.Local
            private string propertyWithoutGetter;

            public string StringProperty { get; set; }

            public int IntegerProperty { get; set; }

            public decimal FloatingDecimalProperty { get; set; }

            public float FloatingBinaryProperty { get; set; }

            public DateTime DateTimeProperty { get; set; }

            public bool BooleanProperty { get; set; }

            public MediaTypeTestPropertyType MediaTypeTestPropertyTypeProperty { get; set; }

            // ReSharper disable once UnusedMember.Local
            private string PrivateProperty { get; set; }

            [ScaffoldColumn(false)]
            public string NonScaffoldedProperty { get; set; }

            public string PropertyWithoutSetter { get; } = null;

            public string PropertyWithoutGetter { set { propertyWithoutGetter = value; } }
        }

        public class MediaTypeTestPropertyType
        {
        }
    }
}
