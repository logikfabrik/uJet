// <copyright file="DocumentTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The <see cref="DocumentTypeTest" /> class.
    /// </summary>
    [TestClass]
    public class DocumentTypeTest : TestBase
    {
        /// <summary>
        /// Test to get default template for document type.
        /// </summary>
        [TestMethod]
        public void CanGetDefaultTemplateFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreEqual("DefaultTemplate", documentType.DefaultTemplate);
        }

        /// <summary>
        /// Test to get templates for document type.
        /// </summary>
        [TestMethod]
        public void CanGetTemplatesFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.IsFalse(documentType.Templates.Any());
        }

        /// <summary>
        /// Test to get type for document type.
        /// </summary>
        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreSame(typeof(DocumentType), documentType.Type);
        }

        /// <summary>
        /// Test to get name for document type.
        /// </summary>
        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreEqual("DocumentType", documentType.Name);
        }

        /// <summary>
        /// Test to get alias for document type.
        /// </summary>
        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreEqual("documentType", documentType.Alias);
        }

        /// <summary>
        /// Test to get ID for document type.
        /// </summary>
        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreEqual(Guid.Parse("85384e6c-9001-4c02-8b0e-eb76f1edabc7"), documentType.Id);
        }

        /// <summary>
        /// Test to get description for document type.
        /// </summary>
        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreEqual("Description", documentType.Description);
        }

        /// <summary>
        /// Test to get allowed as root for document type.
        /// </summary>
        [TestMethod]
        public void CanGetAllowedAsRootFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreEqual(true, documentType.AllowedAsRoot);
        }

        /// <summary>
        /// Test to get allowed child node types for document type.
        /// </summary>
        [TestMethod]
        public void CanGetAllowedChildNodeTypesFromAttribute()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.IsFalse(documentType.AllowedChildNodeTypes.Any());
        }

        /// <summary>
        /// Test to get properties for document type.
        /// </summary>
        [TestMethod]
        public void CanGetProperties()
        {
            var documentType = new Jet.DocumentType(typeof(DocumentType));

            Assert.AreEqual(6, documentType.Properties.Count());
        }

        /// <summary>
        /// Test to get <see cref="string" /> property for document type.
        /// </summary>
        [TestMethod]
        public void CanGetStringProperty()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.StringProperty));

            Assert.AreSame(typeof(string), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="int" /> property for document type.
        /// </summary>
        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.IntegerProperty));

            Assert.AreSame(typeof(int), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="decimal" /> property for document type.
        /// </summary>
        [TestMethod]
        public void CanGetFloatingDecimalPointProperty()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.FloatingDecimalPointProperty));

            Assert.AreSame(typeof(decimal), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="float" /> property for document type.
        /// </summary>
        [TestMethod]
        public void CanGetFloatingBinaryPointProperty()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.FloatingBinaryPointProperty));

            Assert.AreSame(typeof(float), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="DateTime" /> property for document type.
        /// </summary>
        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.DateTimeProperty));

            Assert.AreSame(typeof(DateTime), property.Type);
        }

        /// <summary>
        /// Test to get <see cref="bool" /> property for document type.
        /// </summary>
        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.BooleanProperty));

            Assert.AreSame(typeof(bool), property.Type);
        }

        /// <summary>
        /// Test to get non scaffolded property for document type.
        /// </summary>
        [TestMethod]
        public void CanNotGetNonScaffoldedProperty()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => document.NonScaffoldedStringProperty));

            Assert.IsNull(property);
        }

        /// <summary>
        /// Test to get property without setter for document type.
        /// </summary>
        [TestMethod]
        public void CanNotGetPropertyWithoutSetter()
        {
            var document = new DocumentType();
            var documentType = new Jet.DocumentType(document.GetType());
            var property = documentType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => document.StringPropertyWithoutSetter));

            Assert.IsNull(property);
        }

        /// <summary>
        /// The <see cref="DocumentType" /> class.
        /// </summary>
        [DocumentType(
            "85384e6c-9001-4c02-8b0e-eb76f1edabc7",
            "DocumentType",
            Description = "Description",
            AllowedAsRoot = true,
            AllowedChildNodeTypes = new Type[] { },
            DefaultTemplate = "DefaultTemplate",
            Templates = new string[] { })]
        protected class DocumentType
        {
            // ReSharper disable once NotAccessedField.Local
            private string stringPropertyWithoutGetter;

            /// <summary>
            /// Initializes a new instance of the <see cref="DocumentType"/> class.
            /// </summary>
            public DocumentType()
            {
                StringPropertyWithoutSetter = null;
                stringPropertyWithoutGetter = null;
            }

            /// <summary>
            /// Gets or sets the string property value.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            public string StringProperty { get; set; }

            /// <summary>
            /// Gets or sets the integer property value.
            /// </summary>
            /// <value>
            /// The integer property value.
            /// </value>
            public int IntegerProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating binary point property value.
            /// </summary>
            /// <value>
            /// The floating binary point property value.
            /// </value>
            public float FloatingBinaryPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the floating decimal point property value.
            /// </summary>
            /// <value>
            /// The floating decimal point property value.
            /// </value>
            public decimal FloatingDecimalPointProperty { get; set; }

            /// <summary>
            /// Gets or sets the boolean property value.
            /// </summary>
            /// <value>
            /// The boolean property value.
            /// </value>
            public bool BooleanProperty { get; set; }

            /// <summary>
            /// Gets or sets the DateTime property value.
            /// </summary>
            /// <value>
            /// The DateTime property value.
            /// </value>
            public DateTime DateTimeProperty { get; set; }

            /// <summary>
            /// Gets or sets the non scaffolded string property value.
            /// </summary>
            /// <value>
            /// The non scaffolded string property value.
            /// </value>
            [ScaffoldColumn(false)]
            public string NonScaffoldedStringProperty { get; set; }

            /// <summary>
            /// Gets the string property value, for property without setter.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            public string StringPropertyWithoutSetter { get; }

            /// <summary>
            /// Sets the string property value, for property without getter.
            /// </summary>
            /// <value>
            /// The string property value.
            /// </value>
            public string StringPropertyWithoutGetter
            {
                set { stringPropertyWithoutGetter = value; }
            }

            // ReSharper disable once UnusedMember.Local
            private string PrivateStringProperty { get; set; }
        }
    }
}
