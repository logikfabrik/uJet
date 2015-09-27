// <copyright file="MediaTypeUtility.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System.Reflection;
    using System.Reflection.Emit;

    public static class MediaTypeUtility
    {
        public static TypeBuilder GetTypeBuilder(TypeAttributes? typeAttributes = null)
        {
            var typeBuilder = TypeUtility.GetTypeBuilder("MyMediaType", TypeUtility.GetTypeAttributes(typeAttributes));

            var constructor = typeof(MediaTypeAttribute).GetConstructor(new[] { typeof(string) });

            if (constructor == null)
            {
                return null;
            }

            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { "Name" }));

            return typeBuilder;
        }
    }
}
