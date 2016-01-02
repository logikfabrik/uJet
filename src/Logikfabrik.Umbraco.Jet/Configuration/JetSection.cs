// <copyright file="JetSection.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The <see cref="JetSection" /> class.
    /// </summary>
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
        public SynchronizationMode Synchronize
        {
            get { return (SynchronizationMode)this["synchronize"]; }
            set { this["synchronize"] = value; }
        }

        /// <summary>
        /// Gets or sets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        [ConfigurationProperty(
            "assemblies",
            IsRequired = false,
            IsDefaultCollection = true)]
        public JetAssemblyElementCollection Assemblies
        {
            get { return (JetAssemblyElementCollection)this["assemblies"]; }
            set { this["assemblies"] = value; }
        }
    }
}
