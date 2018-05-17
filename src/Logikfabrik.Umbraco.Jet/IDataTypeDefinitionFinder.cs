// <copyright file="IDataTypeDefinitionFinder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IDataTypeDefinitionFinder" /> interface.
    /// </summary>
    public interface IDataTypeDefinitionFinder
    {
        /// <summary>
        /// Finds the data type definitions matching the specified model.
        /// </summary>
        /// <param name="modelNeedle">The model to find the data type definitions for.</param>
        /// <param name="dataTypeDefinitionsHaystack">The haystack of data type definitions.</param>
        /// <returns>The data type definitions found.</returns>
        /// <remarks>A data type definition is a match if a model needle's ID can be mapped to the ID of the data type definition, or if a model needle and the data type definition has the same name.</remarks>
        IDataTypeDefinition[] Find(DataType modelNeedle, IDataTypeDefinition[] dataTypeDefinitionsHaystack);
    }
}