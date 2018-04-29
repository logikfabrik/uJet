// <copyright file="ComposableContentTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="ComposableContentTypeAttribute" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ComposableContentTypeAttribute : ContentTypeModelAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComposableContentTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ComposableContentTypeAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComposableContentTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ComposableContentTypeAttribute(string id, string name)
            : base(id, name)
        {
        }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>
        /// The thumbnail.
        /// </value>
        public string Thumbnail
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether content of this type can be created at the root of the content tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allowed as root; otherwise, <c>false</c>.
        /// </value>
        public bool AllowedAsRoot
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the allowed child node types.
        /// </summary>
        /// <value>
        /// The allowed child node types.
        /// </value>
        public Type[] AllowedChildNodeTypes
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the composition node types.
        /// </summary>
        /// <value>
        /// The composition node types.
        /// </value>
        public Type[] CompositionNodeTypes
        {
            get; set;
        }
    }
}