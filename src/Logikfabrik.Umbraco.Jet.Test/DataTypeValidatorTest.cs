namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using AutoFixture.Xunit2;
    using Utilities;
    using Xunit;

    public class DataTypeValidatorTest
    {
        [Theory]
        [AutoData]
        public void CanFindConflictById(string typeName, Guid id, Type type, string editor)
        {
            var modelX = new DataType(new DataTypeModelTypeBuilder(typeName, id.ToString(), type, editor).CreateType());
            var modelY = new DataType(new DataTypeModelTypeBuilder(typeName, id.ToString(), type, editor).CreateType());

            var dataTypeValidator = new DataTypeValidator();

            Assert.Throws<InvalidOperationException>(() => dataTypeValidator.Validate(new[] { modelX, modelY }));
        }

        [Theory]
        [AutoData]
        public void CanFindConflictByName(string typeName, Type type, string editor)
        {
            var modelX = new DataType(new DataTypeModelTypeBuilder(typeName, type, editor).CreateType());
            var modelY = new DataType(new DataTypeModelTypeBuilder(typeName, type, editor).CreateType());

            var dataTypeValidator = new DataTypeValidator();

            Assert.Throws<InvalidOperationException>(() => dataTypeValidator.Validate(new[] { modelX, modelY }));
        }
    }
}
