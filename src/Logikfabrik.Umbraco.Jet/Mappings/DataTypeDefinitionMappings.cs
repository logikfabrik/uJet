// <copyright file="DataTypeDefinitionMappings.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="DataTypeDefinitionMappings" /> class.
    /// </summary>
    public static class DataTypeDefinitionMappings
    {
        /// <summary>
        /// The shared mappings.
        /// </summary>
        private static readonly DataTypeDefinitionMappingDictionary SharedMappings = GetDefaultMappings();

        /// <summary>
        /// Gets the mappings.
        /// </summary>
        /// <value>
        /// The mappings.
        /// </value>
        public static DataTypeDefinitionMappingDictionary Mappings => SharedMappings;

        /// <summary>
        /// Gets the definition mapping.
        /// </summary>
        /// <param name="fromType">From type.</param>
        /// <returns>The definition mapping; or <c>null</c> if there's no match.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromType" /> is <c>null</c>.</exception>
        internal static IDataTypeDefinitionMapping GetDefinitionMapping(Type fromType)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException(nameof(fromType));
            }

            IDataTypeDefinitionMapping mapping;

            if (!SharedMappings.TryGetValue(fromType, out mapping))
            {
                return null;
            }

            return mapping.CanMapToDefinition(fromType) ? mapping : null;
        }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <param name="uiHint">The UI hint.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>The definition; or <c>null</c> if there's no match.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fromType" /> is <c>null</c>.</exception>
        internal static IDataTypeDefinition GetDefinition(string uiHint, Type fromType)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException(nameof(fromType));
            }

            // Query the default data type definition mapping.
            if (!string.IsNullOrWhiteSpace(uiHint))
            {
                if (SharedMappings.DefaultMapping.CanMapToDefinition(uiHint, fromType))
                {
                    return SharedMappings.DefaultMapping.GetMappedDefinition(uiHint, fromType);
                }
            }

            var mapping = GetDefinitionMapping(fromType);

            return mapping?.GetMappedDefinition(fromType);
        }

        /// <summary>
        /// Gets the default mappings.
        /// </summary>
        /// <returns>The default mappings.</returns>
        private static DataTypeDefinitionMappingDictionary GetDefaultMappings()
        {
            return new DataTypeDefinitionMappingDictionary
                {
                    { typeof(bool), new BooleanDataTypeDefinitionMapping() },
                    { typeof(bool?), new BooleanDataTypeDefinitionMapping() },
                    { typeof(DateTime), new DateTimeDataTypeDefinitionMapping() },
                    { typeof(DateTime?), new DateTimeDataTypeDefinitionMapping() },
                    { typeof(float), new FloatingBinaryPointDataTypeDefinitionMapping() },
                    { typeof(float?), new FloatingBinaryPointDataTypeDefinitionMapping() },
                    { typeof(double), new FloatingBinaryPointDataTypeDefinitionMapping() },
                    { typeof(double?), new FloatingBinaryPointDataTypeDefinitionMapping() },
                    { typeof(decimal), new FloatingDecimalPointDataTypeDefinitionMapping() },
                    { typeof(decimal?), new FloatingDecimalPointDataTypeDefinitionMapping() },
                    { typeof(short), new IntegerDataTypeDefinitionMapping() },
                    { typeof(short?), new IntegerDataTypeDefinitionMapping() },
                    { typeof(int), new IntegerDataTypeDefinitionMapping() },
                    { typeof(int?), new IntegerDataTypeDefinitionMapping() },
                    { typeof(ushort), new IntegerDataTypeDefinitionMapping() },
                    { typeof(ushort?), new IntegerDataTypeDefinitionMapping() },
                    { typeof(uint), new IntegerDataTypeDefinitionMapping() },
                    { typeof(uint?), new IntegerDataTypeDefinitionMapping() },
                    { typeof(string), new StringDataTypeDefinitionMapping() }
                };
        }
    }
}
