// <copyright file="MemberTypeSpecimenBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class MemberTypeSpecimenBuilder : SpecimenBuilder<MemberType>
    {
        protected override MemberType Create(ISpecimenContext context)
        {
            return new MemberType(context.Create<MemberTypeModelTypeBuilder>().CreateType());
        }
    }
}
