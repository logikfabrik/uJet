// <copyright file="BuiltInDataTypeDefinitionMappingsRegistrar.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="BuiltInDataTypeDefinitionMappingsRegistrar" /> class. Class for registering all built-in data type definition mappings.
    /// </summary>
    public static class BuiltInDataTypeDefinitionMappingsRegistrar
    {
        /// <summary>
        /// Registers all built-in data type definition mappings.
        /// </summary>
        public static void RegisterAll()
        {
            DataTypeDefinitionMappingRegistrar.Register<bool>(new BooleanDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<bool?>(new BooleanDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<DateTime>(new DateTimeDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<DateTime?>(new DateTimeDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<float>(new FloatingBinaryPointDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<float?>(new FloatingBinaryPointDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<double>(new FloatingBinaryPointDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<double?>(new FloatingBinaryPointDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<decimal>(new FloatingDecimalPointDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<decimal?>(new FloatingDecimalPointDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<short>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<short?>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<int>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<int?>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<ushort>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<ushort?>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<uint>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<uint?>(new IntegerDataTypeDefinitionMapping());
            DataTypeDefinitionMappingRegistrar.Register<string>(new StringDataTypeDefinitionMapping());
        }
    }
}
