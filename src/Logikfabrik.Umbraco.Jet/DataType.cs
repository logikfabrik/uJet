// <copyright file="DataType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The <see cref="DataType" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DataType : TypeModel<DataTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataType" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public DataType(Type modelType)
            : base(modelType)
        {
            Name = modelType.Name;
            PreValues = GetPreValues();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        /// <value>
        /// The editor.
        /// </value>
        public string Editor => Attribute.Editor;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type => Attribute.Type;

        /// <summary>
        /// Gets the pre-values.
        /// </summary>
        /// <value>
        /// The pre-values.
        /// </value>
        public IDictionary<string, string> PreValues { get; }

        private IDictionary<string, string> GetPreValues()
        {
            /*
             * Gets the pre-value property (if one exists). Umbraco does not support inheritance or composition for data types,
             * so inherited properties can safely be included, and should be included.
             */
            var property = ModelType.GetProperty("PreValues", BindingFlags.Public | BindingFlags.Instance);

            if (property == null || !property.CanRead)
            {
                return new Dictionary<string, string>();
            }

            if (!typeof(IDictionary<string, string>).IsAssignableFrom(property.PropertyType))
            {
                return new Dictionary<string, string>();
            }

            var preValues = property.GetValue(Activator.CreateInstance(ModelType)) as IDictionary<string, string>;

            return preValues ?? new Dictionary<string, string>();
        }
    }
}