// <copyright file="DocumentTypeUtility.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// The <see cref="DocumentTypeUtility" /> class.
    /// </summary>
    public static class DocumentTypeUtility
    {
        /// <summary>
        /// Gets a type builder.
        /// </summary>
        /// <param name="typeAttributes">The type attributes.</param>
        /// <returns>A type builder.</returns>
        public static TypeBuilder GetTypeBuilder(TypeAttributes? typeAttributes = null)
        {
            var typeBuilder = TypeUtility.GetTypeBuilder("MyDocumentType", TypeUtility.GetTypeAttributes(typeAttributes));

            var constructor = typeof(DocumentTypeAttribute).GetConstructor(new[] { typeof(string) });

            if (constructor == null)
            {
                return null;
            }

            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));

            return typeBuilder;
        }
    }
}
