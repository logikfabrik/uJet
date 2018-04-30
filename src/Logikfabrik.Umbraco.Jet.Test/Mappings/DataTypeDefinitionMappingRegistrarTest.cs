// <copyright file="DataTypeDefinitionMappingRegistrarTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using Jet.Mappings;
    using Moq.AutoMock;
    using Shouldly;
    using Xunit;

    public class DataTypeDefinitionMappingRegistrarTest : IDisposable
    {
        [Fact]
        public void CanRegisterAndGetDataTypeDefinition()
        {
            var type = typeof(object);

            var mocker = new AutoMocker();

            var mappingMock = mocker.GetMock<IDataTypeDefinitionMapping>();

            mappingMock.Setup(m => m.CanMapToDefinition(type)).Returns(true);

            DataTypeDefinitionMappingRegistrar.Register(type, mappingMock.Object);

            DataTypeDefinitionMappings.Mappings.TryGetValue(type, out _).ShouldBeTrue();
        }

        public void Dispose()
        {
            DataTypeDefinitionMappings.Mappings.Clear();
        }
    }
}
