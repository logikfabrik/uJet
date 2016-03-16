// <copyright file="DataTypeDefinitionMappingRegistrar.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;

    /// <summary>
    /// The <see cref="DataTypeDefinitionMappingRegistrar" /> class. Utility class for registering data type definition mappings.
    /// </summary>
    public static class DataTypeDefinitionMappingRegistrar
    {
        /// <summary>
        /// Registers the specified data type definition mapping.
        /// </summary>
        /// <typeparam name="T">The type to register the specified data type definition mapping for.</typeparam>
        /// <param name="dataTypeDefinitionMapping">The data type definition mapping.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dataTypeDefinitionMapping" /> is <c>null</c>.</exception>
        public static void Register<T>(IDataTypeDefinitionMapping dataTypeDefinitionMapping)
        {
            if (dataTypeDefinitionMapping == null)
            {
                throw new ArgumentNullException(nameof(dataTypeDefinitionMapping));
            }

            var type = typeof(T);
            var registry = DataTypeDefinitionMappings.Mappings;

            if (registry.ContainsKey(type))
            {
                if (registry[type].GetType() == dataTypeDefinitionMapping.GetType())
                {
                    return;
                }

                registry.Remove(type);
            }

            registry.Add(type, dataTypeDefinitionMapping);
        }
    }
}