//----------------------------------------------------------------------------------
// <copyright file="DefaultDataTypeDefinitionMapping.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    public class DefaultDataTypeDefinitionMapping : IDefaultDataTypeDefinitionMapping
    {
        private readonly IDictionary<string, KeyValuePair<Type, DataTypeDefinition>> supportedHints;
        private readonly IDataTypeService dataTypeService;
        private readonly IDictionary<DataTypeDefinition, IDataTypeDefinition> mappedDefinitions = new Dictionary<DataTypeDefinition, IDataTypeDefinition>();

        public DefaultDataTypeDefinitionMapping()
            : this(ApplicationContext.Current.Services.DataTypeService, GetSupportedHints())
        {
        }

        protected DefaultDataTypeDefinitionMapping(IDataTypeService dataTypeService, IDictionary<string, KeyValuePair<Type, DataTypeDefinition>> supportedHints)
        {
            if (dataTypeService == null)
            {
                throw new ArgumentNullException("dataTypeService");
            }

            if (supportedHints == null)
            {
                throw new ArgumentNullException("supportedHints");
            }

            this.dataTypeService = dataTypeService;
            this.supportedHints = supportedHints;
        }

        public virtual bool CanMapToDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
            {
                throw new ArgumentException("UI hint cannot be null or white space.", "uiHint");
            }

            if (fromType == null)
            {
                throw new ArgumentNullException("fromType");
            }

            return this.GetDefinition(uiHint, fromType) != null;
        }

        public virtual IDataTypeDefinition GetMappedDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
            {
                throw new ArgumentException("UI hint cannot be null or white space.", "uiHint");
            }

            if (fromType == null)
            {
                throw new ArgumentNullException("fromType");
            }

            return this.GetDefinition(uiHint, fromType);
        }

        private static Type GetNullableType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }

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
            addHint(typeof(string), DataTypeDefinition.Label);
            addHint(typeof(int), DataTypeDefinition.MediaPicker);
            addHint(typeof(string), DataTypeDefinition.MultipleMediaPicker);
            addHint(typeof(int), DataTypeDefinition.Numeric);
            addHint(typeof(string), DataTypeDefinition.Radiobox);
            addHint(typeof(string), DataTypeDefinition.RelatedLinks);
            addHint(typeof(string), DataTypeDefinition.RichtextEditor);
            addHint(typeof(string), DataTypeDefinition.Tags);
            addHint(typeof(string), DataTypeDefinition.TextboxMultiple);
            addHint(typeof(string), DataTypeDefinition.Textstring);
            addHint(typeof(bool), DataTypeDefinition.TrueFalse);
            addHint(typeof(string), DataTypeDefinition.Upload);

            return hints;
        }

        private IDataTypeDefinition GetDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
            {
                throw new ArgumentException("UI hint cannot be null or white space.", "uiHint");
            }

            if (fromType == null)
            {
                throw new ArgumentNullException("fromType");
            }

            KeyValuePair<Type, DataTypeDefinition> v;

            if (!this.supportedHints.TryGetValue(uiHint, out v))
            {
                return null;
            }

            if (v.Key != fromType)
            {
                return GetNullableType(v.Key) == fromType ? this.GetDefinition(v.Value) : null;
            }

            return this.GetDefinition(v.Value);
        }
     
        private IDataTypeDefinition GetDefinition(DataTypeDefinition dataTypeDefinition)
        {
            IDataTypeDefinition v;

            if (this.mappedDefinitions.TryGetValue(dataTypeDefinition, out v))
            {
                return v;
            }

            v = this.dataTypeService.GetDataTypeDefinitionById((int)dataTypeDefinition);

            if (v == null)
            {
                return null;
            }

            this.mappedDefinitions.Add(dataTypeDefinition, v);

            return v;
        }
    }
}
