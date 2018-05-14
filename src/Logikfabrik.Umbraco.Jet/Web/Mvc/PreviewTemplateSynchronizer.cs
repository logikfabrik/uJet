// <copyright file="PreviewTemplateSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;
    using Extensions;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Data;
    using Logging;

    /// <summary>
    /// The <see cref="PreviewTemplateSynchronizer" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class PreviewTemplateSynchronizer : ISynchronizer
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFileService _fileService;
        private readonly IModelService _modelService;
        private readonly ContentTypeFinder<DocumentType, DocumentTypeAttribute, IContentType> _documentTypeFinder;

        private ITemplate _previewTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewTemplateSynchronizer" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="typeRepository">The type repository.</param>
        public PreviewTemplateSynchronizer(
            ILogService logService,
            IContentTypeService contentTypeService,
            IFileService fileService,
            IModelService modelService,
            ITypeRepository typeRepository)
        {
            Ensure.That(logService).IsNotNull();
            Ensure.That(contentTypeService).IsNotNull();
            Ensure.That(fileService).IsNotNull();
            Ensure.That(modelService).IsNotNull();
            Ensure.That(typeRepository).IsNotNull();

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _modelService = modelService;
            _documentTypeFinder = new ContentTypeFinder<DocumentType, DocumentTypeAttribute, IContentType>(logService, typeRepository);
        }

        /// <inheritdoc />
        public void Run()
        {
            var documentTypes = _contentTypeService.GetAllContentTypes().ToArray();

            foreach (var model in _modelService.DocumentTypes.Where(model => Attribute.IsDefined(model.ModelType, typeof(PreviewTemplateAttribute), false)))
            {
                var documentType = _documentTypeFinder.Find(model, documentTypes).SingleOrDefault();

                if (documentType == null)
                {
                    continue;
                }

                UpdateDocumentType(documentType);
            }
        }

        /// <summary>
        /// Updates the specified document type.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        private void UpdateDocumentType(IContentType documentType)
        {
            var allowedTemplates = GetAllowedTemplates(documentType);
            var template = allowedTemplates.Single(t => t.Alias == PreviewTemplateAttribute.TemplateName.Alias());

            documentType.AllowedTemplates = allowedTemplates;
            documentType.SetDefaultTemplate(template);

            _contentTypeService.Save(documentType);
        }

        /// <summary>
        /// Gets the allowed templates for the specified document type.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <returns>The allowed templates.</returns>
        private List<ITemplate> GetAllowedTemplates(IContentType documentType)
        {
            var templates = new List<ITemplate>();

            if (documentType.AllowedTemplates != null)
            {
                templates.AddRange(documentType.AllowedTemplates);
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