// <copyright file="ModelTypeBuilder{T}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;

    public abstract class ModelTypeBuilder<T>
        where T : TypeModelAttribute
    {
        private readonly object[] _arguments;

        protected ModelTypeBuilder(string typeName)
        {
            TypeName = typeName;
            _arguments = new object[] { };
        }

        protected ModelTypeBuilder(string typeName, Guid id)
        {
            TypeName = typeName;
            Id = id;
            _arguments = new object[] { id.ToString() };
        }

        public string TypeName { get; }

        public Guid Id { get; }

        public TypeBuilder GetTypeBuilder(TypeAttributes? typeAttributes = null)
        {
            var typeBuilder = GetTypeBuilder(TypeName, GetTypeAttributes(typeAttributes));

            var attibuteBuilder = GetAttributeBuilder();

            typeBuilder.SetCustomAttribute(attibuteBuilder);

            return typeBuilder;
        }

        public Type CreateType(TypeAttributes? typeAttributes = null)
        {
            return GetTypeBuilder(typeAttributes).CreateType();
        }

        protected static string GetPropertyName<T>(Expression<Func<T>> expression) => ((MemberExpression)expression.Body).Member.Name;

        protected virtual object[] GetAttributeConstructorArguments()
        {
            return _arguments;
        }

        protected abstract string[] GetAttributePropertyNames();

        private static TypeBuilder GetTypeBuilder(string typeName, TypeAttributes typeAttributes)
        {
            var assemblyName = new AssemblyName("Logikfabrik.Umbraco.Jet.Test.Types");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var typeBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name).DefineType(typeName, typeAttributes);

            return typeBuilder;
        }

        private CustomAttributeBuilder GetAttributeBuilder()
        {
            var attributeType = typeof(T);

            var attributeConstructorArguments = GetAttributeConstructorArguments();

            var constructor = attributeType.GetConstructor(attributeConstructorArguments.Select(arg => arg.GetType()).ToArray());

            var attributePropertyNames = GetAttributePropertyNames();

            var attributeProperties = attributePropertyNames.Select(name => attributeType.GetProperty(name)).ToArray();

            // ReSharper disable once PossibleNullReferenceException
            var attributePropertyValues = attributePropertyNames.Select(name => GetType().GetProperty(name).GetValue(this)).ToArray();

            // ReSharper disable once AssignNullToNotNullAttribute
            var attributeBuilder = new CustomAttributeBuilder(constructor, attributeConstructorArguments, attributeProperties, attributePropertyValues);

            return attributeBuilder;
        }

        private static TypeAttributes GetTypeAttributes(TypeAttributes? typeAttributes = null)
        {
            if (!typeAttributes.HasValue)
            {
                typeAttributes = TypeAttributes.Class | TypeAttributes.Public;
            }

            if (!typeAttributes.Value.HasFlag(TypeAttributes.Class))
            {
                typeAttributes = typeAttributes | TypeAttributes.Class;
            }

            if (!typeAttributes.Value.HasFlag(TypeAttributes.Public))
            {
                typeAttributes = typeAttributes | TypeAttributes.Public;
            }

            return typeAttributes.Value;
        }
    }
}
