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
    public class DocumentService : ContentService
    {
        public DocumentService()
            : this(new UmbracoHelperWrapper(), Jet.TypeService.Instance)
        {
        }

        public DocumentService(IUmbracoHelperWrapper umbracoHelperWrapper, ITypeService typeService)
            : base(umbracoHelperWrapper, typeService)
        {
        }

        public T GetDocument<T>(int id) where T : class, new()
        {
            if (!typeof(T).IsDocumentType())
                throw new ArgumentException(string.Format("Type {0} is not a document type.", typeof(T)));

            return GetDocument<T>(UmbracoHelper.TypedDocument(id));
        }

        public T GetDocument<T>(IPublishedContent content) where T : class, new()
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (!typeof(T).IsDocumentType())
                throw new ArgumentException(string.Format("Type {0} is not a document type.", typeof(T)));

            return (T)GetDocument(content, typeof(T));
        }

        public object GetDocument(int id, string documentTypeAlias)
        {
            var documentType = TypeService.DocumentTypes.FirstOrDefault(t => t.Name.Alias() == documentTypeAlias);

            if (documentType == null)
                throw new ArgumentException(
                    string.Format("Document type with alias {0} could not be found.", documentTypeAlias),
                    "documentTypeAlias");

            return GetDocument(UmbracoHelper.TypedDocument(id), documentType);
        }

        public object GetDocument(IPublishedContent content, Type documentType)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (documentType == null)
                throw new ArgumentNullException("documentType");

            if (!documentType.IsDocumentType())
                throw new ArgumentException(string.Format("Type {0} is not a document type.", documentType), "documentType");

            return GetContent(content, documentType);
        }
    }
}