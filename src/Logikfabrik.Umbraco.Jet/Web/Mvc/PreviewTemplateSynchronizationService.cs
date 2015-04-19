//----------------------------------------------------------------------------------
// <copyright file="PreviewTemplateSynchronizationService.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    public class PreviewTemplateSynchronizationService : ISynchronizationService
    {
        private readonly IContentTypeService contentTypeService;
        private readonly ITypeService documentTypeService;
        private readonly IFileService fileService;
        private ITemplate previewTemplate;

        public PreviewTemplateSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                ApplicationContext.Current.Services.FileService,
                TypeService.Instance)
        {
        }

        public PreviewTemplateSynchronizationService(
            IContentTypeService contentTypeService,
            IFileService fileService,
            ITypeService documentTypeService)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException("contentTypeService");
            }

            if (fileService == null)
            {
                throw new ArgumentNullException("fileService");
            }

            if (documentTypeService == null)
            {
                throw new ArgumentNullException("documentTypeService");
            }

            this.contentTypeService = contentTypeService;
            this.fileService = fileService;
            this.documentTypeService = documentTypeService;
        }

        public void Synchronize()
        {
            var contentTypes = this.contentTypeService.GetAllContentTypes().ToArray();

            foreach (
                var type in
                    this.GetTypes().Where(t => t.GetCustomAttribute<PreviewTemplateAttribute>() != null))
            {
                var dt = new DocumentType(type);
                var ct = contentTypes.FirstOrDefault(contentType => contentType.Alias == dt.Alias);

                if (ct == null)
                {
                    continue;
                }

                this.UpdateDocumentType(ct);
            }
        }

        private IEnumerable<Type> GetTypes()
        {
            return this.documentTypeService.DocumentTypes;
        }

        private void UpdateDocumentType(IContentType contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException("contentType");
            }

            var templates = this.GetAllowedTemplates(contentType);
            var template = templates.First(t => t.Alias == PreviewTemplateAttribute.TemplateName.Alias());

            contentType.AllowedTemplates = templates;
            contentType.SetDefaultTemplate(template);

            this.contentTypeService.Save(contentType);
        }

        private List<ITemplate> GetAllowedTemplates(IContentType contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException("contentType");
            }

            var templates = new List<ITemplate>();

            if (contentType.AllowedTemplates != null)
            {
                templates.AddRange(contentType.AllowedTemplates);
            }

            if (templates.All(t => t.Alias != PreviewTemplateAttribute.TemplateName.Alias()))
            {
                templates.Add(this.GetPreviewTemplate());
            }

            return templates;
        }

        private ITemplate GetPreviewTemplate()
        {
            if (this.previewTemplate != null)
            {
                return this.previewTemplate;
            }

            this.previewTemplate = this.fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            if (this.previewTemplate != null)
            {
                return this.previewTemplate;
            }

            this.fileService.SaveTemplate(
                new Template(
                    string.Concat(PreviewTemplateAttribute.TemplateName.Alias(), ".cshtml"),
                    PreviewTemplateAttribute.TemplateName,
                    PreviewTemplateAttribute.TemplateName.Alias())
                {
                    Content = "@inherits Umbraco.Web.Mvc.UmbracoTemplatePage"
                });

            this.previewTemplate = this.fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            return this.previewTemplate;
        }
    }
}
