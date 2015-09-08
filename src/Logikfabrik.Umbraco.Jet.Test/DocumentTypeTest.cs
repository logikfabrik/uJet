// <copyright file="DocumentTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DocumentTypeTest
    {
        private static DocumentType GetDocumentType()
        {
            return new DocumentType(typeof(DocumentTypeTestDocumentType));
        }

        [TestMethod]
        public void CanGetDefaultTemplateFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.AreEqual("DefaultTemplate", dt.DefaultTemplate);
        }

        [TestMethod]
        public void CanGetTemplatesFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.IsFalse(dt.Templates.Any());
        }

        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.AreSame(typeof(DocumentTypeTestDocumentType), dt.Type);
        }

        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.AreEqual("DocumentTypeTestDocumentType", dt.Name);
        }

        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.AreEqual("documentTypeTestDocumentType", dt.Alias);
        }

        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.AreEqual("Description", dt.Description);
        }

        [TestMethod]
        public void CanGetAllowedAsRootFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.AreEqual(true, dt.AllowedAsRoot);
        }

        [TestMethod]
        public void CanGetAllowedChildNodeTypesFromAttribute()
        {
            var dt = GetDocumentType();

            Assert.IsFalse(dt.AllowedChildNodeTypes.Any());
        }

        [TestMethod]
        public void CanGetProperties()
        {
            var dt = GetDocumentType();

            Assert.AreEqual(7, dt.Properties.Count());
        }

        [TestMethod]
        public void CanGetStringProperty()
        {
            var dt = GetDocumentType();
            var pt = dt.Properties.First(p => p.Name == "StringProperty");

            Assert.AreSame(typeof(string), pt.Type);
        }

        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var dt = GetDocumentType();
            var pt = dt.Properties.First(p => p.Name == "IntegerProperty");

            Assert.AreSame(typeof(int), pt.Type);
        }

        [TestMethod]
        public void CanGetFloatingDecimalProperty()
        {
            var dt = GetDocumentType();
            var pt = dt.Properties.First(p => p.Name == "FloatingDecimalProperty");

            Assert.AreSame(typeof(decimal), pt.Type);
        }

        [TestMethod]
        public void CanGetFloatingBinaryProperty()
        {
            var dt = GetDocumentType();
            var pt = dt.Properties.First(p => p.Name == "FloatingBinaryProperty");

            Assert.AreSame(typeof(float), pt.Type);
        }

        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var dt = GetDocumentType();
            var pt = dt.Properties.First(p => p.Name == "DateTimeProperty");

            Assert.AreSame(typeof(DateTime), pt.Type);
        }

        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var dt = GetDocumentType();
            var pt = dt.Properties.First(p => p.Name == "BooleanProperty");

            Assert.AreSame(typeof(bool), pt.Type);
        }

        public void CanGetCustomProperty()
        {
            var dt = GetDocumentType();
            var pt = dt.Properties.First(p => p.Name == "DocumentTypeTestPropertyTypeProperty");

            Assert.AreSame(typeof(DocumentTypeTestPropertyType), pt.Type);
        }

        [DocumentType("DocumentTypeTestDocumentType",
            Description = "Description",
            AllowedAsRoot = true,
            AllowedChildNodeTypes = new Type[] { },
            DefaultTemplate = "DefaultTemplate",
            Templates = new string[] { })]
        public class DocumentTypeTestDocumentType
        {
            // ReSharper disable once NotAccessedField.Local
            private string propertyWithoutGetter;

            public string StringProperty { get; set; }

            public int IntegerProperty { get; set; }

            public decimal FloatingDecimalProperty { get; set; }

            public float FloatingBinaryProperty { get; set; }

            public DateTime DateTimeProperty { get; set; }

            public bool BooleanProperty { get; set; }

            public DocumentTypeTestPropertyType DocumentTypeTestPropertyTypeProperty { get; set; }

            // ReSharper disable once UnusedMember.Local
            private string PrivateProperty { get; set; }

            [ScaffoldColumn(false)]
            public string NonScaffoldedProperty { get; set; }

            public string PropertyWithoutSetter { get; } = null;

            public string PropertyWithoutGetter { set { propertyWithoutGetter = value; } }
        }

        public class DocumentTypeTestPropertyType
        {
        }
    }
}
