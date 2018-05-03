// <copyright file="MemberTypeModelTypeBuilderSpecimenBuilder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class MemberTypeModelTypeBuilderSpecimenBuilder : SpecimenBuilder<MemberTypeModelTypeBuilder>
    {
        protected override MemberTypeModelTypeBuilder Create(ISpecimenContext context)
        {
            return new MemberTypeModelTypeBuilder(context.Create<string>(), context.Create<string>());
        }
    }
}
