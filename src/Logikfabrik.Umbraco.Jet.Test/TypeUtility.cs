// <copyright file="TypeUtility.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// The <see cref="TypeUtility" /> class.
    /// </summary>
    public class TypeUtility
    {
        /// <summary>
        /// Delegate for type prebuilding.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        public delegate void BuildAction(TypeBuilder typeBuilder);

        /// <summary>
        /// Build action for creating abstract types.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        public static void AbstractTypeBuildAction(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Private);
        }

        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="buildActions">The build actions.</param>
        /// <returns>The created type.</returns>
        public static Type CreateType(string typeName, IEnumerable<BuildAction> buildActions)
        {
            return CreateType(typeName, TypeAttributes.Public | TypeAttributes.Class, buildActions);
        }

        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="typeAttributes">The type attributes.</param>
        /// <param name="buildActions">The build actions.</param>
        /// <returns>The created type.</returns>
        public static Type CreateType(string typeName, TypeAttributes typeAttributes, IEnumerable<BuildAction> buildActions)
        {
            var typeBuilder = GetTypeBuilder(typeName, typeAttributes);

            foreach (var buildAction in buildActions)
            {
                buildAction(typeBuilder);
            }

            return typeBuilder.CreateType();
        }

        private static TypeBuilder GetTypeBuilder(string typeName, TypeAttributes typeAttributes)
        {
            var assemblyName = new AssemblyName("Logikfabrik.Umbraco.Jet.Test.Types");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var typeBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name).DefineType(typeName, typeAttributes);

            return typeBuilder;
        }
    }
}
