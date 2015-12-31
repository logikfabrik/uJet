// <copyright file="MediaService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="MediaService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to media models.
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
        /// Gets a model for the media with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The media model type.</typeparam>
        /// <param name="id">The media identifier.</param>
        /// <returns>A model for the media with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a media model type.</exception>
        public T GetMedia<T>(int id)
            where T : class, new()
        {
            if (!typeof(T).IsMediaType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a media model type.");
            }

            return GetMedia<T>(_umbracoHelperWrapper.TypedMedia(id));
        }

        /// <summary>
        /// Gets a model for the media.
        /// </summary>
        /// <typeparam name="T">The media model type.</typeparam>
        /// <param name="content">The media content.</param>
        /// <returns>A model for the media.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a media model type.</exception>
        public T GetMedia<T>(IPublishedContent content)
            where T : class, new()
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!typeof(T).IsMediaType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a media model type.");
            }

            return (T)GetMedia(content, typeof(T));
        }

        /// <summary>
        /// Gets a model for the media.
        /// </summary>
        /// <param name="content">The media content.</param>
        /// <param name="mediaModelType">The media model type.</param>
        /// <returns>A model for the media.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="mediaModelType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="mediaModelType" /> is not a media model type.</exception>
        public object GetMedia(IPublishedContent content, Type mediaModelType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (mediaModelType == null)
            {
                throw new ArgumentNullException(nameof(mediaModelType));
            }

            if (!mediaModelType.IsMediaType())
            {
                throw new ArgumentException($"Type {mediaModelType} is not a media model type.", nameof(mediaModelType));
            }

            return GetMappedContent(content, mediaModelType);
        }
    }
}