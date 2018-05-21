// <copyright file="ComposableContentTypeModel{TModelTypeAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The <see cref="ComposableContentTypeModel{TModelTypeAttribute}" /> class.
    /// </summary>
    /// <typeparam name="TModelTypeAttribute">The model type attribute type.</typeparam>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ComposableContentTypeModel<TModelTypeAttribute> : ContentTypeModel<TModelTypeAttribute>
        where TModelTypeAttribute : ComposableContentTypeModelTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComposableContentTypeModel{TModelTypeAttribute}" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ComposableContentTypeModel(Type modelType)
            : base(modelType)
        {
            ParentNodeType = GetParentNodeType();
        }

        /// <summary>
        /// Gets a value indicating whether content of this type can be created at the root of the content tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allowed as root; otherwise, <c>false</c>.
        /// </value>
        public bool AllowedAsRoot => Attribute.AllowedAsRoot;

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        public string Thumbnail => Attribute.Thumbnail;

        /// <summary>
        /// Gets the allowed child node types.
        /// </summary>
        /// <value>
        /// The allowed child node types.
        /// </value>
        public Type[] AllowedChildNodeTypes => Attribute.AllowedChildNodeTypes?.Distinct().ToArray() ?? new Type[] { };

        /// <summary>
        /// Gets the composition node types.
        /// </summary>
        /// <value>
        /// The composition node types.
        /// </value>
        public Type[] CompositionNodeTypes => Attribute.CompositionNodeTypes?.Distinct().ToArray() ?? new Type[] { };

        /// <summary>
        /// Gets the parent node type.
        /// </summary>
        /// <value>
        /// The parent node type.
        /// </value>
        public Type ParentNodeType { get; }

        /// <inheritdoc />
        protected override IEnumerable<PropertyTypeModel> GetProperties()
        {
            /*
             * Only include public instance properties declared for the model type in question. The type hierarchy
             * is flattened down to model types. Inheritance for model types is handled by Umbraco, and adding
             * inherited properties will cause collisions.
             */
            var properties = GetInheritance()[ModelType].SelectMany(type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return properties.Where(IsValidProperty).Select(property => new PropertyTypeModel(property));
        }

        private Type GetParentNodeType()
        {
            var baseType = ModelType.BaseType;

            for (var type = baseType; type != null; type = type.BaseType)
            {
                if (type.IsModelType<TModelTypeAttribute>())
                {
                    return type;
                }
            }

            return null;
        }

        private IDictionary<Type, IEnumerable<Type>> GetInheritance()
        {
            var inheritance = new Dictionary<Type, IEnumerable<Type>>();
            List<Type> types = null;

            // TODO: Rewrite. As soon as another model type is found in the inheritance chain, we should return.
            for (var type = ModelType; type != null; type = type.BaseType)
            {
                if (type.IsModelType<TModelTypeAttribute>())
                {
                    inheritance.Add(type, new List<Type> { type });
                    types = (List<Type>)inheritance[type];
                }
                else
                {
                    types?.Add(type);
                }
            }

            return inheritance;
        }
    }
}