// <copyright file="DataTypeValidatorTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DataTypeValidatorTest
    {
        [Theory]
        [CustomAutoData]
        public void CanFindConflictById(string typeNameX, string typeNameY, Guid id, Type type, string editor)
        {
            var modelX = new DataType(new DataTypeModelTypeBuilder(typeNameX, id, type, editor).CreateType());
            var modelY = new DataType(new DataTypeModelTypeBuilder(typeNameY, id, type, editor).CreateType());

            var dataTypeValidator = new DataTypeValidator();

            Assert.Throws<InvalidOperationException>(() => dataTypeValidator.Validate(new[] { modelX, modelY }));
        }

        [Theory]
        [CustomAutoData]
        public void CanFindConflictByName(string typeName, Type type, string editor)
        {
            var modelX = new DataType(new DataTypeModelTypeBuilder(typeName, type, editor).CreateType());
            var modelY = new DataType(new DataTypeModelTypeBuilder(typeName, type, editor).CreateType());

            var dataTypeValidator = new DataTypeValidator();

            Assert.Throws<InvalidOperationException>(() => dataTypeValidator.Validate(new[] { modelX, modelY }));
        }
    }
}
