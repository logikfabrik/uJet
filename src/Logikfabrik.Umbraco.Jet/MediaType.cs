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
        /// <param name="composition">The type composition.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="type" /> is not a media type.</exception>
        public MediaType(Type type, IDictionary<Type, IEnumerable<Type>> composition)
            : base(type, composition)
        {
            if (!type.IsMediaType())
            {
                throw new ArgumentException($"Type {type} is not a media type.", nameof(type));
            }
        }
    }
}