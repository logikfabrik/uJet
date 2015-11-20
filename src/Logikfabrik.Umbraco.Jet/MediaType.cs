// <copyright file="MediaType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
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
        /// <exception cref="ArgumentException">Thrown if <paramref name="type" /> is not a media type.</exception>
        public MediaType(Type type)
            : base(type)
        {
            if (!type.IsMediaType())
            {
                throw new ArgumentException($"Type {type} is not a media type.", nameof(type));
            }
        }
    }
}