// <copyright file="DataTypeDefinitionMappingRegistrar.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using EnsureThat;

    /// <summary>
    /// The <see cref="DataTypeDefinitionMappingRegistrar" /> class. Class for registering data type definition mappings.
    /// </summary>
    public static class DataTypeDefinitionMappingRegistrar
    {
        /// <summary>
        /// Registers the specified data type definition mapping.
        /// </summary>
        /// <typeparam name="T">The type to register the specified data type definition mapping for.</typeparam>
        /// <param name="dataTypeDefinitionMapping">The data type definition mapping.</param>
        public static void Register<T>(IDataTypeDefinitionMapping dataTypeDefinitionMapping)
        {
            var type = typeof(T);

            Register(type, dataTypeDefinitionMapping);
        }

        /// <summary>
        /// Registers the specified data type definition mapping.
        /// </summary>
        /// <param name="type">The type to register the specified data type definition mapping for.</param>
        /// <param name="dataTypeDefinitionMapping">The data type definition mapping.</param>
        public static void Register(Type type, IDataTypeDefinitionMapping dataTypeDefinitionMapping)
        {
            Ensure.That(type).IsNotNull();
            Ensure.That(dataTypeDefinitionMapping).IsNotNull();

            if (DataTypeDefinitionMappings.Mappings.ContainsKey(type))
            {
                if (DataTypeDefinitionMappings.Mappings[type].GetType() == dataTypeDefinitionMapping.GetType())
                {
                    // There is already a mapping registered for the specified type and data type definition mapping.
                    return;
                }

                // Remove the existing mapping. It will be replaced using the specified data type definition mapping.
                DataTypeDefinitionMappings.Mappings.Remove(type);
            }

            DataTypeDefinitionMappings.Mappings.Add(type, dataTypeDefinitionMapping);
        }
    }
}