namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public class TypeUtility
    {
        public delegate CustomAttributeBuilder GetAttributeBuilder<T>() where T : Attribute;

        public static Type GetTypeWithoutPublicDefaultConstructor<T>(string typeName, GetAttributeBuilder<T> attributeBuilder) where T : Attribute
        {
            var typeBuilder = GetTypeBuilder(typeName, TypeAttributes.Public | TypeAttributes.Class, attributeBuilder);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);

            return typeBuilder.CreateType();
        }

        public static Type GetAbstractType<T>(string typeName, GetAttributeBuilder<T> attributeBuilder) where T : Attribute
        {
            var typeBuilder = GetTypeBuilder(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Abstract, attributeBuilder);

            return typeBuilder.CreateType();
        }

        public static Type GetType<T>(string typeName, GetAttributeBuilder<T> attributeBuilder) where T : Attribute
        {
            var typeBuilder = GetTypeBuilder(typeName, TypeAttributes.Public | TypeAttributes.Class, attributeBuilder);

            return typeBuilder.CreateType();
        }

        private static TypeBuilder GetTypeBuilder<T>(string typeName, TypeAttributes typeAttributes, GetAttributeBuilder<T> attributeBuilder) where T : Attribute
        {
            var typeBuilder = GetTypeBuilder(new AssemblyName("Logikfabrik.Umbraco.Jet.Test.Types"), typeName, typeAttributes);

            typeBuilder.SetCustomAttribute(attributeBuilder());

            return typeBuilder;
        }

        private static TypeBuilder GetTypeBuilder(AssemblyName assemblyName, string typeName, TypeAttributes typeAttributes)
        {
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            return assemblyBuilder.DefineDynamicModule(assemblyName.Name).DefineType(typeName, typeAttributes);
        }
    }
}
