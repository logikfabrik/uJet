// <copyright file="DataTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using EnsureThat;

    /// <summary>
    /// The <see cref="DataTypeAttribute" /> class. Attribute for model type annotation.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]

    // ReSharper disable once InheritdocConsiderUsage
    public class DataTypeAttribute : ModelTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="editor">The editor.</param>
        // ReSharper disable once InheritdocConsiderUsage
        // ReSharper disable once UnusedMember.Global
        public DataTypeAttribute(Type type, string editor)
        {
            Ensure.That(type).IsNotNull();
            Ensure.That(editor).IsNotNullOrWhiteSpace();

            Type = type;
            Editor = editor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="editor">The editor.</param>
        // ReSharper disable once InheritdocConsiderUsage
        // ReSharper disable once UnusedMember.Global
        public DataTypeAttribute(string id, Type type, string editor)
            : base(id)
        {
            Ensure.That(type).IsNotNull();
            Ensure.That(editor).IsNotNullOrWhiteSpace();

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