using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Umbraco.Web.Models;

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Mappings;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using DataTypeDefinition = Jet.Mappings.DataTypeDefinition;

    [TestClass]
    public class DefaultDataTypeDefinitionMappingTest
    {
        [TestMethod]
        public void CanUseNewContentPickerDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IPublishedContent), DataTypeDefinition.ContentPicker));
        }

        [TestMethod]
        public void CanUseOldContentPickerDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(int), DataTypeDefinition.ContentPicker));
        }

        [TestMethod]
        public void CanUseNewCheckboxListDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IEnumerable<string>), DataTypeDefinition.CheckboxList));
        }

        [TestMethod]
        public void CanUseOldCheckboxListDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), DataTypeDefinition.CheckboxList));
        }

        [TestMethod]
        public void CanUseNewDropdownMultipleDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IEnumerable<string>), DataTypeDefinition.DropdownMultiple));
        }

        [TestMethod]
        public void CanUseOldDropdownMultipleDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), DataTypeDefinition.DropdownMultiple));
        }

        [TestMethod]
        public void CanUseNewMediaPickerDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IPublishedContent), DataTypeDefinition.MediaPicker));
        }

        [TestMethod]
        public void CanUseOldMediaPickerDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), DataTypeDefinition.MediaPicker));
        }

        [TestMethod]
        public void CanUseNewRelatedLinksDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(RelatedLinks), DataTypeDefinition.RelatedLinks));
        }

        [TestMethod]
        public void CanUseOldRelatedLinksDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), DataTypeDefinition.RelatedLinks));
        }

        [TestMethod]
        public void CanUseNewTagsDataType()
        {
            Assert.IsTrue(CanUseNewDataType(typeof(IEnumerable<string>), DataTypeDefinition.Tags));
        }

        [TestMethod]
        public void CanUseOldTagsDataType()
        {
            Assert.IsTrue(CanUseOldDataType(typeof(string), DataTypeDefinition.Tags));
        }

        [TestMethod]
        public void CannotUseInvalidDataType()
        {
            var dataTypeDefinition = GetMappedDefinition(typeof(string), DataTypeDefinition.ContentPicker, false);

            Assert.IsNull(dataTypeDefinition);
        }

        private bool CanUseNewDataType(Type type, DataTypeDefinition dataTypeDefinition)
        {
            return GetMappedDefinition(type, dataTypeDefinition, true) != null;
        }

        private bool CanUseOldDataType(Type type, DataTypeDefinition dataTypeDefinition)
        {
            return GetMappedDefinition(type, dataTypeDefinition, false) != null;
        }

        private static IDataTypeDefinition GetMappedDefinition(Type type, DataTypeDefinition dataTypeDefinition, bool usePropertyValueConverter)
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
