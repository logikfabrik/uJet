// <copyright file="PreviewTemplateSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

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
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            if (documentTypeService == null)
            {
                throw new ArgumentNullException(nameof(documentTypeService));
            }

            this.contentTypeService = contentTypeService;
            this.fileService = fileService;
            this.documentTypeService = documentTypeService;
        }

        public void Synchronize()
        {
            var contentTypes = contentTypeService.GetAllContentTypes().ToArray();

            foreach (
                var type in
                    GetTypes().Where(t => t.GetCustomAttribute<PreviewTemplateAttribute>() != null))
            {
                var dt = new DocumentType(type);
                var ct = contentTypes.FirstOrDefault(contentType => contentType.Alias == dt.Alias);

                if (ct == null)
                {
                    continue;
                }

                UpdateDocumentType(ct);
            }
        }

        private IEnumerable<Type> GetTypes()
        {
            return documentTypeService.DocumentTypes;
        }

        private void UpdateDocumentType(IContentType contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var templates = GetAllowedTemplates(contentType);
            var template = templates.First(t => t.Alias == PreviewTemplateAttribute.TemplateName.Alias());

            contentType.AllowedTemplates = templates;
            contentType.SetDefaultTemplate(template);

            contentTypeService.Save(contentType);
        }

        private List<ITemplate> GetAllowedTemplates(IContentType contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var templates = new List<ITemplate>();

            if (contentType.AllowedTemplates != null)
            {
                templates.AddRange(contentType.AllowedTemplates);
            }

            if (templates.All(t => t.Alias != PreviewTemplateAttribute.TemplateName.Alias()))
            {
                templates.Add(GetPreviewTemplate());
            }

            return templates;
        }

        private ITemplate GetPreviewTemplate()
        {
            if (previewTemplate != null)
            {
                return previewTemplate;
            }

            previewTemplate = fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            if (previewTemplate != null)
            {
                return previewTemplate;
            }

            fileService.SaveTemplate(
                new Template(
                    string.Concat(PreviewTemplateAttribute.TemplateName.Alias(), ".cshtml"),
                    PreviewTemplateAttribute.TemplateName,
                    PreviewTemplateAttribute.TemplateName.Alias())
                {
                    Content = "@inherits Umbraco.Web.Mvc.UmbracoTemplatePage"
                });

            previewTemplate = fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            return previewTemplate;
        }
    }
}
