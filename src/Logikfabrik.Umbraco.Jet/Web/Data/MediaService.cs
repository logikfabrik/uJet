// <copyright file="MediaService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="MediaService" /> class.
    /// </summary>
    public class MediaService : ContentService
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaService" /> class.
        /// </summary>
        public MediaService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="umbracoHelperWrapper" /> is <c>null</c>.</exception>
        public MediaService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            if (umbracoHelperWrapper == null)
            {
                throw new ArgumentNullException(nameof(umbracoHelperWrapper));
            }

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <summary>
        /// Gets the media with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The media type.</typeparam>
        /// <param name="id">The media identifier.</param>
        /// <returns>The media with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a media type.</exception>
        public T GetMedia<T>(int id)
            where T : class, new()
        {
            if (!typeof(T).IsMediaType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a media type.");
            }

            return GetMedia<T>(_umbracoHelperWrapper.TypedMedia(id));
        }

        /// <summary>
        /// Gets the media.
        /// </summary>
        /// <typeparam name="T">The media type.</typeparam>
        /// <param name="content">The media content.</param>
        /// <returns>The media.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a media type.</exception>
        public T GetMedia<T>(IPublishedContent content)
            where T : class, new()
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!typeof(T).IsMediaType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a media type.");
            }

            return (T)GetMedia(content, typeof(T));
        }

        /// <summary>
        /// Gets the media.
        /// </summary>
        /// <param name="content">The media content.</param>
        /// <param name="mediaType">The media type.</param>
        /// <returns>The media.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="mediaType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="mediaType" /> is not a media type.</exception>
        public object GetMedia(IPublishedContent content, Type mediaType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            if (!mediaType.IsMediaType())
            {
                throw new ArgumentException($"Type {mediaType} is not a media type.", nameof(mediaType));
            }

            return GetMappedContent(content, mediaType);
        }
    }
}
