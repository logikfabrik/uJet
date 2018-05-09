// <copyright file="DataTypeDefinitionMappingsTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using Jet.Mappings;
    using Moq.AutoMock;
    using Shouldly;
    using Xunit;

    public class DataTypeDefinitionMappingsTest : IDisposable
    {
        [Fact]
        public void CanAddDataTypeDefinitionMapping()
        {
            var type = typeof(object);

            var mocker = new AutoMocker();

            var mappingMock = mocker.GetMock<IDataTypeDefinitionMapping>();

            mappingMock.Setup(m => m.CanMapToDefinition(type)).Returns(true);

            DataTypeDefinitionMappings.Mappings.Add(type, mappingMock.Object);

            DataTypeDefinitionMappings.Mappings.TryGetValue(type, out _).ShouldBeTrue();
        }

        [Fact]
        public void CanRemoveDataTypeDefinitionMapping()
        {
            var type = typeof(object);

            var mocker = new AutoMocker();

            var mappingMock = mocker.GetMock<IDataTypeDefinitionMapping>();

            mappingMock.Setup(m => m.CanMapToDefinition(type)).Returns(true);

            DataTypeDefinitionMappings.Mappings.Add(type, mappingMock.Object);
            DataTypeDefinitionMappings.Mappings.Remove(type);

            DataTypeDefinitionMappings.Mappings.TryGetValue(type, out var _).ShouldBeFalse();
        }

        public void Dispose()
        {
            DataTypeDefinitionMappings.Mappings.Clear();
        }
    }
}
