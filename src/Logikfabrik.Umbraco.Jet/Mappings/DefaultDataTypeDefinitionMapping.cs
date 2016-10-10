// <copyright file="DefaultDataTypeDefinitionMapping.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using System.Collections.Generic;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DefaultDataTypeDefinitionMapping" /> class.
    /// </summary>
    public class DefaultDataTypeDefinitionMapping : IDefaultDataTypeDefinitionMapping
    {
        private readonly IDictionary<string, KeyValuePair<Type, DataTypeDefinition>> _supportedHints;
        private readonly IDataTypeService _dataTypeService;
        private readonly IDictionary<DataTypeDefinition, IDataTypeDefinition> _mappedDefinitions = new Dictionary<DataTypeDefinition, IDataTypeDefinition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDataTypeDefinitionMapping" /> class.
        /// </summary>
        public DefaultDataTypeDefinitionMapping()
            : this(ApplicationContext.Current.Services.DataTypeService, GetSupportedHints())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDataTypeDefinitionMapping" /> class.
        /// </summary>
        /// <param name="dataTypeService">The data type service.</param>
        /// <param name="supportedHints">The supported hints.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeService" />, or <paramref name="supportedHints" /> are <c>null</c>.</exception>
        protected DefaultDataTypeDefinitionMapping(IDataTypeService dataTypeService, IDictionary<string, KeyValuePair<Type, DataTypeDefinition>> supportedHints)
        {
            if (dataTypeService == null)
            {
                throw new ArgumentNullException(nameof(dataTypeService));
            }

            if (supportedHints == null)
            {
                throw new ArgumentNullException(nameof(supportedHints));
            }

            _dataTypeService = dataTypeService;
            _supportedHints = supportedHints;
        }

        /// <summary>
        /// Determines whether this instance can map to definition.
        /// </summary>
        /// <param name="uiHint">A UI hint.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can map to definition; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="uiHint" /> is <c>null</c> or white space.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromType" /> is <c>null</c>.</exception>
        public virtual bool CanMapToDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
            {
                throw new ArgumentException("UI hint cannot be null or white space.", nameof(uiHint));
            }

            if (fromType == null)
            {
                throw new ArgumentNullException(nameof(fromType));
            }

            return GetDefinition(uiHint, fromType) != null;
        }

        /// <summary>
        /// Gets the mapped definition.
        /// </summary>
        /// <param name="uiHint">A UI hint.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>
        /// The mapped definition.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="uiHint" /> is <c>null</c> or white space.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromType" /> is <c>null</c>.</exception>
        public virtual IDataTypeDefinition GetMappedDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
            {
                throw new ArgumentException("UI hint cannot be null or white space.", nameof(uiHint));
            }

            if (fromType == null)
            {
                throw new ArgumentNullException(nameof(fromType));
            }

            return GetDefinition(uiHint, fromType);
        }

        /// <summary>
        /// Gets the type of the nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type of the nullable.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        private static Type GetNullableType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsValueType ? typeof(Nullable<>).MakeGenericType(type) : type;
        }

        /// <summary>
        /// Gets the supported hints.
        /// </summary>
        /// <returns>The supported hints.</returns>
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
            addHint(typeof(string), DataTypeDefinition.MediaPicker); // Umbraco Media Picker uses Umbraco.MultipleMediaPicker which is string. Legacy Media Picker used int
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

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <param name="uiHint">The UI hint.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>The definition.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="uiHint" /> is <c>null</c> or white space.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromType" /> is <c>null</c>.</exception>
        private IDataTypeDefinition GetDefinition(string uiHint, Type fromType)
        {
            if (string.IsNullOrWhiteSpace(uiHint))
            {
                throw new ArgumentException("UI hint cannot be null or white space.", nameof(uiHint));
            }

            if (fromType == null)
            {
                throw new ArgumentNullException(nameof(fromType));
            }

            KeyValuePair<Type, DataTypeDefinition> v;

            if (!_supportedHints.TryGetValue(uiHint, out v))
            {
                return null;
            }

            if (v.Key != fromType)
            {
                return GetNullableType(v.Key) == fromType ? GetDefinition(v.Value) : null;
            }

            return GetDefinition(v.Value);
        }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <param name="dataTypeDefinition">The data type definition.</param>
        /// <returns>The definition.</returns>
        private IDataTypeDefinition GetDefinition(DataTypeDefinition dataTypeDefinition)
        {
            IDataTypeDefinition v;

            if (_mappedDefinitions.TryGetValue(dataTypeDefinition, out v))
            {
                return v;
            }

            v = _dataTypeService.GetDataTypeDefinitionById((int)dataTypeDefinition);

            if (v == null)
            {
                return null;
            }

            _mappedDefinitions.Add(dataTypeDefinition, v);

            return v;
        }
    }
}
