// <copyright file="DataType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// Data type.
    /// </summary>
    public class DataType
    {
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
        /// Gets the ID for this data type.
        /// </summary>
        public Guid? Id { get; }

        /// <summary>
        /// Gets the name of this data type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the editor for this data type.
        /// </summary>
        public string Editor { get; }

        /// <summary>
        /// Gets the database type for this data type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the data type ID from the given type.
        /// </summary>
        /// <param name="attribute">The data type attribute of the underlying type.</param>
        /// <returns>A data type ID.</returns>
        private static Guid? GetId(IdAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Id;
        }

        /// <summary>
        /// Gets the data type name from the given type.
        /// </summary>
        /// <param name="type">The underlying type.</param>
        /// <returns>A data type name.</returns>
        private static string GetName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.Name;
        }

        /// <summary>
        /// Gets the data type editor from the given type.
        /// </summary>
        /// <param name="attribute">The data type attribute of the underlying type.</param>
        /// <returns>A data type editor.</returns>
        private static string GetEditor(DataTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Editor;
        }

        /// <summary>
        /// Gets the data type type from the given type.
        /// </summary>
        /// <param name="attribute">The data type attribute of the underlying type.</param>
        /// <returns>A data type type.</returns>
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