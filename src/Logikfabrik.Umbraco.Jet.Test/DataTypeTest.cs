﻿// <copyright file="DataTypeTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            var modelType = builder.Create(Scope.Public);

            var model = new DataType(modelType);

            model.ModelType.ShouldBe(modelType);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetIdFromAttribute(string typeName, Guid id, Type type, string editor)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id, type, editor).Create(Scope.Public);

            var model = new DataType(modelType);

            model.Id.ShouldBe(id);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetNameFromAttribute(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var model = new DataType(modelType);

            model.Name.ShouldBe(builder.TypeName);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetEditorFromAttribute(DataTypeModelTypeBuilder builder)
        {
            var modelType = builder.Create(Scope.Public);

            var model = new DataType(modelType);

            model.Editor.ShouldBe(builder.Editor);
        }

        [Theory]
        [CustomAutoData]
        public void CanGetPreValuesFromAttribute(DataTypeModelTypeBuilder builder)
        {
            builder.AddProperty(Scope.Public, Accessors.Get, "PreValues", typeof(Dictionary<string, string>));

            var modelType = builder.Create(Scope.Public);

            var model = new DataType(modelType);

            model.PreValues.ShouldNotBeNull();
        }

        [Theory]
        [CustomAutoData]
        public void CanNotGetPreValuesFromAttribute(DataType model)
        {
            model.PreValues.Any().ShouldBeFalse();
        }
    }
}