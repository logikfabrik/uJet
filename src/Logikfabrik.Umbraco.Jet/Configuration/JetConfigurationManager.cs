﻿// <copyright file="JetConfigurationManager.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Configuration
{
    using System.Linq;

    /// <summary>
    /// The <see cref="JetConfigurationManager" /> class.
    /// </summary>
    public static class JetConfigurationManager
    {
        private static readonly JetSection Section = GetSection();

        /// <summary>
        /// Gets the synchronization mode.
        /// </summary>
        /// <value>
        /// The synchronization mode.
        /// </value>
        public static SynchronizationModes Synchronize => Section.Synchronize;

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        public static string[] Assemblies => GetAssemblies();

        private static JetSection GetSection()
        {
            return System.Configuration.ConfigurationManager.GetSection("logikfabrik.umbraco.jet") as JetSection ?? new JetSection();
        }

        private static string[] GetAssemblies()
        {
            return Section.Assemblies.Cast<JetAssemblyElement>().Select(element => element.Name).ToArray();
        }
    }
}
