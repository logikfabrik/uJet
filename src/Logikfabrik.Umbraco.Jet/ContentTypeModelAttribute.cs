// <copyright file="ContentTypeModelAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using EnsureThat;

    /// <summary>
    /// The <see cref="ContentTypeModelAttribute" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ContentTypeModelAttribute : TypeModelAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeModelAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ContentTypeModelAttribute(string name)
        {
            Ensure.That(name).IsNotNullOrWhiteSpace();

            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeModelAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        protected ContentTypeModelAttribute(string id, string name)
            : base(id)
        {
            Ensure.That(name).IsNotNullOrWhiteSpace();

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
        /// Gets or sets a value indicating whether this instance is a container.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a container; otherwise, <c>false</c>.
        /// </value>
        public bool IsContainer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias
        {
            get; set;
        }
    }
}