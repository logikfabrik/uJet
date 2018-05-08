// <copyright file="ContentTypeModelTypeBuilder{T}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Linq;

    public abstract class ContentTypeModelTypeBuilder<T> : ModelTypeBuilder<T>
        where T : ContentTypeModelAttribute
    {
        protected ContentTypeModelTypeBuilder(string typeName, string name)
            : base(typeName)
        {
            Name = name;
        }

        protected ContentTypeModelTypeBuilder(string typeName, Guid id, string name)
            : base(typeName, id)
        {
            Name = name;
        }

        public string Name { get; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public bool IsContainer { get; set; }

        public string Alias { get; set; }

        protected override object[] GetModelAttributeConstructorArguments()
        {
            return base.GetModelAttributeConstructorArguments().Concat(new[] { Name }).ToArray();
        }

        protected override string[] GetModelAttributePropertyNames()
        {
            return new[]
            {
                GetPropertyName(() => Description),
                GetPropertyName(() => Icon),
                GetPropertyName(() => IsContainer),
                GetPropertyName(() => Alias)
            };
        }
    }
}
