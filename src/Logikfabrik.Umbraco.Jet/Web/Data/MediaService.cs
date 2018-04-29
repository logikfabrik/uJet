// <copyright file="MediaService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using EnsureThat;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="MediaService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to media models.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class MediaService : ContentService
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaService" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public MediaService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        public MediaService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            EnsureArg.IsNotNull(umbracoHelperWrapper);

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <summary>
        /// Gets a model for the media with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The media model type.</typeparam>
        /// <param name="id">The media identifier.</param>
        /// <returns>A model for the media with the specified identifier.</returns>
        public T GetMedia<T>(int id)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<MediaTypeAttribute>(), nameof(T));

            return GetMedia<T>(_umbracoHelperWrapper.TypedMedia(id));
        }

        /// <summary>
        /// Gets a model for the media.
        /// </summary>
        /// <typeparam name="T">The media model type.</typeparam>
        /// <param name="content">The media content.</param>
        /// <returns>A model for the media.</returns>
        public T GetMedia<T>(IPublishedContent content)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<MediaTypeAttribute>(), nameof(T));
            EnsureArg.IsNotNull(content);

            return (T)GetMedia(content, typeof(T));
        }

        /// <summary>
        /// Gets a model for the media.
        /// </summary>
        /// <param name="content">The media content.</param>
        /// <param name="mediaModelType">The media model type.</param>
        /// <returns>A model for the media.</returns>
        public object GetMedia(IPublishedContent content, Type mediaModelType)
        {
            EnsureArg.IsNotNull(content);
            EnsureArg.IsNotNull(mediaModelType);
            EnsureArg.IsTrue(mediaModelType.IsModelType<MediaTypeAttribute>(), nameof(mediaModelType));

            return GetMappedContent(content, mediaModelType);
        }
    }
}