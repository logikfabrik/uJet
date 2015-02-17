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

using Logikfabrik.Umbraco.Jet.Extensions;
using System;
using System.Linq;
using Umbraco.Core.Models;

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    public class MediaService : ContentService
    {
        public MediaService()
            : this(new UmbracoHelperWrapper(), Jet.TypeService.Instance)
        {
        }

        public MediaService(IUmbracoHelperWrapper umbracoHelperWrapper, ITypeService typeService)
            : base(umbracoHelperWrapper, typeService)
        {
        }

        public T GetMedia<T>(int id) where T : class, new()
        {
            if (!typeof(T).IsMediaType())
                throw new ArgumentException(string.Format("Type {0} is not a media type.", typeof(T)));

            return GetMedia<T>(UmbracoHelper.TypedMedia(id));
        }

        public T GetMedia<T>(IPublishedContent content) where T : class, new()
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (!typeof(T).IsMediaType())
                throw new ArgumentException(string.Format("Type {0} is not a media type.", typeof(T)));

            return (T)GetMedia(content, typeof(T));
        }

        public object GetMedia(int id, string mediaTypeAlias)
        {
            var mediaType = TypeService.MediaTypes.FirstOrDefault(t => t.Name.Alias().Equals(mediaTypeAlias));

            if (mediaType == null)
                throw new ArgumentException(
                    string.Format("Media type with alias {0} could not be found.", mediaTypeAlias),
                    "mediaTypeAlias");

            return GetMedia(UmbracoHelper.TypedMedia(id), mediaType);
        }

        public object GetMedia(IPublishedContent content, Type mediaType)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (mediaType == null)
                throw new ArgumentNullException("mediaType");

            if (!mediaType.IsMediaType())
                throw new ArgumentException(string.Format("Type {0} is not a media type.", mediaType), "mediaType");

            return GetContent(content, mediaType);
        }
    }
}
