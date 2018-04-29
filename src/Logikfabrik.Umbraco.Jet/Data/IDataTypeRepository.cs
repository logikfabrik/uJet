// <copyright file="IDataTypeRepository.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;

    /// <summary>
    /// The <see cref="IDataTypeRepository" /> interface.
    /// </summary>
    public interface IDataTypeRepository
    {
        /// <summary>
        /// Gets the definition type model identifier.
        /// </summary>
        /// <param name="definitionId">The definition identifier.</param>
        /// <returns>The definition type model identifier.</returns>
        Guid? GetDefinitionTypeModelId(int definitionId);

        /// <summary>
        /// Gets the definition identifier.
        /// </summary>
        /// <param name="id">The definition type model identifier.</param>
        /// <returns>The definition identifier.</returns>
        int? GetDefinitionId(Guid id);

        /// <summary>
        /// Sets the definition identifier.
        /// </summary>
        /// <param name="id">The definition type model identifier.</param>
        /// <param name="definitionId">The definition identifier.</param>
        void SetDefinitionId(Guid id, int definitionId);
    }
}
