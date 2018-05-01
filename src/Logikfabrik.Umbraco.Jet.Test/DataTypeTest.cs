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
    using Utilities;
    using Xunit;

    public class DataTypeTest
    {
        [Theory]
        [AutoData]
        public void CanGetTypeFromAttribute(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new DataType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [AutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id.ToString(), type, editor).CreateType();

            var model = new DataType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [AutoData]
        public void CanGetNameFromAttribute(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new DataType(modelType);

            model.Name.ShouldBe(modelType.Name);
        }

        [Theory]
        [AutoData]
        public void CanGetEditorFromAttribute(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new DataType(modelType);

            model.Editor.ShouldBe(editor);
        }

        [Theory]
        [AutoData]
        public void CanGetPreValuesFromAttribute(string typeName, Type type, string editor)
        {
            var typeBuilder = new DataTypeModelTypeBuilder(typeName, type, editor).GetTypeBuilder();

            typeBuilder.AddPublicReadOnlyProperty("PreValues", typeof(IDictionary<string, string>));

            var modelType = typeBuilder.CreateType();

            // TODO: Add default value for property.
            throw new NotImplementedException();
        }

        [Theory]
        [AutoData]
        public void CanNotGetPreValuesFromAttribute(string typeName, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, type, editor).CreateType();

            var model = new DataType(modelType);

            model.PreValues.Any().ShouldBeFalse();
        }
    }
}