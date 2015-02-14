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
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet
{
    public class DocumentTypeSynchronizationService : ContentTypeSynchronizationService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFileService _fileService;
        private readonly ITypeService _typeService;

        public DocumentTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                ApplicationContext.Current.Services.FileService,
                TypeService.Instance) { }

        public DocumentTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IFileService fileService,
            ITypeService typeService)
            : base(contentTypeService)
        {
            if (contentTypeService == null)
                throw new ArgumentNullException("contentTypeService");

            if (fileService == null)
                throw new ArgumentNullException("fileService");

            if (typeService == null)
                throw new ArgumentNullException("typeService");

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _typeService = typeService;
        }

        /// <summary>
        /// Synchronizes document types.
        /// </summary>
        public override void Synchronize()
        {
            var contentTypes = _contentTypeService.GetAllContentTypes().ToArray();
            var documentTypes = _typeService.DocumentTypes.Select(t => new DocumentType(t)).ToArray();

            // Create and/or update document types.
            foreach (var documentType in documentTypes)
                if (contentTypes.All(ct => ct.Alias != documentType.Alias))
                    CreateDocumentType(documentType);
                else
                    UpdateDocumentType(contentTypes.First(ct => ct.Alias == documentType.Alias), documentType);

            // ReSharper disable once CoVariantArrayConversion
            SetAllowedContentTypes(contentTypes, documentTypes);
        }

        /// <summary>
        /// Creates a document type.
        /// </summary>
        /// <param name="documentType">The reflected document type to create.</param>
        private void CreateDocumentType(DocumentType documentType)
        {
            if (documentType == null)
                throw new ArgumentNullException("documentType");

            var contentType = (IContentType)CreateContentType(() => new ContentType(-1), documentType);

            SetTemplates(contentType, documentType);
            SetDefaultTemplate(contentType, documentType);

            CreatePropertyTypes(contentType, documentType.Properties);

            _contentTypeService.Save(contentType);
        }

        /// <summary>
        /// Updates a document type.
        /// </summary>
        /// <param name="contentType">The document type to update.</param>
        /// <param name="documentType">The reflected document type.</param>
        private void UpdateDocumentType(IContentType contentType, DocumentType documentType)
        {
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            if (documentType == null)
                throw new ArgumentNullException("documentType");

            UpdateContentType(contentType, () => new ContentType(-1), documentType);
            SetTemplates(contentType, documentType);
            SetDefaultTemplate(contentType, documentType);

            _contentTypeService.Save(contentType);
        }

        /// <summary>
        /// Sets document type templates.
        /// </summary>
        /// <param name="contentType">The document type to set templates for.</param>
        /// <param name="documentType">The reflected document type to set templates for.</param>
        private void SetTemplates(IContentType contentType, DocumentType documentType)
        {
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            if (documentType == null)
                throw new ArgumentNullException("documentType");

            IEnumerable<ITemplate> templates = new ITemplate[] { };

            if (documentType.Templates != null && !documentType.Templates.Any())
                templates =
                    _fileService.GetTemplates(documentType.Templates.ToArray()).Where(template => template != null);

            contentType.AllowedTemplates = templates;
        }

        /// <summary>
        /// Sets a document type default template.
        /// </summary>
        /// <param name="contentType">The document type to set default template for.</param>
        /// <param name="documentType">The reflected document type to set default template for.</param>
        private void SetDefaultTemplate(IContentType contentType, DocumentType documentType)
        {
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            if (documentType == null)
                throw new ArgumentNullException("documentType");

            ITemplate template = null;

            if (!string.IsNullOrWhiteSpace(documentType.DefaultTemplate))
                template = _fileService.GetTemplate(documentType.DefaultTemplate);

            contentType.SetDefaultTemplate(template);
        }
    }
}
