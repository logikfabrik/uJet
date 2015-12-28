// <copyright file="MediaType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    /// <summary>
    /// The <see cref="MediaType" /> class.
    /// </summary>
    public class MediaType : ContentType<MediaTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaType" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public MediaType(Type type)
            : base(type)
        {
            if (!type.IsMediaType())
            {
                throw new ArgumentException($"Type {type} is not a media type.", nameof(type));
            }
        }

        /// <summary>
        /// Gets the composition.
        /// </summary>
        /// <returns>
        /// The composition.
        /// </returns>
        protected override IDictionary<Type, IEnumerable<Type>> GetComposition()
        {
            return GetComposition(TypeExtensions.IsMediaType);
        }
    }
}