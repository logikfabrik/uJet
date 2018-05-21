// <copyright file="PropertyValueConverterRegistrar.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="PropertyValueConverterRegistrar" /> class. Utility class for registering property value converters.
    /// </summary>
    [PublicAPI]
    public static class PropertyValueConverterRegistrar
    {
        /// <summary>
        /// Registers the specified property value converter.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type" /> to register the specified property value converter for.</typeparam>
        /// <param name="propertyValueConverter">The property value converter to register.</param>
        public static void Register<T>(IPropertyValueConverter propertyValueConverter)
        {
            Ensure.That(propertyValueConverter).IsNotNull();

            var type = typeof(T);
            var registry = PropertyValueConverters.Converters;

            if (registry.TryGetValue(type, out var converters))
            {
                var c = converters.ToArray();

                if (c.Any(converter => converter.GetType() == type))
                {
                    return;
                }

                registry.Remove(type);
                registry.Add(type, new List<IPropertyValueConverter>(c) { propertyValueConverter });
            }
            else
            {
                registry.Add(type, new[] { propertyValueConverter });
            }
        }
    }
}
