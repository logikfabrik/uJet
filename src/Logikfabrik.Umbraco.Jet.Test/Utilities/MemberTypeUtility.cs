// <copyright file="MemberTypeUtility.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class MemberTypeUtility
    {
        public static TypeBuilder GetTypeBuilder(TypeAttributes? typeAttributes = null)
        {
            var typeBuilder = TypeUtility.GetTypeBuilder("MyDataType", TypeUtility.GetTypeAttributes(typeAttributes));

            var constructor = typeof(MemberTypeAttribute).GetConstructor(new Type[] { });

            if (constructor == null)
            {
                return null;
            }

            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(constructor, new object[] { }));

            return typeBuilder;
        }
    }
}
