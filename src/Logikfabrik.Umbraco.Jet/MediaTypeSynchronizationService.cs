﻿//----------------------------------------------------------------------------------
// <copyright file="MediaTypeSynchronizationService.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

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
                throw new ArgumentNullException("contentTypeService");
            }

            if (contentTypeRepository == null)
            {
                throw new ArgumentNullException("contentTypeRepository");
            }

            if (typeService == null)
            {
                throw new ArgumentNullException("typeService");
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
            var mediaTypes = this.typeService.MediaTypes.Select(t => new MediaType(t)).ToArray();

            ValidateMediaTypeId(mediaTypes);
            ValidateMediaTypeAlias(mediaTypes);

            foreach (var documentType in mediaTypes.Where(dt => dt.Id.HasValue))
            {
                this.SynchronizeById(this.contentTypeService.GetAllMediaTypes(), documentType);
            }

            foreach (var documentType in mediaTypes.Where(dt => !dt.Id.HasValue))
            {
                this.SynchronizeByName(this.contentTypeService.GetAllMediaTypes(), documentType);
            }

            this.SetAllowedContentTypes(
                this.contentTypeService.GetAllMediaTypes().Cast<IContentTypeBase>().ToArray(),
                mediaTypes);
        }

        private static void ValidateMediaTypeId(IEnumerable<MediaType> mediaTypes)
        {
            if (mediaTypes == null)
            {
                throw new ArgumentNullException("mediaTypes");
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
                        string.Format(
                            "ID conflict for media type {0}. ID {1} is already in use.",
                            mediaType.Name,
                            mediaType.Id.Value));
                }

                set.Add(mediaType.Id.Value);
            }
        }

        private static void ValidateMediaTypeAlias(IEnumerable<MediaType> mediaTypes)
        {
            if (mediaTypes == null)
            {
                throw new ArgumentNullException("mediaTypes");
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
                throw new ArgumentNullException("contentTypes");
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }

            if (mediaType.Id.HasValue)
            {
                throw new ArgumentException("Media type ID must be null.", "mediaType");
            }

            var ct = contentTypes.FirstOrDefault(type => type.Alias == mediaType.Alias);

            if (ct == null)
            {
                this.CreateMediaType(mediaType);
            }
            else
            {
                this.UpdateMediaType(ct, mediaType);
            }
        }

        private void SynchronizeById(IEnumerable<IMediaType> contentTypes, MediaType mediaType)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException("contentTypes");
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }

            if (!mediaType.Id.HasValue)
            {
                throw new ArgumentException("Media type ID cannot be null.", "mediaType");
            }

            IMediaType ct = null;

            var id = this.contentTypeRepository.GetContentTypeId(mediaType.Id.Value);

            if (id.HasValue)
            {
                // The media type has been synchronized before. Get the matching content type.
                // It might have been removed using the back office.
                ct = contentTypes.FirstOrDefault(type => type.Id == id.Value);
            }

            if (ct == null)
            {
                this.CreateMediaType(mediaType);

                // Get the created media type.
                ct = this.contentTypeService.GetMediaType(mediaType.Alias);

                // Connect the media type and the created content type.
                this.contentTypeRepository.SetContentTypeId(mediaType.Id.Value, ct.Id);
            }
            else
            {
                this.UpdateMediaType(ct, mediaType);
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
                throw new ArgumentNullException("mediaType");
            }

            var contentType = (IMediaType)CreateContentType(() => new global::Umbraco.Core.Models.MediaType(-1), mediaType);

            this.contentTypeService.Save(contentType);

            // Update tracking.
            this.SetPropertyTypeId(this.contentTypeService.GetMediaType(contentType.Alias), mediaType);
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
                throw new ArgumentNullException("contentType");
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }

            this.UpdateContentType(contentType, () => new global::Umbraco.Core.Models.MediaType(-1), mediaType);

            this.contentTypeService.Save(contentType);

            // Update tracking.
            this.SetPropertyTypeId(this.contentTypeService.GetMediaType(contentType.Alias), mediaType);
        }
    }
}