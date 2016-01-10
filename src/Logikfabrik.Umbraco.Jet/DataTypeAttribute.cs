// <copyright file="DataTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="DataTypeAttribute" /> class. Attribute for model type annotation.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class DataTypeAttribute : TypeModelAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="editor">The editor.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="editor" /> is <c>null</c> or white space.</exception>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="editor">The editor.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="editor" /> is <c>null</c> or white space.</exception>
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
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        /// <value>
        /// The editor.
        /// </value>
        public string Editor { get; }
    }
}