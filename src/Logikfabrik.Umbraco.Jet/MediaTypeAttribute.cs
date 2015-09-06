// <copyright file="MediaTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]
    public class MediaTypeAttribute : ContentTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name to use for the new media type attribute.</param>
        public MediaTypeAttribute(string name) : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The ID to use for the new media type attribute.</param>
        /// <param name="name">The name to use for the new media type attribute.</param>
        public MediaTypeAttribute(string id, string name) : base(id, name)
        {
        }
    }
}