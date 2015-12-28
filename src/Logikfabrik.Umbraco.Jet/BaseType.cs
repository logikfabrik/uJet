﻿// <copyright file="BaseType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// The <see cref="BaseType{T}" /> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="BaseTypeAttribute" /> type.</typeparam>
    public abstract class BaseType<T>
        where T : BaseTypeAttribute
    {
        private readonly Lazy<IEnumerable<TypeProperty>> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseType{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        protected BaseType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type;
            Alias = GetAlias(type);
            _properties = new Lazy<IEnumerable<TypeProperty>>(() => GetProperties(type));

            var attribute = type.GetCustomAttribute<T>();

            Name = GetName(attribute);
            Id = GetId(attribute);
            Icon = GetIcon(attribute);
            Description = GetDescription(attribute);
        }

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
        /// Gets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public IEnumerable<TypeProperty> Properties => _properties.Value;

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string Icon { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The properties.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        protected virtual IEnumerable<TypeProperty> GetProperties(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var property in type.GetProperties())
            {
                if (!IsValidProperty(property))
                {
                    continue;
                }

                yield return new TypeProperty(property);
            }
        }

        /// <summary>
        /// Determines whether the specified property is valid.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        protected bool IsValidProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!property.CanRead || !property.CanWrite)
            {
                return false;
            }

            var attribute = property.GetCustomAttribute<ScaffoldColumnAttribute>();

            return attribute == null || attribute.Scaffold;
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The alias.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        private static string GetAlias(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.Name.Alias();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static string GetName(T attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Name;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static Guid? GetId(T attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Id;
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The icon.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static string GetIcon(T attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Icon;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The description.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static string GetDescription(T attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Description;
        }
    }
}
