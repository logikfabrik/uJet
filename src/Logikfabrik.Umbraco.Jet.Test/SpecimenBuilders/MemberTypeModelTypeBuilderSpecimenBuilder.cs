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
