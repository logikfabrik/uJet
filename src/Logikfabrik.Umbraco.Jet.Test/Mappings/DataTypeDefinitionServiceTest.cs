// <copyright file="DataTypeDefinitionServiceTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using global::Umbraco.Web.Models;
    using Logikfabrik.Umbraco.Jet.Mappings;
    using Moq.AutoMock;
    using Shouldly;
    using Xunit;

    public class DataTypeDefinitionServiceTest
    {
        [Theory]
        [InlineData(false, UIHints.ListViewMembers, typeof(string))]
        [InlineData(false, UIHints.ListViewMedia, typeof(string))]
        [InlineData(false, UIHints.ListViewContent, typeof(string))]
        [InlineData(false, UIHints.Label, typeof(string))]
        [InlineData(false, UIHints.Upload, typeof(string))]
        [InlineData(false, UIHints.Textarea, typeof(string))]
        [InlineData(false, UIHints.Textstring, typeof(string))]
        [InlineData(false, UIHints.RichtextEditor, typeof(string))]
        [InlineData(false, UIHints.Numeric, typeof(int))]
        [InlineData(false, UIHints.Numeric, typeof(int?))]
        [InlineData(false, UIHints.TrueFalse, typeof(bool))]
        [InlineData(false, UIHints.TrueFalse, typeof(bool?))]
        [InlineData(false, UIHints.Dropdown, typeof(int))]
        [InlineData(false, UIHints.Dropdown, typeof(int?))]
        [InlineData(false, UIHints.DatePicker, typeof(DateTime))]
        [InlineData(false, UIHints.DatePicker, typeof(DateTime?))]
        [InlineData(false, UIHints.Radiobox, typeof(string))]
        [InlineData(false, UIHints.ApprovedColor, typeof(string))]
        [InlineData(false, UIHints.DatePickerWithTime, typeof(DateTime))]
        [InlineData(false, UIHints.DatePickerWithTime, typeof(DateTime?))]
        [InlineData(false, UIHints.ImageCropper, typeof(string))]
        [InlineData(false, UIHints.MemberPicker, typeof(int))]
        [InlineData(false, UIHints.MemberPicker, typeof(int?))]
        [InlineData(false, UIHints.CheckboxList, typeof(string))]
        [InlineData(false, UIHints.DropdownMultiple, typeof(string))]
        [InlineData(false, UIHints.Tags, typeof(string))]
        [InlineData(false, UIHints.ContentPicker, typeof(int))]
        [InlineData(false, UIHints.ContentPicker, typeof(int?))]
        [InlineData(false, UIHints.MediaPicker, typeof(string))]
        [InlineData(false, UIHints.MultipleMediaPicker, typeof(string))]
        [InlineData(false, UIHints.RelatedLinks, typeof(string))]
        [InlineData(true, UIHints.ListViewMembers, typeof(string))]
        [InlineData(true, UIHints.ListViewMedia, typeof(string))]
        [InlineData(true, UIHints.ListViewContent, typeof(string))]
        [InlineData(true, UIHints.Label, typeof(string))]
        [InlineData(true, UIHints.Upload, typeof(string))]
        [InlineData(true, UIHints.Textarea, typeof(string))]
        [InlineData(true, UIHints.Textstring, typeof(string))]
        [InlineData(true, UIHints.RichtextEditor, typeof(string))]
        [InlineData(true, UIHints.Numeric, typeof(int))]
        [InlineData(true, UIHints.Numeric, typeof(int?))]
        [InlineData(true, UIHints.TrueFalse, typeof(bool))]
        [InlineData(true, UIHints.TrueFalse, typeof(bool?))]
        [InlineData(true, UIHints.Dropdown, typeof(int))]
        [InlineData(true, UIHints.Dropdown, typeof(int?))]
        [InlineData(true, UIHints.DatePicker, typeof(DateTime))]
        [InlineData(true, UIHints.DatePicker, typeof(DateTime?))]
        [InlineData(true, UIHints.Radiobox, typeof(string))]
        [InlineData(true, UIHints.ApprovedColor, typeof(string))]
        [InlineData(true, UIHints.DatePickerWithTime, typeof(DateTime))]
        [InlineData(true, UIHints.DatePickerWithTime, typeof(DateTime?))]
        [InlineData(true, UIHints.ImageCropper, typeof(string))]
        [InlineData(true, UIHints.MemberPicker, typeof(int))]
        [InlineData(true, UIHints.MemberPicker, typeof(int?))]
        [InlineData(true, UIHints.CheckboxList, typeof(IEnumerable<string>))]
        [InlineData(true, UIHints.DropdownMultiple, typeof(IEnumerable<string>))]
        [InlineData(true, UIHints.Tags, typeof(IEnumerable<string>))]
        [InlineData(true, UIHints.ContentPicker, typeof(IPublishedContent))]
        [InlineData(true, UIHints.MediaPicker, typeof(IPublishedContent))]
        [InlineData(true, UIHints.MultipleMediaPicker, typeof(IEnumerable<IPublishedContent>))]
        [InlineData(true, UIHints.RelatedLinks, typeof(RelatedLinks))]

        // ReSharper disable once InconsistentNaming
        public void CanGetDefaultDataTypeDefinitionByUIHint(bool enablePropertyValueConverters, string uiHint, Type fromType)
        {
            var defaultDataTypeDefinition = Enum.Parse(typeof(DefaultDataTypeDefinition), uiHint);

            var mocker = new AutoMocker();

            var dataTypeDefinition = mocker.Get<IDataTypeDefinition>();

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionById((int)defaultDataTypeDefinition)).Returns(dataTypeDefinition);

            var dataTypeDefinitionService = new DataTypeDefinitionService(dataTypeServiceMock.Object, enablePropertyValueConverters);

            var definition = dataTypeDefinitionService.GetDefinition(uiHint, fromType);

            definition.ShouldNotBeNull();
        }

        [Theory]
        [ClassData(typeof(CanGetDefaultDataTypeDefinitionByTypeThroughBuiltInDataTypeDefinitionMappingsClassData))]
        public void CanGetDefaultDataTypeDefinitionByTypeThroughBuiltInDataTypeDefinitionMappings(IDataTypeDefinitionMapping mapping)
        {
            var mocker = new AutoMocker();

            var dataTypeDefinition = mocker.Get<IDataTypeDefinition>();

            var dataTypeServiceMock = mocker.GetMock<IDataTypeService>();

            dataTypeServiceMock.Setup(m => m.GetDataTypeDefinitionById((int)mapping.DefaultDataTypeDefinition)).Returns(dataTypeDefinition);

            var dataTypeDefinitionService = new DataTypeDefinitionService(dataTypeServiceMock.Object, false);

            foreach (var supportedType in mapping.SupportedTypes)
            {
                var definition = dataTypeDefinitionService.GetDefinition(supportedType);

                definition.ShouldNotBeNull();
            }
        }

        private class CanGetDefaultDataTypeDefinitionByTypeThroughBuiltInDataTypeDefinitionMappingsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                BuiltInDataTypeDefinitionMappingsRegistrar.RegisterAll();

                return DataTypeDefinitionMappings.Mappings.Values.Select(mapping => new[] { mapping}).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}