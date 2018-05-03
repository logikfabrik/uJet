namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class DocumentTypeModelTypeBuilderSpecimenBuilder : SpecimenBuilder<DocumentTypeModelTypeBuilder>
    {
        protected override DocumentTypeModelTypeBuilder Create(ISpecimenContext context)
        {
            return new DocumentTypeModelTypeBuilder(context.Create<string>(), context.Create<string>());
        }
    }
}
