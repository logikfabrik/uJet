// <copyright file="DataTypeDefinitionFinderTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test
{
    using System;
    using System.Linq;
    using global::Umbraco.Core.Models;
    using Logikfabrik.Umbraco.Jet.Data;
    using Moq.AutoMock;
    using Shouldly;
    using SpecimenBuilders;
    using Utilities;
    using Xunit;

    public class DataTypeDefinitionFinderTest
    {
        [Theory]
        [CustomAutoData]
        public void CanFindById(string typeName, Guid id, Type type, string editor, IDataTypeDefinition[] definitions)
        {
            var modelType = new DataTypeModelTypeBuilder(typeName, id, type, editor).CreateType();

            var model = new Jet.DataType(modelType);

            var mocker = new AutoMocker();

            var typeRepositoryMock = mocker.GetMock<ITypeRepository>();

            typeRepositoryMock.Setup(m => m.GetDefinitionId(id)).Returns(definitions.First().Id);

            var dataTypeDefinitionFinder = mocker.CreateInstance<DataTypeDefinitionFinder>();

            dataTypeDefinitionFinder.Find(model, definitions).ShouldHaveSingleItem();
        }

        [Theory]
        [CustomAutoData]
        public void CanFindByName(Type type, string editor, IDataTypeDefinition[] definitions)
        {
            var modelType = new DataTypeModelTypeBuilder(definitions.First().Name, type, editor).CreateType();

            var model = new Jet.DataType(modelType);

            var mocker = new AutoMocker();

            var dataTypeDefinitionFinder = mocker.CreateInstance<DataTypeDefinitionFinder>();

            dataTypeDefinitionFinder.Find(model, definitions).ShouldHaveSingleItem();
        }
    }
}