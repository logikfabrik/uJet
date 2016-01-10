// <copyright file="DocumentType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The <see cref="DocumentType" /> class. The model type for document types.
    /// </summary>
    public class DocumentType : ComposableContentTypeModel<DocumentTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentType" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        public DocumentType(Type modelType)
            : base(modelType)
        {
        }

        /// <summary>
        /// Gets the default template.
        /// </summary>
        /// <value>
        /// The default template.
        /// </value>
        public string DefaultTemplate => Attribute.DefaultTemplate;

        /// <summary>
        /// Gets the templates.
        /// </summary>
        /// <value>
        /// The templates.
        /// </value>
        public IEnumerable<string> Templates => Attribute.Templates?.Distinct(StringComparer.InvariantCultureIgnoreCase) ?? new string[] { };
    }
}