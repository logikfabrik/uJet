// <copyright file="JetSection.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The <see cref="JetSection" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class JetSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the synchronization mode.
        /// </summary>
        /// <value>
        /// The synchronization mode.
        /// </value>
        [ConfigurationProperty(
            "synchronize",
            DefaultValue = "DocumentTypes,MediaTypes,DataTypes,MemberTypes",
            IsRequired = false)]
        public SynchronizationModes Synchronize
        {
            get => (SynchronizationModes)this["synchronize"];
            set => this["synchronize"] = value;
        }

        /// <summary>
        /// Gets or sets the assemblies to scan.
        /// </summary>
        /// <value>
        /// The assemblies to scan.
        /// </value>
        [ConfigurationProperty(
            "assemblies",
            IsRequired = false,
            IsDefaultCollection = true)]
        public JetAssemblyElementCollection Assemblies
        {
            get => (JetAssemblyElementCollection)this["assemblies"];
            set => this["assemblies"] = value;
        }
    }
}
