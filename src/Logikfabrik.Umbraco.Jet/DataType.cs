// <copyright file="DataType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="DataType" /> class. The model type for data types.
    /// </summary>
    public class DataType : TypeModel<DataTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataType" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        public DataType(Type modelType)
            : base(modelType)
        {
            Name = modelType.Name;
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
    }
}