// <copyright file="BuiltInDataTypeDefinitionMappingsRegistrarTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using AutoFixture.Xunit2;
    using Jet.Mappings;
    using Shouldly;
    using Xunit;

    public class BuiltInDataTypeDefinitionMappingsRegistrarTest : IDisposable
    {
        [Theory]
        [InlineAutoData(typeof(bool))]
        [InlineAutoData(typeof(bool?))]
        [InlineAutoData(typeof(DateTime))]
        [InlineAutoData(typeof(DateTime?))]
        [InlineAutoData(typeof(float))]
        [InlineAutoData(typeof(float?))]
        [InlineAutoData(typeof(double))]
        [InlineAutoData(typeof(double?))]
        [InlineAutoData(typeof(decimal))]
        [InlineAutoData(typeof(decimal?))]
        [InlineAutoData(typeof(short))]
        [InlineAutoData(typeof(short?))]
        [InlineAutoData(typeof(int))]
        [InlineAutoData(typeof(int?))]
        [InlineAutoData(typeof(ushort))]
        [InlineAutoData(typeof(ushort?))]
        [InlineAutoData(typeof(uint))]
        [InlineAutoData(typeof(uint?))]
        [InlineAutoData(typeof(string))]
        public void CanRegisterAllAndGetDataTypeDefinition(Type type)
        {
            BuiltInDataTypeDefinitionMappingsRegistrar.RegisterAll();

            DataTypeDefinitionMappings.Mappings.TryGetValue(type, out var _).ShouldBeTrue();
        }

        public void Dispose()
        {
            DataTypeDefinitionMappings.Mappings.Clear();
        }
    }
}