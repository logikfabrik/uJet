// <copyright file="ContentType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    /// <summary>
    /// The <see cref="ContentType{T}" /> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="ContentTypeAttribute" /> type.</typeparam>
    public abstract class ContentType<T> : BaseType<T>
        where T : ContentTypeAttribute
    {
        private readonly Lazy<Type> _parentNodeType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentType{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        protected ContentType(Type type)
            : base(type)
        {
            _parentNodeType = new Lazy<Type>(GetParentNodeType);

            var attribute = type.GetCustomAttribute<T>();

            Thumbnail = GetThumbnail(attribute);
            AllowedAsRoot = GetAllowedAsRoot(attribute);
            AllowedChildNodeTypes = GetAllowedChildNodeTypes(attribute);
            CompositionNodeTypes = GetCompositionNodeTypes(attribute);
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
        /// Gets the composition node types.
        /// </summary>
        /// <value>
        /// The composition node types.
        /// </value>
        public IEnumerable<Type> CompositionNodeTypes { get; }

        /// <summary>
        /// Gets the parent node type.
        /// </summary>
        /// <value>
        /// The parent node type.
        /// </value>
        public Type ParentNodeType => _parentNodeType.Value;

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <returns>The properties.</returns>
        protected override IEnumerable<TypeProperty> GetProperties()
        {
            var properties = GetInheritance()[Type].SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return from property in properties where IsValidProperty(property) select new TypeProperty(property);
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

            return attribute.AllowedChildNodeTypes?.Where(t => t.IsContentType<T>()) ?? new Type[] { };
        }

        /// <summary>
        /// Gets the composition node types.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns>The composition node types.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="attribute" /> is <c>null</c>.</exception>
        private static IEnumerable<Type> GetCompositionNodeTypes(ContentTypeAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            return attribute.CompositionNodeTypes?.Where(t => t.IsContentType<T>()) ?? new Type[] { };
        }

        /// <summary>
        /// Gets the parent node type.
        /// </summary>
        /// <returns>The parent node type.</returns>
        private Type GetParentNodeType()
        {
            var baseType = Type.BaseType;

            for (var t = baseType; t != null; t = t.BaseType)
            {
                if (t.IsContentType<T>())
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the inheritance.
        /// </summary>
        /// <returns>The inheritance.</returns>
        private IDictionary<Type, IEnumerable<Type>> GetInheritance()
        {
            var inheritance = new Dictionary<Type, IEnumerable<Type>>();
            List<Type> types = null;

            for (var t = Type; t != null; t = t.BaseType)
            {
                if (t.IsContentType<T>())
                {
                    inheritance.Add(t, new List<Type> { t });
                    types = (List<Type>)inheritance[t];
                }
                else
                {
                    types?.Add(t);
                }
            }

            return inheritance;
        }
    }
}