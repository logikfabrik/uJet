// <copyright file="DataTypeDefinitionService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Collections.Generic;
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
        private readonly Lazy<IDictionary<string, Tuple<Type, DataTypeDefinition>>> _hints;
        private readonly IDictionary<DataTypeDefinition, IDataTypeDefinition> _definitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeDefinitionService" /> class.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="enablePropertyValueConverters">Whether to enable property value converters.</param>
        public DataTypeDefinitionService(IDataTypeService dataTypeService, bool enablePropertyValueConverters)
        {
            EnsureArg.IsNotNull(dataTypeService);

            _dataTypeService = dataTypeService;
            _hints = new Lazy<IDictionary<string, Tuple<Type, DataTypeDefinition>>>(() => GetHints(enablePropertyValueConverters));
            _definitions = new Dictionary<DataTypeDefinition, IDataTypeDefinition>();
        }

        /// <inheritdoc />
        public IDataTypeDefinition GetDefinition(string uiHint, Type fromType)
        {
            if (!_hints.Value.TryGetValue(uiHint, out var hint))
            {
                return null;
            }

            if (hint.Item1 != fromType)
            {
                return GetNullableType(hint.Item1) == fromType ? GetDefinition(hint.Item2) : null;
            }

            return GetDefinition(hint.Item2);
        }

        private static IDictionary<string, Tuple<Type, DataTypeDefinition>> GetHints(bool enablePropertyValueConverters)
        {
            var hints = new Dictionary<string, Tuple<Type, DataTypeDefinition>>();

            string GetName(DataTypeDefinition dtd) => Enum.GetName(typeof(DataTypeDefinition), dtd);

            void AddHint(Type t, DataTypeDefinition dtd) => hints.Add(GetName(dtd), new Tuple<Type, DataTypeDefinition>(t, dtd));

            AddHint(typeof(string), DataTypeDefinition.ApprovedColor);
            AddHint(typeof(IPublishedContent), DataTypeDefinition.ContentPicker2);
            AddHint(typeof(string), DataTypeDefinition.DatePicker);
            AddHint(typeof(DateTime), DataTypeDefinition.DatePickerWithTime);
            AddHint(typeof(int), DataTypeDefinition.Dropdown);
            AddHint(typeof(string), DataTypeDefinition.FolderBrowser);
            AddHint(typeof(string), DataTypeDefinition.Label);
            AddHint(typeof(IPublishedContent), DataTypeDefinition.MediaPicker2);
            AddHint(typeof(string), DataTypeDefinition.MultipleMediaPicker);
            AddHint(typeof(int), DataTypeDefinition.Numeric);
            AddHint(typeof(string), DataTypeDefinition.Radiobox);
            AddHint(typeof(RelatedLinks), DataTypeDefinition.RelatedLinks2);
            AddHint(typeof(string), DataTypeDefinition.RichtextEditor);
            AddHint(typeof(string), DataTypeDefinition.TextboxMultiple);
            AddHint(typeof(string), DataTypeDefinition.Textstring);
            AddHint(typeof(bool), DataTypeDefinition.TrueFalse);
            AddHint(typeof(string), DataTypeDefinition.Upload);

            if (enablePropertyValueConverters)
            {
                AddHint(typeof(IPublishedContent), DataTypeDefinition.ContentPicker);
                AddHint(typeof(IEnumerable<string>), DataTypeDefinition.CheckboxList);
                AddHint(typeof(IEnumerable<string>), DataTypeDefinition.DropdownMultiple);
                AddHint(typeof(IPublishedContent), DataTypeDefinition.MediaPicker);
                AddHint(typeof(RelatedLinks), DataTypeDefinition.RelatedLinks);
                AddHint(typeof(IEnumerable<string>), DataTypeDefinition.Tags);
            }
            else
            {
                AddHint(typeof(int), DataTypeDefinition.ContentPicker);
                AddHint(typeof(string), DataTypeDefinition.CheckboxList);
                AddHint(typeof(string), DataTypeDefinition.DropdownMultiple);
                AddHint(typeof(string), DataTypeDefinition.MediaPicker);
                AddHint(typeof(string), DataTypeDefinition.RelatedLinks);
                AddHint(typeof(string), DataTypeDefinition.Tags);
            }

            return hints;
        }

        private static Type GetNullableType(Type type)
        {
            return type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }

        private IDataTypeDefinition GetDefinition(DataTypeDefinition dataTypeDefinition)
        {
            if (_definitions.TryGetValue(dataTypeDefinition, out var dtd))
            {
                return dtd;
            }

            dtd = _dataTypeService.GetDataTypeDefinitionById((int)dataTypeDefinition);

            if (dtd == null)
            {
                return null;
            }

            _definitions.Add(dataTypeDefinition, dtd);

            return dtd;
        }
    }
}