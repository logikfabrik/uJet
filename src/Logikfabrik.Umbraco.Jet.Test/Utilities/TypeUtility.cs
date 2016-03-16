// <copyright file="TypeUtility.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
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
        /// <summary>
        /// Gets the type attributes.
        /// </summary>
        /// <param name="typeAttributes">The type attributes, if any.</param>
        /// <returns>The type attributes.</returns>
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

        /// <summary>
        /// Gets a type builder.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="typeAttributes">The type attributes.</param>
        /// <returns>A type builder.</returns>
        public static TypeBuilder GetTypeBuilder(string typeName, TypeAttributes typeAttributes)
        {
            var assemblyName = new AssemblyName("Logikfabrik.Umbraco.Jet.Test.Types");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var typeBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name)
                .DefineType(typeName, typeAttributes);

            return typeBuilder;
        }
    }
}
