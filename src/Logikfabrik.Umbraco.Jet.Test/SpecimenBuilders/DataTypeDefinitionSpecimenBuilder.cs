namespace Logikfabrik.Umbraco.Jet.Test.SpecimenBuilders
{
    using System;
    using AutoFixture;
    using AutoFixture.Kernel;
    using global::Umbraco.Core.Models;
    using Moq;

    public class DataTypeDefinitionSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;

            if (type == null)
            {
                return new NoSpecimen();
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (type != typeof(IDataTypeDefinition))
            {
                return new NoSpecimen();
            }

            var definition = new Mock<IDataTypeDefinition>();

            definition.SetupAllProperties();

            definition.Object.DatabaseType = context.Create<DataTypeDatabaseType>();
            definition.Object.PropertyEditorAlias = context.Create<string>();
            definition.Object.Id = context.Create<int>();
            definition.Object.Name = context.Create<string>();

            return definition.Object;
        }
    }
}
