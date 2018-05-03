// <copyright file="MemberTypeModelTypeBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Utilities
{
    using System;

    public class MemberTypeModelTypeBuilder : ContentTypeModelTypeBuilder<MemberTypeAttribute>
    {
        public MemberTypeModelTypeBuilder(string typeName, string name)
            : base(typeName, name)
        {
        }

        public MemberTypeModelTypeBuilder(string typeName, Guid id, string name)
            : base(typeName, id, name)
        {
        }
    }
}
