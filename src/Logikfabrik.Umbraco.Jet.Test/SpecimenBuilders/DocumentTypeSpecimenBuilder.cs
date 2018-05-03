namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class DocumentTypeSpecimenBuilder : SpecimenBuilder<DocumentType>
    {
        protected override DocumentType Create(ISpecimenContext context)
        {
            return new DocumentType(context.Create<DocumentTypeModelTypeBuilder>().CreateType());
        }
    }
}
