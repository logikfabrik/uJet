// <copyright file="DefaultDataTypeDefinitionMappingTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using global::Umbraco.Web.Models;
    using Jet.Mappings;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DefaultDataTypeDefinitionMappingTest
    {
        [TestMethod]
        public void CanUseNewContentPickerDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IPublishedContent), Jet.Mappings.DataTypeDefinition.ContentPicker));
        }

        [TestMethod]
        public void CanUseOldContentPickerDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(int), Jet.Mappings.DataTypeDefinition.ContentPicker));
        }

        [TestMethod]
        public void CanUseNewCheckboxListDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IEnumerable<string>), Jet.Mappings.DataTypeDefinition.CheckboxList));
        }

        [TestMethod]
        public void CanUseOldCheckboxListDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), Jet.Mappings.DataTypeDefinition.CheckboxList));
        }

        [TestMethod]
        public void CanUseNewDropdownMultipleDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IEnumerable<string>), Jet.Mappings.DataTypeDefinition.DropdownMultiple));
        }

        [TestMethod]
        public void CanUseOldDropdownMultipleDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), Jet.Mappings.DataTypeDefinition.DropdownMultiple));
        }

        [TestMethod]
        public void CanUseNewMediaPickerDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IPublishedContent), Jet.Mappings.DataTypeDefinition.MediaPicker));
        }

        [TestMethod]
        public void CanUseOldMediaPickerDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), Jet.Mappings.DataTypeDefinition.MediaPicker));
        }

        [TestMethod]
        public void CanUseNewRelatedLinksDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(RelatedLinks), Jet.Mappings.DataTypeDefinition.RelatedLinks));
        }

        [TestMethod]
        public void CanUseOldRelatedLinksDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), Jet.Mappings.DataTypeDefinition.RelatedLinks));
        }

        [TestMethod]
        public void CanUseNewTagsDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IEnumerable<string>), Jet.Mappings.DataTypeDefinition.Tags));
        }

        [TestMethod]
        public void CanUseOldTagsDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), Jet.Mappings.DataTypeDefinition.Tags));
        }

        [TestMethod]
        public void CannotUseInvalidDataType()
        {
            var dataTypeDefinition = GetMappedDefinition(typeof(string), Jet.Mappings.DataTypeDefinition.ContentPicker, false);

            Assert.IsNull(dataTypeDefinition);
        }

        private static bool CanUseNewDataType(Type type, Jet.Mappings.DataTypeDefinition dataTypeDefinition)
        {
            return GetMappedDefinition(type, dataTypeDefinition, true) != null;
        }

        private static bool CanUseOldDataType(Type type, Jet.Mappings.DataTypeDefinition dataTypeDefinition)
        {
            return GetMappedDefinition(type, dataTypeDefinition, false) != null;
        }

        private static IDataTypeDefinition GetMappedDefinition(Type type, Jet.Mappings.DataTypeDefinition dataTypeDefinition, bool usePropertyValueConverter)
        {
            var propertyAlias = dataTypeDefinition.ToString();
            var dataTypeDefinitionId = (int)dataTypeDefinition;
            var dataTypeService = CreateMockDataTypeServiceForDataType(dataTypeDefinitionId, propertyAlias);
            var defaultDataTypeDefinitionMapping = new DefaultDataTypeDefinitionMapping(usePropertyValueConverter, dataTypeService);

            return defaultDataTypeDefinitionMapping.GetMappedDefinition(propertyAlias, type);
        }

        private static IDataTypeService CreateMockDataTypeServiceForDataType(int id, string alias)
        {
            var mockDataTypeDefinition = new Mock<IDataTypeDefinition>();
            mockDataTypeDefinition.Setup(m => m.Id).Returns(id);
            mockDataTypeDefinition.Setup(m => m.PropertyEditorAlias).Returns(alias);

            var dataTypeServiceMock = new Mock<IDataTypeService>();
            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionById(mockDataTypeDefinition.Object.Id)).Returns(mockDataTypeDefinition.Object);
            return dataTypeServiceMock.Object;
        }
    }
}
