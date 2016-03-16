// <copyright file="JetAssemblyElementCollection.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The <see cref="JetAssemblyElementCollection" /> class.
    /// </summary>
    public class JetAssemblyElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="JetAssemblyElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="JetAssemblyElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new JetAssemblyElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="JetAssemblyElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="string" /> that acts as the key for the specified <see cref="JetAssemblyElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JetAssemblyElement)element).Name;
        }
    }
}
