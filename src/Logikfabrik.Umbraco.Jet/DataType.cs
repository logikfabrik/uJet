// <copyright file="DataType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// The <see cref="DataType" /> class.
    /// </summary>
    public class DataType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="type" /> is not a data type.</exception>
        public DataType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsDataType())
            {
                throw new ArgumentException("Type is not a data type.", nameof(type));
            }

            Name = GetName(type);

            var attribute = type.GetCustomAttribute<DataTypeAttribute>();

            Id = GetId(attribute);
            Editor = GetEditor(attribute);
            Type = GetType(attribute);
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid? Id { get; }

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
        public string Editor { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static Guid? GetId(IdAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Id;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The name.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        private static string GetName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.Name;
        }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The editor.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static string GetEditor(DataTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Editor;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static Type GetType(DataTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Type;
        }
    }
}