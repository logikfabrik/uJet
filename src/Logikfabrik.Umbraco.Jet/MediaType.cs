// <copyright file="MediaType.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using Extensions;

    public class MediaType : ContentType<MediaTypeAttribute>
    {
        public MediaType(Type type) : base(type)
        {
            if (!type.IsMediaType())
            {
                throw new ArgumentException($"Type {type} is not a media type.", nameof(type));
            }
        }
    }
}