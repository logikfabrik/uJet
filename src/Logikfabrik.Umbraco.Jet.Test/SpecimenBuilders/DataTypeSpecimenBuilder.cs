namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Utilities;

    public class DataTypeSpecimenBuilder : SpecimenBuilder<DataType>
    {
        protected override DataType Create(ISpecimenContext context)
        {
            return new DataType(context.Create<DataTypeModelTypeBuilder>().CreateType());
        }
    }
}
