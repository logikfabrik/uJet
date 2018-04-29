﻿namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Linq;

    public abstract class ComposableContentTypeModelTypeBuilder<T> : ContentTypeModelTypeBuilder<T>
        where T : ComposableContentTypeAttribute
    {
        protected ComposableContentTypeModelTypeBuilder(string typeName, string name)
            : base(typeName, name)
        {
        }

        protected ComposableContentTypeModelTypeBuilder(string typeName, string id, string name)
            : base(typeName, id, name)
        {
        }

        public string Thumbnail { get; set; }

        public bool AllowedAsRoot { get; set; }

        public Type[] AllowedChildNodeTypes { get; set; } = { };

        public Type[] CompositionNodeTypes { get; set; } = { };

        protected override string[] GetAttributePropertyNames()
        {
            return base.GetAttributePropertyNames().Concat(new[]
            {
                GetPropertyName(() => Thumbnail),
                GetPropertyName(() => AllowedAsRoot),
                GetPropertyName(() => AllowedChildNodeTypes),
                GetPropertyName(() => CompositionNodeTypes)
            }).ToArray();
        }
    }
}