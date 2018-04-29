// <copyright file="IAssemblyLoader.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The <see cref="IAssemblyLoader" /> interface.
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <returns>The assemblies.</returns>
        IEnumerable<Assembly> GetAssemblies();
    }
}