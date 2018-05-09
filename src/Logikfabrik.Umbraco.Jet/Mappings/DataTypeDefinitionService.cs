// <copyright file="DataTypeDefinitionService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using global::Umbraco.Web.Models;

    /// <summary>
    /// The <see cref="DataTypeDefinitionService" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DataTypeDefinitionService : IDataTypeDefinitionService
    {
        private readonly IDataTypeService _dataTypeService;
        private readonly IDictionary<DefaultDataTypeDefinition, Type> _hints;
        private readonly List<IDataTypeDefinition> _definitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeDefinitionService" /> class.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="enablePropertyValueConverters">Whether to enable property value converters.</param>
        public DataTypeDefinitionService(IDataTypeService dataTypeService, bool enablePropertyValueConverters)
        {
            Ensure.That(dataTypeService).IsNotNull();

            _dataTypeService = dataTypeService;
            _hints = GetDefaultDataTypeHints(enablePropertyValueConverters);
            _definitions = new List<IDataTypeDefinition>();
        }

        /// <inheritdoc />
        public IDataTypeDefinition GetDefinition(Type fromType)
        {
            if (!DataTypeDefinitionMappings.Mappings.TryGetValue(fromType, out var mapping))
            {
                return null;
            }

            return mapping.CanMapToDefinition(fromType) ? GetDefinition(mapping.DefaultDataTypeDefinition) : null;
        }

        /// <inheritdoc />
        public IDataTypeDefinition GetDefinition(string uiHint, Type fromType)
        {
            if (!Enum.TryParse(uiHint, out DefaultDataTypeDefinition defaultDataTypeDefinition))
            {
                return GetDefinition(uiHint);
            }

            if (!_hints.TryGetValue(defaultDataTypeDefinition, out var type))
            {
                // Find data type definition by name.
                return GetDefinition(uiHint);
            }

            if (type == fromType || GetNullableType(type) == fromType)
            {
                return GetDefinition(defaultDataTypeDefinition);
            }

            return null;
        }

        private static IDictionary<DefaultDataTypeDefinition, Type> GetDefaultDataTypeHints(bool enablePropertyValueConverters)
        {
            var hints = new Dictionary<DefaultDataTypeDefinition, Type>();

            void AddHint(Type type, DefaultDataTypeDefinition defaultDataTypeDefinition)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                hints.Add(defaultDataTypeDefinition, type);
            }

            AddHint(typeof(string), DefaultDataTypeDefinition.ListViewMembers);
            AddHint(typeof(string), DefaultDataTypeDefinition.ListViewMedia);
            AddHint(typeof(string), DefaultDataTypeDefinition.ListViewContent);
            AddHint(typeof(string), DefaultDataTypeDefinition.Label);
            AddHint(typeof(string), DefaultDataTypeDefinition.Upload);
            AddHint(typeof(string), DefaultDataTypeDefinition.Textarea);
            AddHint(typeof(string), DefaultDataTypeDefinition.Textstring);
            AddHint(typeof(string), DefaultDataTypeDefinition.RichtextEditor);
            AddHint(typeof(int), DefaultDataTypeDefinition.Numeric);
            AddHint(typeof(bool), DefaultDataTypeDefinition.TrueFalse);
            AddHint(typeof(int), DefaultDataTypeDefinition.Dropdown);
            AddHint(typeof(DateTime), DefaultDataTypeDefinition.DatePicker);
            AddHint(typeof(string), DefaultDataTypeDefinition.Radiobox);
            AddHint(typeof(string), DefaultDataTypeDefinition.ApprovedColor);
            AddHint(typeof(DateTime), DefaultDataTypeDefinition.DatePickerWithTime);
            AddHint(typeof(string), DefaultDataTypeDefinition.ImageCropper);
            AddHint(typeof(int), DefaultDataTypeDefinition.MemberPicker);

            if (enablePropertyValueConverters)
            {
                AddHint(typeof(IEnumerable<string>), DefaultDataTypeDefinition.CheckboxList);
                AddHint(typeof(IEnumerable<string>), DefaultDataTypeDefinition.DropdownMultiple);
                AddHint(typeof(IEnumerable<string>), DefaultDataTypeDefinition.Tags);
                AddHint(typeof(IPublishedContent), DefaultDataTypeDefinition.ContentPicker);
                AddHint(typeof(IPublishedContent), DefaultDataTypeDefinition.MediaPicker);
                AddHint(typeof(IEnumerable<IPublishedContent>), DefaultDataTypeDefinition.MultipleMediaPicker);
                AddHint(typeof(RelatedLinks), DefaultDataTypeDefinition.RelatedLinks);
            }
            else
            {
                AddHint(typeof(string), DefaultDataTypeDefinition.CheckboxList);
                AddHint(typeof(string), DefaultDataTypeDefinition.DropdownMultiple);
                AddHint(typeof(string), DefaultDataTypeDefinition.Tags);
                AddHint(typeof(int), DefaultDataTypeDefinition.ContentPicker);
                AddHint(typeof(string), DefaultDataTypeDefinition.MediaPicker);
                AddHint(typeof(string), DefaultDataTypeDefinition.MultipleMediaPicker);
                AddHint(typeof(string), DefaultDataTypeDefinition.RelatedLinks);
            }

            return hints;
        }

        private static Type GetNullableType(Type type)
        {
            return type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }

        private IDataTypeDefinition GetDefinition(DefaultDataTypeDefinition defaultDataTypeDefinition)
        {
            var definition = _definitions.SingleOrDefault(d => d.Id == (int)defaultDataTypeDefinition);

            if (definition != null)
            {
                return definition;
            }

            definition = _dataTypeService.GetDataTypeDefinitionById((int)defaultDataTypeDefinition);

            if (definition == null)
            {
                return null;
            }

            _definitions.Add(definition);

            return definition;
        }

        private IDataTypeDefinition GetDefinition(string dataTypeDefinitionName)
        {
            if (dataTypeDefinitionName == null)
            {
                return null;
            }

            var definition = _definitions.SingleOrDefault(d => d.Name == dataTypeDefinitionName);

            if (definition != null)
            {
                return definition;
            }

            definition = _dataTypeService.GetDataTypeDefinitionByName(dataTypeDefinitionName);

            if (definition == null)
            {
                return null;
            }

            _definitions.Add(definition);

            return definition;
        }
    }
}