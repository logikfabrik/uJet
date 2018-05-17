// <copyright file="ModelTypeBuilderExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection.Emit;

    public static class ModelTypeBuilderExtensions
    {
        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, string name, Type type, object defaultValue)
            where TModelAttribute : ModelTypeAttribute
        {
            AddProperty(builder, Scope.Public, Accessors.GetSet, name, type, defaultValue);
        }

        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, Scope scope, Accessors accessors, string name, Type type, object defaultValue)
            where TModelAttribute : ModelTypeAttribute
        {
            var attributeBuilders = new[]
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                new CustomAttributeBuilder(typeof(DefaultValueAttribute).GetConstructor(new[] { typeof(Type), typeof(string) }), new[] { type, defaultValue })
            };

            builder.AddProperty(scope, accessors, name, type, attributeBuilders);
        }

        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, string name, Type type, Guid id)
            where TModelAttribute : ModelTypeAttribute
        {
            AddProperty(builder, Scope.Public, Accessors.GetSet, name, type, id);
        }

        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, Scope scope, Accessors accessors, string name, Type type, Guid id)
            where TModelAttribute : ModelTypeAttribute
        {
            var attributeBuilders = new[]
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                new CustomAttributeBuilder(typeof(IdAttribute).GetConstructor(new[] { typeof(string) }), new object[] { id.ToString() })
            };

            builder.AddProperty(scope, accessors, name, type, attributeBuilders);
        }

        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, string name, Type type, string alias)
            where TModelAttribute : ModelTypeAttribute
        {
            AddProperty(builder, Scope.Public, Accessors.GetSet, name, type, alias);
        }

        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, Scope scope, Accessors accessors, string name, Type type, string alias)
            where TModelAttribute : ModelTypeAttribute
        {
            var attributeBuilders = new[]
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                new CustomAttributeBuilder(typeof(AliasAttribute).GetConstructor(new[] { typeof(string) }), new object[] { alias })
            };

            builder.AddProperty(scope, accessors, name, type, attributeBuilders);
        }

        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, string name, Type type, bool scaffold)
            where TModelAttribute : ModelTypeAttribute
        {
            AddProperty(builder, Scope.Public, Accessors.GetSet, name, type, scaffold);
        }

        public static void AddProperty<TModelAttribute>(this ModelTypeBuilder<TModelAttribute> builder, Scope scope, Accessors accessors, string name, Type type, bool scaffold)
            where TModelAttribute : ModelTypeAttribute
        {
            var attributeBuilders = new[]
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                new CustomAttributeBuilder(typeof(ScaffoldColumnAttribute).GetConstructor(new[] { typeof(bool) }), new object[] { scaffold })
            };

            builder.AddProperty(scope, accessors, name, type, attributeBuilders);
        }
    }
}
