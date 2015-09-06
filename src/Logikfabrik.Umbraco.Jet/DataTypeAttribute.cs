// <copyright file="DataTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// Data type attribute.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class DataTypeAttribute : IdAttribute
    {
        public DataTypeAttribute(Type type, string editor)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(editor))
            {
                throw new ArgumentException("Editor cannot be null or white space.", nameof(editor));
            }

            Type = type;
            Editor = editor;
        }

        public DataTypeAttribute(string id, Type type, string editor)
            : base(id)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(editor))
            {
                throw new ArgumentException("Editor cannot be null or white space.", nameof(editor));
            }

            Type = type;
            Editor = editor;
        }

        /// <summary>
        /// Gets the name of this data type attribute.
        /// </summary>
        public string Editor { get; }

        /// <summary>
        /// Gets the type of this data type attribute.
        /// </summary>
        public Type Type { get; }
    }
}
