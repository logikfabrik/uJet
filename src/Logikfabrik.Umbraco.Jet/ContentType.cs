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
        private readonly Lazy<IDictionary<Type, IEnumerable<Type>>> _composition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentType{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        protected ContentType(Type type)
            : base(type)
        {
            _composition = new Lazy<IDictionary<Type, IEnumerable<Type>>>(GetComposition);

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
        public IDictionary<Type, IEnumerable<Type>> Composition => _composition.Value;

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <returns>The properties.</returns>
        protected override IEnumerable<TypeProperty> GetProperties()
        {
            var properties = Composition[Type].SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return from property in properties where IsValidProperty(property) select new TypeProperty(property);
        }

        /// <summary>
        /// Gets the composition.
        /// </summary>
        /// <returns>The composition.</returns>
        protected abstract IDictionary<Type, IEnumerable<Type>> GetComposition();

        /// <summary>
        /// Gets the composition for the specified type.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The composition for the specified type.</returns>
        protected IDictionary<Type, IEnumerable<Type>> GetComposition(Func<Type, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var composition = new Dictionary<Type, IEnumerable<Type>>();
            List<Type> compositionTypes = null;

            foreach (var t in GetInheritance())
            {
                if (predicate(t))
                {
                    composition.Add(t, new List<Type> { t });
                    compositionTypes = (List<Type>)composition[t];
                }
                else
                {
                    compositionTypes?.Add(t);
                }
            }

            return composition;
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

        private IEnumerable<Type> GetInheritance()
        {
            for (var t = Type; t != null; t = t.BaseType)
            {
                yield return t;
            }
        }
    }
}