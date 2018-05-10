// <copyright file="MediaType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;

    /// <summary>
    /// The <see cref="MediaType" /> class. Model for media types.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class MediaType : ComposableContentTypeModel<MediaTypeAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaType" /> class.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public MediaType(Type modelType)
            : base(modelType)
        {
        }
    }
}