// <copyright file="IDataTypeDefinitionService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    using System;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IDataTypeDefinitionService" /> interface.
    /// </summary>
    public interface IDataTypeDefinitionService
    {
        /// <summary>
        /// Gets a definition for the specified UI from type.
        /// </summary>
        /// <param name="fromType">The from type.</param>
        /// <returns>A definition; or <c>null</c>.</returns>
        IDataTypeDefinition GetDefinition(Type fromType);

        /// <summary>
        /// Gets a definition for the specified UI hint and from type.
        /// </summary>
        /// <param name="uiHint">The UI hint.</param>
        /// <param name="fromType">The from type.</param>
        /// <returns>A definition; or <c>null</c>.</returns>
        IDataTypeDefinition GetDefinition(string uiHint, Type fromType);
    }
}