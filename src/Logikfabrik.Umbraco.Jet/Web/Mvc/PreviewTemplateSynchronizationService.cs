// <copyright file="PreviewTemplateSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="PreviewTemplateSynchronizationService" /> class.
    /// </summary>
    public class PreviewTemplateSynchronizationService : ISynchronizationService
    {
        /// <summary>
        /// The content type service.
        /// </summary>
        private readonly IContentTypeService _contentTypeService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly ITypeService _documentTypeService;

        /// <summary>
        /// The file service.
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// The preview template.
        /// </summary>
        private ITemplate _previewTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewTemplateSynchronizationService" /> class.
        /// </summary>
        public PreviewTemplateSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                ApplicationContext.Current.Services.FileService,
                TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewTemplateSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="documentTypeService">The document type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, <paramref name="fileService" />, or <paramref name="documentTypeService" /> is <c>null</c>.</exception>
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

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _documentTypeService = documentTypeService;
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public void Synchronize()
        {
            var contentTypes = _contentTypeService.GetAllContentTypes().ToArray();

            foreach (var type in _documentTypeService.DocumentTypes.Where(t => Attribute.IsDefined(t, typeof(PreviewTemplateAttribute))))
            {
                var contentType = contentTypes.FirstOrDefault(t => t.Alias == new DocumentType(type).Alias);

                if (contentType == null)
                {
                    continue;
                }

                UpdateDocumentType(contentType);
            }
        }

        /// <summary>
        /// Updates the content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" /> is <c>null</c>.</exception>
        private void UpdateDocumentType(IContentType contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var allowedTemplates = GetAllowedTemplates(contentType);
            var template = allowedTemplates.First(t => t.Alias == PreviewTemplateAttribute.TemplateName.Alias());

            contentType.AllowedTemplates = allowedTemplates;
            contentType.SetDefaultTemplate(template);

            _contentTypeService.Save(contentType);
        }

        /// <summary>
        /// Gets the allowed templates for the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <returns>The allowed templates.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" /> is <c>null</c>.</exception>
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

            var previewTemplateAlias = PreviewTemplateAttribute.TemplateName.Alias();

            if (templates.All(t => t.Alias != previewTemplateAlias))
            {
                templates.Add(GetPreviewTemplate());
            }

            return templates;
        }

        /// <summary>
        /// Gets the preview template.
        /// </summary>
        /// <returns>The preview template.</returns>
        private ITemplate GetPreviewTemplate()
        {
            if (_previewTemplate != null)
            {
                return _previewTemplate;
            }

            _previewTemplate = _fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            if (_previewTemplate != null)
            {
                return _previewTemplate;
            }

            var previewTemplateAlias = PreviewTemplateAttribute.TemplateName.Alias();

            _fileService.SaveTemplate(
                new Template(
                    string.Concat(previewTemplateAlias, ".cshtml"),
                    PreviewTemplateAttribute.TemplateName,
                    previewTemplateAlias)
                {
                    Content = $"@inherits {typeof(global::Umbraco.Web.Mvc.UmbracoTemplatePage)}"
                });

            _previewTemplate = _fileService.GetTemplate(PreviewTemplateAttribute.TemplateName.Alias());

            return _previewTemplate;
        }
    }
}
