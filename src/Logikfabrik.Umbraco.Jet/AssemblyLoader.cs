// <copyright file="AssemblyLoader.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using EnsureThat;

    /// <summary>
    /// The <see cref="AssemblyLoader" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class AssemblyLoader : IAssemblyLoader
    {
        private readonly AppDomain _appDomain;
        private readonly IEnumerable<string> _assemblyNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoader" /> class.
        /// </summary>
        /// <param name="appDomain">The application domain.</param>
        /// <param name="assemblyNames">The assembly names.</param>
        public AssemblyLoader(AppDomain appDomain, IEnumerable<string> assemblyNames)
        {
            EnsureArg.IsNotNull(appDomain);
            EnsureArg.IsNotNull(assemblyNames);

            _appDomain = appDomain;
            _assemblyNames = assemblyNames;
        }

        /// <inheritdoc />
        public IEnumerable<Assembly> GetAssemblies()
        {
            return !_assemblyNames.Any()
                ? _appDomain.GetAssemblies()
                : _appDomain.GetAssemblies().Where(assembly => _assemblyNames.Contains(assembly.GetName().Name, StringComparer.InvariantCultureIgnoreCase)).ToArray();
        }
    }
}
