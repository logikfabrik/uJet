// <copyright file="DataTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture.Xunit2;
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DataTypeTest
    {
        [Theory]
        [CustomAutoData]
        public void CanGetTypeFromAttribute(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var model = new DataType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [AutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id, type, editor).CreateType();

            var model = new DataType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetNameFromAttribute(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var model = new DataType(modelType);

            model.Name.ShouldBe(builder.TypeName);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetEditorFromAttribute(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.CreateType();

            var model = new DataType(modelType);

            model.Editor.ShouldBe(builder.Editor);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetPreValuesFromAttribute(DataTypeModelTypeBuilder builder)
        {
            var typeBuilder = builder.GetTypeBuilder();

            typeBuilder.AddPublicReadOnlyProperty("PreValues", typeof(IDictionary<string, string>));

            var modelType = typeBuilder.CreateType();

            // TODO: Add default value for property.
            throw new NotImplementedException();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetPreValuesFromAttribute(DataType model)
        {
            model.PreValues.Any().ShouldBeFalse();
        }
    }
}