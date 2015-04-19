//----------------------------------------------------------------------------------
// <copyright file="DataTypeDefinitionMappings.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core.Models;

    public static class DataTypeDefinitionMappings
    {
        private static readonly DataTypeDefinitionMappingDictionary SharedMappings = GetDefaultMappings();

        public static DataTypeDefinitionMappingDictionary Mappings
        {
            get { return SharedMappings; }
        }
        
        /// <summary>
        /// Gets a matching definition mapping.
        /// </summary>
        /// <param name="fromType">The from type to match.</param>
        /// <returns>A definition mapping; or null if there's no match.</returns>
        internal static IDataTypeDefinitionMapping GetDefinitionMapping(Type fromType)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException("fromType");
            }

            IDataTypeDefinitionMapping mapping;

            if (!SharedMappings.TryGetValue(fromType, out mapping))
            {
                return null;
            }

            return mapping.CanMapToDefinition(fromType) ? mapping : null;
        }

        /// <summary>
        /// Gets a matching definition.
        /// </summary>
        /// <param name="uiHint">The UI hint to match.</param>
        /// <param name="fromType">The from type to match.</param>
        /// <returns>A definition; or null if there's no match.</returns>
        internal static IDataTypeDefinition GetDefinition(string uiHint, Type fromType)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException("fromType");
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

            return mapping != null ? mapping.GetMappedDefinition(fromType) : null;
        }

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
