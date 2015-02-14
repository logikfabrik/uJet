// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    public class DefaultDataTypeDefinitionMapping : IDefaultDataTypeDefinitionMapping
    {
        private readonly IDictionary<string, KeyValuePair<Type, DataTypeDefinition>> _supportedHints;
        private readonly IDataTypeService _dataTypeService;
        private readonly IDictionary<DataTypeDefinition, IDataTypeDefinition> _mappedDefinitions = new Dictionary<DataTypeDefinition, IDataTypeDefinition>();

        private static IDictionary<string, KeyValuePair<Type, DataTypeDefinition>> GetSupportedHints()
        {
            var hints = new Dictionary<string, KeyValuePair<Type, DataTypeDefinition>>();

            Func<DataTypeDefinition, string> getName = v => Enum.GetName(typeof(DataTypeDefinition), v);
            Action<Type, DataTypeDefinition> addHint = (t, v) => hints.Add(getName(v), new KeyValuePair<Type, DataTypeDefinition>(t, v));

            addHint(typeof(string), DataTypeDefinition.ApprovedColor);
            addHint(typeof(string), DataTypeDefinition.CheckboxList);
            addHint(typeof(int), DataTypeDefinition.ContentPicker);
            addHint(typeof(string), DataTypeDefinition.DatePicker);
            addHint(typeof(DateTime), DataTypeDefinition.DatePickerWithTime);
            addHint(typeof(int), DataTypeDefinition.Dropdown);
            addHint(typeof(string), DataTypeDefinition.DropdownMultiple);
            addHint(typeof(string), DataTypeDefinition.FolderBrowser);
            // DataTypeDefinition.ImageCropper
            addHint(typeof(string), DataTypeDefinition.Label);
            // DataTypeDefinition.MacroContainer
            // DataTypeDefinition.MediaPicker
            addHint(typeof(int), DataTypeDefinition.MediaPicker);
            addHint(typeof(int), DataTypeDefinition.Numeric);
            addHint(typeof(string), DataTypeDefinition.Radiobox);
            // DataTypeDefinition.RelatedLinks
            addHint(typeof(string), DataTypeDefinition.RichtextEditor);
            addHint(typeof(string), DataTypeDefinition.SimpleEditor);
            addHint(typeof(string), DataTypeDefinition.Tags);
            addHint(typeof(string), DataTypeDefinition.TextboxMultiple);
            addHint(typeof(string), DataTypeDefinition.Textstring);
            addHint(typeof(bool), DataTypeDefinition.TrueFalse);
            addHint(typeof(string), DataTypeDefinition.UltimatePicker);
            addHint(typeof(string), DataTypeDefinition.Upload);

            return hints;
        }

        public DefaultDataTypeDefinitionMapping() : this(ApplicationContext.Current.Services.DataTypeService, GetSupportedHints()) { }

        protected DefaultDataTypeDefinitionMapping(IDataTypeService dataTypeService, IDictionary<string, KeyValuePair<Type, DataTypeDefinition>> supportedHints)
        {
            if (dataTypeService == null)
                throw new ArgumentNullException("dataTypeService");

            if (supportedHints == null)
                throw new ArgumentNullException("supportedHints");

            _dataTypeService = dataTypeService;
            _supportedHints = supportedHints;
        }

        private IDataTypeDefinition GetDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
                throw new ArgumentException("UI hint cannot be null or white space.", "uiHint");

            if (fromType == null)
                throw new ArgumentNullException("fromType");

            KeyValuePair<Type, DataTypeDefinition> v;

            if (!_supportedHints.TryGetValue(uiHint, out v))
                return null;

            if (v.Key != fromType)
                return GetNullableType(v.Key) == fromType ? GetDefinition(v.Value) : null;

            return GetDefinition(v.Value);
        }

        private static Type GetNullableType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }

        private IDataTypeDefinition GetDefinition(DataTypeDefinition dataTypeDefinition)
        {
            IDataTypeDefinition v;

            if (_mappedDefinitions.TryGetValue(dataTypeDefinition, out v))
                return v;

            v = _dataTypeService.GetDataTypeDefinitionById((int)dataTypeDefinition);

            if (v == null)
                return null;

            _mappedDefinitions.Add(dataTypeDefinition, v);

            return v;
        }

        public virtual bool CanMapToDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
                throw new ArgumentException("UI hint cannot be null or white space.", "uiHint");

            if (fromType == null)
                throw new ArgumentNullException("fromType");

            return GetDefinition(uiHint, fromType) != null;
        }

        public virtual IDataTypeDefinition GetMappedDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
                throw new ArgumentException("UI hint cannot be null or white space.", "uiHint");

            if (fromType == null)
                throw new ArgumentNullException("fromType");

            return GetDefinition(uiHint, fromType);
        }
    }
}
