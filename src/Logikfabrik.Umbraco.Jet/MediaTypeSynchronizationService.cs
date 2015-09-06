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

    public class MediaTypeSynchronizationService : ContentTypeSynchronizationService
    {
        private readonly IContentTypeRepository contentTypeRepository;
        private readonly IContentTypeService contentTypeService;
        private readonly ITypeService typeService;

        public MediaTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database)),
                TypeService.Instance)
        {
        }

        public MediaTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository,
            ITypeService typeService)
            : base(contentTypeService, contentTypeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (contentTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(contentTypeRepository));
            }

            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            this.contentTypeService = contentTypeService;
            this.contentTypeRepository = contentTypeRepository;
            this.typeService = typeService;
        }

        /// <summary>
        /// Synchronizes media types.
        /// </summary>
        public override void Synchronize()
        {
            var mediaTypes = typeService.MediaTypes.Select(t => new MediaType(t)).ToArray();

            ValidateMediaTypeId(mediaTypes);
            ValidateMediaTypeAlias(mediaTypes);

            foreach (var documentType in mediaTypes.Where(dt => dt.Id.HasValue))
            {
                SynchronizeById(contentTypeService.GetAllMediaTypes(), documentType);
            }

            foreach (var documentType in mediaTypes.Where(dt => !dt.Id.HasValue))
            {
                SynchronizeByName(contentTypeService.GetAllMediaTypes(), documentType);
            }

            SetAllowedContentTypes(
                contentTypeService.GetAllMediaTypes().Cast<IContentTypeBase>().ToArray(),
                mediaTypes);
        }

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

        private void SynchronizeByName(IEnumerable<IMediaType> contentTypes, MediaType mediaType)
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

        private void SynchronizeById(IEnumerable<IMediaType> contentTypes, MediaType mediaType)
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

            var id = contentTypeRepository.GetContentTypeId(mediaType.Id.Value);

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
                ct = contentTypeService.GetMediaType(mediaType.Alias);

                // Connect the media type and the created content type.
                contentTypeRepository.SetContentTypeId(mediaType.Id.Value, ct.Id);
            }
            else
            {
                UpdateMediaType(ct, mediaType);
            }
        }

        /// <summary>
        /// Creates a media type.
        /// </summary>
        /// <param name="mediaType">The reflected media type to create.</param>
        private void CreateMediaType(MediaType mediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            var contentType = (IMediaType)CreateContentType(() => new global::Umbraco.Core.Models.MediaType(-1), mediaType);

            contentTypeService.Save(contentType);

            // Update tracking.
            SetPropertyTypeId(contentTypeService.GetMediaType(contentType.Alias), mediaType);
        }

        /// <summary>
        /// Updates a media type.
        /// </summary>
        /// <param name="contentType">The media type to update.</param>
        /// <param name="mediaType">The reflected media type.</param>
        private void UpdateMediaType(IMediaType contentType, MediaType mediaType)
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

            contentTypeService.Save(contentType);

            // Update tracking.
            SetPropertyTypeId(contentTypeService.GetMediaType(contentType.Alias), mediaType);
        }
    }
}