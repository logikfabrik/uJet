//// <copyright file="TypeBuilderExtensions.cs" company="Logikfabrik">
////   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
//// </copyright>

//namespace Logikfabrik.Umbraco.Jet.Test.Utilities
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Reflection;
//    using System.Reflection.Emit;

//    public static class TypeBuilderExtensions
//    {
//        public static void AddPublicReadOnlyProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
//        {
//            AddReadOnlyProperty(typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
//        }

//        public static void AddPublicWriteOnlyProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
//        {
//            AddWriteOnlyProperty(typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
//        }

//        public static void AddPrivateProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
//        {
//            AddProperty(typeBuilder, MethodAttributes.Private | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
//        }

//        public static void AddPublicProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
//        {
//            AddProperty(typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType);
//        }

//        public static void AddPublicProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType, IEnumerable<CustomAttributeBuilder> propertyAttributeBuilders)
//        {
//            AddProperty(typeBuilder, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyName, propertyType, propertyAttributeBuilders);
//        }

//        public static void SetPublicProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
//        {
//            var fieldBuilder = typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);

//            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);

//            var constructorGenerator = constructorBuilder.GetILGenerator();

//            constructorGenerator.Emit(OpCodes.Ldarg_0);
//            constructorGenerator.Emit(OpCodes.Newobj, propertyType.GetConstructor(Type.EmptyTypes));
//            constructorGenerator.Emit(OpCodes.Stfld, fieldBuilder);
//            constructorGenerator.Emit(OpCodes.Ldarg_0);
//            constructorGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
//            constructorGenerator.Emit(OpCodes.Ret);
//        }



        
//    }
//}
