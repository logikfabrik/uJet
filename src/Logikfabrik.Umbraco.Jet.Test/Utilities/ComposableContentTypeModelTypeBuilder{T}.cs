﻿// <copyright file="ComposableContentTypeModelTypeBuilder{T}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Linq;

    public abstract class ComposableContentTypeModelTypeBuilder<T> : ContentTypeModelTypeBuilder<T>
        where T : ComposableContentTypeModelTypeAttribute
    {
        protected ComposableContentTypeModelTypeBuilder(string typeName, string name)
            : base(typeName, name)
        {
        }

        protected ComposableContentTypeModelTypeBuilder(string typeName, Guid id, string name)
            : base(typeName, id, name)
        {
        }

        public string Thumbnail { get; set; }

        public bool AllowedAsRoot { get; set; }

        public Type[] AllowedChildNodeTypes { get; set; } = { };

        public Type[] CompositionNodeTypes { get; set; } = { };

        protected override string[] GetModelAttributePropertyNames()
        {
            return base.GetModelAttributePropertyNames().Concat(new[]
            {
                GetPropertyName(() => Thumbnail),
                GetPropertyName(() => AllowedAsRoot),
                GetPropertyName(() => AllowedChildNodeTypes),
                GetPropertyName(() => CompositionNodeTypes)
            }).ToArray();
        }
    }
}