namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class TypeBuilderExtensions
    {
        public static void AddPublicReadOnlyProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            AddReadOnlyProperty(typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
        }

        public static void AddPublicWriteOnlyProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            AddWriteOnlyProperty(typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
        }

        public static void AddPrivateProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            AddProperty(typeBuilder, MethodAttributes.Private | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
        }

        public static void AddPublicProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            AddProperty(typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
        }

        private static void AddProperty(TypeBuilder typeBuilder, MethodAttributes attributes, string propertyName, Type propertyType)
        {
            var fieldBuilder = typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

            var getMethodBuilder = GetPropertyGetter(typeBuilder, fieldBuilder, attributes, propertyName, propertyType);

            propertyBuilder.SetGetMethod(getMethodBuilder);

            var setMethodBuilder = GetPropertySetter(typeBuilder, fieldBuilder, attributes, propertyName, propertyType);

            propertyBuilder.SetSetMethod(setMethodBuilder);
        }

        private static void AddReadOnlyProperty(TypeBuilder typeBuilder, MethodAttributes attributes, string propertyName, Type propertyType)
        {
            var fieldBuilder = typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

            var getMethodBuilder = GetPropertyGetter(typeBuilder, fieldBuilder, attributes, propertyName, propertyType);

            propertyBuilder.SetGetMethod(getMethodBuilder);
        }

        private static void AddWriteOnlyProperty(TypeBuilder typeBuilder, MethodAttributes attributes, string propertyName, Type propertyType)
        {
            var fieldBuilder = typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

            var setMethodBuilder = GetPropertySetter(typeBuilder, fieldBuilder, attributes, propertyName, propertyType);

            propertyBuilder.SetSetMethod(setMethodBuilder);
        }

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
    }
}
