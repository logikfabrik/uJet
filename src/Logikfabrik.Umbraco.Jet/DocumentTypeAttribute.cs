// <copyright file="DocumentTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="DocumentTypeAttribute" /> class. Attribute for document model type annotation.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class DocumentTypeAttribute : ContentTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DocumentTypeAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public DocumentTypeAttribute(string id, string name)
            : base(id, name)
        {
        }

        /// <summary>
        /// Gets or sets the templates.
        /// </summary>
        /// <value>
        /// The templates.
        /// </value>
        public string[] Templates { get; set; }

        /// <summary>
        /// Gets or sets the default template.
        /// </summary>
        /// <value>
        /// The default template.
        /// </value>
        public string DefaultTemplate { get; set; }
    }
}
