// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet
{
    public class MediaTypeSynchronizationService : ContentTypeSynchronizationService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly ITypeService _typeService;

        public MediaTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                TypeService.Instance) { }

        public MediaTypeSynchronizationService(
            IContentTypeService contentTypeService,
            ITypeService typeService)
            : base(contentTypeService)
        {
            if (contentTypeService == null)
                throw new ArgumentNullException("contentTypeService");

            if (typeService == null)
                throw new ArgumentNullException("typeService");

            _contentTypeService = contentTypeService;
            _typeService = typeService;
        }

        /// <summary>
        /// Synchronizes media types.
        /// </summary>
        public override void Synchronize()
        {
            var contentTypes = _contentTypeService.GetAllMediaTypes().ToArray();
            var mediaTypes = _typeService.MediaTypes.Select(t => new MediaType(t)).ToArray();

            // Create and/or update media types.
            foreach (var mediaType in mediaTypes)
                if (contentTypes.All(ct => ct.Name != mediaType.Name))
                    CreateMediaType(mediaType);
                else
                    UpdateMediaType(contentTypes.First(ct => ct.Name == mediaType.Name), mediaType);

            // ReSharper disable once CoVariantArrayConversion
            SetAllowedContentTypes(contentTypes, mediaTypes);
        }

        /// <summary>
        /// Creates a media type.
        /// </summary>
        /// <param name="mediaType">The reflected media type to create.</param>
        private void CreateMediaType(MediaType mediaType)
        {
            if (mediaType == null)
                throw new ArgumentNullException("mediaType");

            var contentType = (IMediaType)CreateContentType(() => new global::Umbraco.Core.Models.MediaType(-1), mediaType);

            CreatePropertyTypes(contentType, mediaType.Properties);

            _contentTypeService.Save(contentType);
        }

        /// <summary>
        /// Updates a media type.
        /// </summary>
        /// <param name="contentType">The media type to update.</param>
        /// <param name="mediaType">The reflected media type.</param>
        private void UpdateMediaType(IMediaType contentType, MediaType mediaType)
        {
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            if (mediaType == null)
                throw new ArgumentNullException("mediaType");

            UpdateContentType(contentType, () => new global::Umbraco.Core.Models.MediaType(-1), mediaType);

            _contentTypeService.Save(contentType);
        }
    }
}