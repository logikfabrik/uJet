// <copyright file="TypeUtility.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// The <see cref="TypeUtility" /> class.
    /// </summary>
    public class TypeUtility
    {
        public static TypeAttributes GetTypeAttributes(TypeAttributes? typeAttributes = null)
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

        public static TypeBuilder GetTypeBuilder(string typeName, TypeAttributes typeAttributes)
        {
            var assemblyName = new AssemblyName("Logikfabrik.Umbraco.Jet.Test.Types");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var typeBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name)
                .DefineType(typeName, typeAttributes);

            return typeBuilder;
        }

        public static void AddProperty<T>(TypeBuilder myTypeBuilder, string propertyName, T propertyValue)
        {
            var fieldBuilder = myTypeBuilder.DefineField(char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1), typeof(T), FieldAttributes.Private);

            var propertyBuilder = myTypeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, typeof(T),
                null);

            const MethodAttributes methodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var getMethodBuilder = myTypeBuilder.DefineMethod(string.Concat("get_", propertyName), methodAttributes, typeof(T), Type.EmptyTypes);
            var getGenerator = getMethodBuilder.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getGenerator.Emit(OpCodes.Ret);

            var setMethodBuilder = myTypeBuilder.DefineMethod(string.Concat("set_", propertyName), methodAttributes, null, new[] { typeof(T) });
            var setGenerator = setMethodBuilder.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            setGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            setGenerator.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }

        public static T GetPropertyValue<T>(object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
