// <copyright file="ModelTypeBuilder{TModelAttribute}.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;

    public abstract class ModelTypeBuilder<TModelAttribute>
        where TModelAttribute : TypeModelAttribute
    {
        private readonly object[] _arguments;
        private readonly IList<FieldBuilder> _fieldBuilders;
        private TypeBuilder _typeBuilder;

        protected ModelTypeBuilder(string typeName)
        {
            TypeName = typeName;

            _arguments = new object[] { };
            _fieldBuilders = new List<FieldBuilder>();
        }

        protected ModelTypeBuilder(string typeName, Guid id)
        {
            TypeName = typeName;
            Id = id;

            _arguments = new object[] { id.ToString() };
            _fieldBuilders = new List<FieldBuilder>();
        }

        public bool IsAbstractType { get; set; }

        public string TypeName { get; }

        public Guid Id { get; }

        public Type Create(Scope scope)
        {
            var typeBuilder = GetModelTypeBuilder();

            ConstructorBuilder constructorBuilder;

            switch (scope)
            {
                case Scope.Public:
                    constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
                    break;

                case Scope.Private:
                    constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Private, CallingConventions.Standard, null);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }

            var constructorGenerator = constructorBuilder.GetILGenerator();

            foreach (var fieldBuilder in _fieldBuilders)
            {
                var fieldTypeConstructor = fieldBuilder.FieldType.GetConstructor(Type.EmptyTypes);

                if (fieldTypeConstructor == null)
                {
                    continue;
                }

                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Newobj, fieldTypeConstructor);
                constructorGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            }

            constructorGenerator.Emit(OpCodes.Ldarg_0);
            constructorGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            constructorGenerator.Emit(OpCodes.Ret);

            var type = typeBuilder.CreateType();

            _typeBuilder = null;
            _fieldBuilders.Clear();

            return type;
        }

        public void AddProperty(Scope scope, Accessor accessor, string name, Type type)
        {
            AddProperty(scope, accessor, name, type, null);
        }

        public void AddProperty(Scope scope, Accessor accessor, string name, Type type, IEnumerable<CustomAttributeBuilder> attributeBuilders)
        {
            var propertyAttributes = MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            switch (scope)
            {
                case Scope.Public:
                    propertyAttributes = propertyAttributes | MethodAttributes.Public;
                    break;

                case Scope.Private:
                    propertyAttributes = propertyAttributes | MethodAttributes.Private;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }

            var typeBuilder = GetModelTypeBuilder();

            var fieldBuilder = typeBuilder.DefineField($"_{name}", type, FieldAttributes.Private);

            _fieldBuilders.Add(fieldBuilder);

            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, null);

            if (attributeBuilders != null)
            {
                foreach (var customAttribute in attributeBuilders)
                {
                    propertyBuilder.SetCustomAttribute(customAttribute);
                }
            }

            if (accessor.HasFlag(Accessor.Get))
            {
                var getMethodBuilder = GetPropertyGetter(typeBuilder, fieldBuilder, propertyAttributes, name, type);

                propertyBuilder.SetGetMethod(getMethodBuilder);
            }

            if (accessor.HasFlag(Accessor.Set))
            {
                var setMethodBuilder = GetPropertySetter(typeBuilder, fieldBuilder, propertyAttributes, name, type);

                propertyBuilder.SetSetMethod(setMethodBuilder);
            }
        }

        protected static string GetPropertyName<T>(Expression<Func<T>> expression) => ((MemberExpression)expression.Body).Member.Name;

        protected virtual object[] GetModelAttributeConstructorArguments()
        {
            return _arguments;
        }

        protected abstract string[] GetModelAttributePropertyNames();

        private static MethodBuilder GetPropertyGetter(TypeBuilder typeBuilder, FieldInfo fieldBuilder, MethodAttributes attributes, string propertyName, Type propertyType)
        {
            var getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyName}", attributes, propertyType, Type.EmptyTypes);

            var getGenerator = getMethodBuilder.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getGenerator.Emit(OpCodes.Ret);

            return getMethodBuilder;
        }

        private static MethodBuilder GetPropertySetter(TypeBuilder typeBuilder, FieldInfo fieldBuilder, MethodAttributes attributes, string propertyName, Type propertyType)
        {
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}", attributes, null, new[] { propertyType });

            var setGenerator = setMethodBuilder.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            setGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            setGenerator.Emit(OpCodes.Ret);

            return setMethodBuilder;
        }

        private static TypeBuilder GetTypeBuilder(string typeName, TypeAttributes typeAttributes)
        {
            var assemblyName = new AssemblyName("Logikfabrik.Umbraco.Jet.Test.Types");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var typeBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name).DefineType(typeName, typeAttributes);

            return typeBuilder;
        }

        private TypeBuilder GetModelTypeBuilder()
        {
            if (_typeBuilder != null)
            {
                return _typeBuilder;
            }

            var typeAttributes = TypeAttributes.Class | TypeAttributes.Public;

            if (IsAbstractType)
            {
                typeAttributes = typeAttributes | TypeAttributes.Abstract;
            }

            _typeBuilder = GetTypeBuilder(TypeName, typeAttributes);

            var attibuteBuilder = GetModelAttributeBuilder();

            _typeBuilder.SetCustomAttribute(attibuteBuilder);

            return _typeBuilder;
        }

        private CustomAttributeBuilder GetModelAttributeBuilder()
        {
            var attributeType = typeof(TModelAttribute);

            var attributeConstructorArguments = GetModelAttributeConstructorArguments();

            var constructor = attributeType.GetConstructor(attributeConstructorArguments.Select(arg => arg.GetType()).ToArray());

            var attributePropertyNames = GetModelAttributePropertyNames();

            var attributeProperties = attributePropertyNames.Select(name => attributeType.GetProperty(name)).ToArray();

            // ReSharper disable once PossibleNullReferenceException
            var attributePropertyValues = attributePropertyNames.Select(name => GetType().GetProperty(name).GetValue(this)).ToArray();

            // ReSharper disable once AssignNullToNotNullAttribute
            var attributeBuilder = new CustomAttributeBuilder(constructor, attributeConstructorArguments, attributeProperties, attributePropertyValues);

            return attributeBuilder;
        }
    }
}
