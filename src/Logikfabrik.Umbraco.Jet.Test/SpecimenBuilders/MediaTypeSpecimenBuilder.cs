namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class MediaTypeSpecimenBuilder : SpecimenBuilder<MediaType>
    {
        protected override MediaType Create(ISpecimenContext context)
        {
            return new MediaType(context.Create<MediaTypeModelTypeBuilder>().CreateType());
        }
    }
}
