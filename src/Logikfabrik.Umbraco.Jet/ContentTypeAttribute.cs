// <copyright file="ContentTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="ContentTypeAttribute" /> class.
    /// </summary>
    public abstract class ContentTypeAttribute : IdAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name" /> is <c>null</c> or white space.</exception>
        protected ContentTypeAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or white space.", nameof(name));
            }

            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeAttribute"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name" /> is <c>null</c> or white space.</exception>
        protected ContentTypeAttribute(string id, string name)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or white space.", nameof(name));
            }

            Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string Icon
        {
            get; set;
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
        /// Gets a value indicating whether content of this type can be created at the root of the content tree.
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
    }
}