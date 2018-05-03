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
