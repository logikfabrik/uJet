// <copyright file="DocumentTypeModelTypeBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Linq;

    public class DocumentTypeModelTypeBuilder : ComposableContentTypeModelTypeBuilder<DocumentTypeAttribute>
    {
        public DocumentTypeModelTypeBuilder(string typeName, string name)
            : base(typeName, name)
        {
        }

        public DocumentTypeModelTypeBuilder(string typeName, Guid id, string name)
            : base(typeName, id, name)
        {
        }

        public string[] Templates { get; set; } = { };

        public string DefaultTemplate { get; set; }

        protected override string[] GetAttributePropertyNames()
        {
            return base.GetAttributePropertyNames().Concat(new[]
            {
                GetPropertyName(() => Templates),
                GetPropertyName(() => DefaultTemplate)
            }).ToArray();
        }
    }
}
