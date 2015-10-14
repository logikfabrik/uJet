// <copyright file="MediaTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="MediaTypeSynchronizationService" /> class. Synchronizes types annotated using the <see cref="MediaTypeAttribute" />.
    /// </summary>
    public class MediaTypeSynchronizationService : ContentTypeSynchronizationService
    {
        /// <summary>
        /// The type service.
        /// </summary>
        private readonly ITypeService _typeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizationService" /> class.
        /// </summary>
        public MediaTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database)),
                TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeService" /> is <c>null</c>.</exception>
        public MediaTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository,
            ITypeService typeService)
            : base(contentTypeService, contentTypeRepository)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            _typeService = typeService;
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public override void Synchronize()
        {
            var mediaTypes = _typeService.MediaTypes.Select(t => new MediaType(t)).ToArray();

            // No media types; there's nothing to sync.
            if (!mediaTypes.Any())
            {
                return;
            }

            ValidateMediaTypeId(mediaTypes);
            ValidateMediaTypeAlias(mediaTypes);

            // WARNING: This might cause issues; the array of types only contains the initial types, not including ones added/updated during sync.
            var types = ContentTypeService.GetAllMediaTypes().ToArray();

            foreach (var documentType in mediaTypes.Where(dt => dt.Id.HasValue))
            {
                SynchronizeById(types, documentType);
            }

            foreach (var documentType in mediaTypes.Where(dt => !dt.Id.HasValue))
            {
                SynchronizeByAlias(types, documentType);
            }

            SetAllowedContentTypes(types.Cast<IContentTypeBase>().ToArray(), mediaTypes);
        }

        /// <summary>
        /// Synchronizes media type by alias.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="mediaType">The media type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, or <paramref name="mediaType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the media type identifier is not <c>null</c>.</exception>
        internal virtual void SynchronizeByAlias(IEnumerable<IMediaType> contentTypes, MediaType mediaType)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            if (mediaType.Id.HasValue)
            {
                throw new ArgumentException("Media type ID must be null.", nameof(mediaType));
            }

            var ct = contentTypes.FirstOrDefault(type => type.Alias == mediaType.Alias);

            if (ct == null)
            {
                CreateMediaType(mediaType);
            }
            else
            {
                UpdateMediaType(ct, mediaType);
            }
        }

        /// <summary>
        /// Synchronizes media type by identifier.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="mediaType">The media type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, or <paramref name="mediaType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the media type identifier is <c>null</c>.</exception>
        internal virtual void SynchronizeById(IEnumerable<IMediaType> contentTypes, MediaType mediaType)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            if (!mediaType.Id.HasValue)
            {
                throw new ArgumentException("Media type ID cannot be null.", nameof(mediaType));
            }

            IMediaType ct = null;

            var id = ContentTypeRepository.GetContentTypeId(mediaType.Id.Value);

            if (id.HasValue)
            {
                // The media type has been synchronized before. Get the matching content type.
                // It might have been removed using the back office.
                ct = contentTypes.FirstOrDefault(type => type.Id == id.Value);
            }

            if (ct == null)
            {
                CreateMediaType(mediaType);

                // Get the created media type.
                ct = ContentTypeService.GetMediaType(mediaType.Alias);

                // Connect the media type and the created content type.
                ContentTypeRepository.SetContentTypeId(mediaType.Id.Value, ct.Id);
            }
            else
            {
                UpdateMediaType(ct, mediaType);
            }
        }

        /// <summary>
        /// Updates the media type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="mediaType">The media type to update.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="mediaType" /> are <c>null</c>.</exception>
        internal virtual void UpdateMediaType(IMediaType contentType, MediaType mediaType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            UpdateContentType(contentType, () => new global::Umbraco.Core.Models.MediaType(-1), mediaType);

            ContentTypeService.Save(contentType);

            // Update tracking.
            SetPropertyTypeId(ContentTypeService.GetMediaType(contentType.Alias), mediaType);
        }

        /// <summary>
        /// Validates the media type identifier.
        /// </summary>
        /// <param name="mediaTypes">The media types.</param>
        /// <exception cref="ArgumentNullException">>Thrown if <paramref name="mediaTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an identifier in <paramref name="mediaTypes" /> is conflicting.</exception>
        private static void ValidateMediaTypeId(IEnumerable<MediaType> mediaTypes)
        {
            if (mediaTypes == null)
            {
                throw new ArgumentNullException(nameof(mediaTypes));
            }

            var set = new HashSet<Guid>();

            foreach (var mediaType in mediaTypes)
            {
                if (!mediaType.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(mediaType.Id.Value))
                {
                    throw new InvalidOperationException(
                        $"ID conflict for media type {mediaType.Name}. ID {mediaType.Id.Value} is already in use.");
                }

                set.Add(mediaType.Id.Value);
            }
        }

        /// <summary>
        /// Validates the media type alias.
        /// </summary>
        /// <param name="mediaTypes">The media types.</param>
        /// <exception cref="ArgumentNullException">>Thrown if <paramref name="mediaTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an alias in <paramref name="mediaTypes" /> is conflicting.</exception>
        private static void ValidateMediaTypeAlias(IEnumerable<MediaType> mediaTypes)
        {
            if (mediaTypes == null)
            {
                throw new ArgumentNullException(nameof(mediaTypes));
            }

            var set = new HashSet<string>();

            foreach (var mediaType in mediaTypes)
            {
                if (set.Contains(mediaType.Alias))
                {
                    throw new InvalidOperationException(
                        string.Format("Alias conflict for media type {0}. Alias {0} is already in use.", mediaType.Alias));
                }

                set.Add(mediaType.Alias);
            }
        }

        /// <summary>
        /// Creates the media type.
        /// </summary>
        /// <param name="mediaType">The media type to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mediaType" /> is <c>null</c>.</exception>
        private void CreateMediaType(MediaType mediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            var t = (IMediaType)CreateContentType(() => new global::Umbraco.Core.Models.MediaType(-1), mediaType);

            ContentTypeService.Save(t);

            // Update tracking.
            SetPropertyTypeId(ContentTypeService.GetMediaType(t.Alias), mediaType);
        }
    }
}