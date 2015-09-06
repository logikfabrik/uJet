// <copyright file="DocumentTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class DocumentTypeAttribute : ContentTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name to use for the new document type attribute.</param>
        public DocumentTypeAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The ID to use for the new document type attribute.</param>
        /// <param name="name">The name to use for the new document type attribute.</param>
        public DocumentTypeAttribute(string id, string name)
            : base(id, name)
        {
        }

        /// <summary>
        /// Gets or sets the available templates (aliases) of this document type attribute.
        /// </summary>
        public string[] Templates { get; set; }

        /// <summary>
        /// Gets or sets the default template (alias) of this document type attribute.
        /// </summary>
        public string DefaultTemplate { get; set; }
    }
}
