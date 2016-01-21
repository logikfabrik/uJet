// <copyright file="PreviewTemplateSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Jet.Data;

    /// <summary>
    /// The <see cref="PreviewTemplateSynchronizer" /> class.
    /// </summary>
    public class PreviewTemplateSynchronizer : ISynchronizer
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFileService _fileService;
        private readonly ITypeResolver _typeResolver;
        private readonly ContentTypeFinder<DocumentType, DocumentTypeAttribute, IContentType> _documentTypeFinder;

        private ITemplate _previewTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewTemplateSynchronizer" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, <paramref name="fileService" />, <paramref name="typeResolver" />, or <paramref name="typeRepository" /> are <c>null</c>.</exception>
        public PreviewTemplateSynchronizer(
            IContentTypeService contentTypeService,
            IFileService fileService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            if (typeResolver == null)
            {
                throw new ArgumentNullException(nameof(typeResolver));
            }

            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _typeResolver = typeResolver;
            _documentTypeFinder = new ContentTypeFinder<DocumentType, DocumentTypeAttribute, IContentType>(typeRepository);
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public void Run()
        {
            var documentTypes = _contentTypeService.GetAllContentTypes().ToArray();

            foreach (var model in _typeResolver.DocumentTypes.Where(model => Attribute.IsDefined(model.ModelType, typeof(PreviewTemplateAttribute), false)))
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