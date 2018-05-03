namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class MediaTypeModelTypeBuilderSpecimenBuilder : SpecimenBuilder<MediaTypeModelTypeBuilder>
    {
        protected override MediaTypeModelTypeBuilder Create(ISpecimenContext context)
        {
            return new MediaTypeModelTypeBuilder(context.Create<string>(), context.Create<string>());
        }
    }
}
