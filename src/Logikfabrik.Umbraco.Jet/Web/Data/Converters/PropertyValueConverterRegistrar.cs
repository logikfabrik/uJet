// <copyright file="PropertyValueConverterRegistrar.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

using System.Linq;

namespace Logikfabrik.Umbraco.Jet.Web.Data.Converters
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="PropertyValueConverterRegistrar" /> class. Utility class for registering property value converters.
    /// </summary>
    public static class PropertyValueConverterRegistrar
    {
        /// <summary>
        /// Registers the specified property value converter.
        /// </summary>
        /// <typeparam name="T">The type to register the specified property value converter for.</typeparam>
        /// <param name="propertyValueConverter">The property value converter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyValueConverter" /> is <c>null</c>.</exception>
        public static void Register<T>(IPropertyValueConverter propertyValueConverter)
        {
            if (propertyValueConverter == null)
            {
                throw new ArgumentNullException(nameof(propertyValueConverter));
            }

            var type = typeof(T);
            var registry = PropertyValueConverters.Converters;

            IEnumerable<IPropertyValueConverter> converters;

            if (registry.TryGetValue(type, out converters))
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
