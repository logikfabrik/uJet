// <copyright file="JetAssemblyElementCollection.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The <see cref="JetAssemblyElementCollection" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class JetAssemblyElementCollection : ConfigurationElementCollection
    {
        /// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
        {
            return new JetAssemblyElement();
        }

        /// <inheritdoc />
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JetAssemblyElement)element).Name;
        }
    }
}
