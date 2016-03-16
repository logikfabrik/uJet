// <copyright file="JetAssemblyElement.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The <see cref="JetAssemblyElement" /> class.
    /// </summary>
    public class JetAssemblyElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the assembly full name.
        /// </summary>
        /// <value>
        /// The assembly full name.
        /// </value>
        [ConfigurationProperty(
            "name",
            IsRequired = true,
            IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}
