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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    public class PreviewTemplateSynchronizationService : ISynchronizationService
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly ITypeService _documentTypeService;
        private readonly IFileService _fileService;
        private ITemplate _previewTemplate;

        public PreviewTemplateSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                ApplicationContext.Current.Services.FileService,
                TypeService.Instance) { }

        public PreviewTemplateSynchronizationService(
            IContentTypeService contentTypeService,
            IFileService fileService,
            ITypeService documentTypeService)
        {
            if (contentTypeService == null)
                throw new ArgumentNullException("contentTypeService");

            if (fileService == null)
                throw new ArgumentNullException("fileService");

            if (documentTypeService == null)
                throw new ArgumentNullException("documentTypeService");

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _documentTypeService = documentTypeService;
        }

        public void Synchronize()
        {
            var contentTypes = _contentTypeService.GetAllContentTypes().ToArray();

            foreach (
                var type in
                    GetTypes().Where(t => t.GetCustomAttribute<PreviewTemplateAttribute>() != null))
            {
                var dt = new DocumentType(type);
                var ct = contentTypes.FirstOrDefault(contentType => contentType.Alias == dt.Alias);

                if (ct == null)
                    continue;

                UpdateDocumentType(ct);
            }
        }

        private IEnumerable<Type> GetTypes()
        {
            return _documentTypeService.DocumentTypes;
        }

        private void UpdateDocumentType(IContentType contentType)
        {
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            var templates = GetAllowedTemplates(contentType);
            var template = templates.First(t => t.Alias.Equals(PreviewTemplateAttribute.TemplateName.Alias()));

            contentType.AllowedTemplates = templates;
            contentType.SetDefaultTemplate(template);

            _contentTypeService.Save(contentType);
        }

        private List<ITemplate> GetAllowedTemplates(IContentType contentType)
        {
            if (contentType == null)
                throw new ArgumentNullException("contentType");

            var templates = new List<ITemplate>();

            if (contentType.AllowedTemplates != null)
                templates.AddRange(contentType.AllowedTemplates);

            if (templates.All(t => !t.Alias.Equals(PreviewTemplateAttribute.TemplateName.Alias())))
                templates.Add(GetPreviewTemplate());

            return templates;
        }

        private ITemplate GetPreviewTemplate()
        {
            if (_previewTemplate != null)
                return _previewTemplate;

            _previewTemplate = _fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            if (_previewTemplate != null)
                return _previewTemplate;

            _fileService.SaveTemplate(
                new Template(string.Concat(PreviewTemplateAttribute.TemplateName.Alias(), ".cshtml"),
                    PreviewTemplateAttribute.TemplateName,
                    PreviewTemplateAttribute.TemplateName.Alias())
                {
                    Content = "@inherits Umbraco.Web.Mvc.UmbracoTemplatePage"
                });

            _previewTemplate = _fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            return _previewTemplate;
        }
    }
}
