// <copyright file="MediaTypeAttribute.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="MediaTypeAttribute" /> class. Attribute for model type annotation.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class,
        Inherited = false)]

    // ReSharper disable once InheritdocConsiderUsage
    public class MediaTypeAttribute : ComposableContentTypeModelTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        // ReSharper disable once UnusedMember.Global
        public MediaTypeAttribute(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        // ReSharper disable once InheritdocConsiderUsage
        // ReSharper disable once UnusedMember.Global
        public MediaTypeAttribute(string id, string name)
            : base(id, name)
        {
        }
    }
}