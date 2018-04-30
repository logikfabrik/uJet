// <copyright file="BuiltInDataTypeDefinitionMappingsRegistrarTest.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Mappings
{
    using System;
    using Jet.Mappings;
    using Shouldly;
    using Xunit;

    public class BuiltInDataTypeDefinitionMappingsRegistrarTest : IDisposable
    {
        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(DateTime?))]
        [InlineData(typeof(float))]
        [InlineData(typeof(float?))]
        [InlineData(typeof(double))]
        [InlineData(typeof(double?))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(decimal?))]
        [InlineData(typeof(short))]
        [InlineData(typeof(short?))]
        [InlineData(typeof(int))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(ushort?))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(uint?))]
        [InlineData(typeof(string))]
        public void CanRegisterAllAndGetDataTypeDefinition(Type type)
        {
            BuiltInDataTypeDefinitionMappingsRegistrar.RegisterAll();

            DataTypeDefinitionMappings.Mappings.TryGetValue(type, out _).ShouldBeTrue();
        }

        public void Dispose()
        {
            DataTypeDefinitionMappings.Mappings.Clear();
        }
    }
}