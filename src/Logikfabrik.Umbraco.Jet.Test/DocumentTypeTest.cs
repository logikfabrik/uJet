// <copyright file="DocumentTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DocumentTypeTest : TestBase
    {
        [TestMethod]
        public void CanGetComposition()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual(3, documentType.Composition[typeof(Models.DocumentType)].Count());
        }

        [TestMethod]
        public void CanGetDefaultTemplateFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual("DefaultTemplate", documentType.DefaultTemplate);
        }

        [TestMethod]
        public void CanGetTemplatesFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.IsFalse(documentType.Templates.Any());
        }

        [TestMethod]
        public void CanGetTypeFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreSame(typeof(Models.DocumentType), documentType.Type);
        }

        [TestMethod]
        public void CanGetNameFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual("DocumentType", documentType.Name);
        }

        [TestMethod]
        public void CanGetAliasFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual("documentType", documentType.Alias);
        }

        [TestMethod]
        public void CanGetIdFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual(Guid.Parse("85384e6c-9001-4c02-8b0e-eb76f1edabc7"), documentType.Id);
        }

        [TestMethod]
        public void CanGetDescriptionFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual("Description", documentType.Description);
        }

        [TestMethod]
        public void CanGetAllowedAsRootFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual(true, documentType.AllowedAsRoot);
        }

        [TestMethod]
        public void CanGetAllowedChildNodeTypesFromAttribute()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.IsFalse(documentType.AllowedChildNodeTypes.Any());
        }

        [TestMethod]
        public void CanGetProperties()
        {
            var documentType = new DocumentType(typeof(Models.DocumentType));

            Assert.AreEqual(11, documentType.Properties.Count());
        }

        [TestMethod]
        public void CanGetStringProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.StringProperty));

            Assert.AreSame(typeof(string), property.Type);
        }

        [TestMethod]
        public void CanGetIntegerProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.IntegerProperty));

            Assert.AreSame(typeof(int), property.Type);
        }

        [TestMethod]
        public void CanGetFloatingDecimalPointProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.FloatingDecimalPointProperty));

            Assert.AreSame(typeof(decimal), property.Type);
        }

        [TestMethod]
        public void CanGetFloatingBinaryPointProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.FloatingBinaryPointProperty));

            Assert.AreSame(typeof(float), property.Type);
        }

        [TestMethod]
        public void CanGetDateTimeProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.DateTimeProperty));

            Assert.AreSame(typeof(DateTime), property.Type);
        }

        [TestMethod]
        public void CanGetBooleanProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.First(p => p.Name == GetPropertyName(() => document.BooleanProperty));

            Assert.AreSame(typeof(bool), property.Type);
        }

        [TestMethod]
        public void CannotGetNonScaffoldedProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => document.NonScaffoldedStringProperty));

            Assert.IsNull(property);
        }

        [TestMethod]
        public void CannotGetPropertyWithoutSetter()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.FirstOrDefault(p => p.Name == GetPropertyName(() => document.StringPropertyWithoutSetter));

            Assert.IsNull(property);
        }

        [TestMethod]
        public void CannotGetPrivateProperty()
        {
            var document = new Models.DocumentType();
            var documentType = new DocumentType(document.GetType());
            var property = documentType.Properties.FirstOrDefault(p => p.Name == "PrivateStringProperty");

            Assert.IsNull(property);
        }
    }
}