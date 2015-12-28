// <copyright file="ContentType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The <see cref="ContentType{T}" /> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
    public abstract class ContentType<T> : BaseType<T>
        where T : ContentTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentType{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="composition">The type composition.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="composition" /> is <c>null</c>.</exception>
        protected ContentType(Type type, IDictionary<Type, IEnumerable<Type>> composition)
            : base(type)
        {
            if (composition == null)
            {
                throw new ArgumentNullException(nameof(composition));
            }

            Composition = composition;

            var attribute = type.GetCustomAttribute<T>();

            Thumbnail = GetThumbnail(attribute);
            AllowedAsRoot = GetAllowedAsRoot(attribute);
            AllowedChildNodeTypes = GetAllowedChildNodeTypes(attribute);
        }

        /// <summary>
        /// Gets a value indicating whether content of this type can be created at the root of the content tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allowed as root; otherwise, <c>false</c>.
        /// </value>
        public bool AllowedAsRoot { get; }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        public string Thumbnail { get; }

        /// <summary>
        /// Gets the allowed child node types.
        /// </summary>
        /// <value>
        /// The allowed child node types.
        /// </value>
        public IEnumerable<Type> AllowedChildNodeTypes { get; }

        /// <summary>
        /// Gets the composition.
        /// </summary>
        /// <value>
        /// The composition.
        /// </value>
        public IDictionary<Type, IEnumerable<Type>> Composition { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The properties.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type" /> is <c>null</c>.</exception>
        protected override IEnumerable<TypeProperty> GetProperties(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var types = Composition[type];
            var properties = types.SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var property in properties)
            {
                if (!IsValidProperty(property))
                {
                    continue;
                }

                yield return new TypeProperty(property);
            }
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The thumbnail.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static string GetThumbnail(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.Thumbnail;
        }

        /// <summary>
        /// Gets a value indicating whether content of this type can be created at the root of the content tree.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>
        ///   <c>true</c> if allowed as root; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static bool GetAllowedAsRoot(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.AllowedAsRoot;
        }

        /// <summary>
        /// Gets the allowed child node types.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The allowed child node types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static IEnumerable<Type> GetAllowedChildNodeTypes(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.AllowedChildNodeTypes?.Where(t => t.GetCustomAttribute<T>(false) != null) ?? new Type[] { };
        }
    }
}