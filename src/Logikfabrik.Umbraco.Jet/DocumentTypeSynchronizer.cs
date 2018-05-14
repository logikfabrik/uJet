// <copyright file="DocumentTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using EnsureThat;
    using Extensions;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Logging;
    using Mappings;

    /// <summary>
    /// The <see cref="DocumentTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="DocumentTypeAttribute" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class DocumentTypeSynchronizer : ComposableContentTypeModelSynchronizer<DocumentType, DocumentTypeAttribute, IContentType>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFileService _fileService;
        private readonly Lazy<DocumentType[]> _documentTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="dataTypeDefinitionService">The data type definition service.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public DocumentTypeSynchronizer(
            ILogService logService,
            ITypeRepository typeRepository,
            IModelService modelService,
            IContentTypeService contentTypeService,
            IFileService fileService,
            IDataTypeDefinitionService dataTypeDefinitionService)
            : base(logService, typeRepository, dataTypeDefinitionService)
        {
            Ensure.That(modelService).IsNotNull();
            Ensure.That(contentTypeService).IsNotNull();
            Ensure.That(fileService).IsNotNull();

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _documentTypes = new Lazy<DocumentType[]>(() => modelService.DocumentTypes.ToArray());
        }

        /// <inheritdoc />
        protected override DocumentType[] Models => _documentTypes.Value;

        /// <inheritdoc />
        internal override IContentType CreateContentType(DocumentType model)
        {
            var contentType = base.CreateContentType(model);

            SetAllowedTemplates(contentType, model);
            SetDefaultTemplate(contentType, model);

            return contentType;
        }

        /// <inheritdoc />
        internal override IContentType UpdateContentType(IContentType contentType, DocumentType model)
        {
            contentType = base.UpdateContentType(contentType, model);

            SetAllowedTemplates(contentType, model);
            SetDefaultTemplate(contentType, model);

            return contentType;
        }

        /// <inheritdoc />
        protected override IContentType[] GetContentTypes()
        {
            return _contentTypeService.GetAllContentTypes().ToArray();
        }

        /// <inheritdoc />
        protected override IContentType CreateContentType()
        {
            return new global::Umbraco.Core.Models.ContentType(-1);
        }

        /// <inheritdoc />
        protected override void SaveContentType(IContentType contentType)
        {
            _contentTypeService.Save(contentType);
        }

        private void SetAllowedTemplates(IContentType contentType, DocumentType model)
        {
            IEnumerable<ITemplate> allowedTemplates = new ITemplate[] { };

            var templates = model.Templates.Select(template => template.Alias()).ToList();
            var defaultTemplate = contentType.DefaultTemplate?.Alias;

            if (!string.IsNullOrWhiteSpace(defaultTemplate) && !templates.Contains(defaultTemplate, StringComparer.InvariantCultureIgnoreCase))
            {
                templates.Add(defaultTemplate);
            }

            if (templates.Any())
            {
                allowedTemplates = _fileService.GetTemplates(templates.ToArray());
            }

            if (allowedTemplates.Any())
            {
                contentType.AllowedTemplates = allowedTemplates;
            }
            else
            {
                foreach (var allowedTemplate in contentType.AllowedTemplates)
                {
                    contentType.RemoveTemplate(allowedTemplate);
                }
            }
        }

        private void SetDefaultTemplate(IContentType contentType, DocumentType model)
        {
            ITemplate template = null;

            if (!string.IsNullOrWhiteSpace(model.DefaultTemplate))
            {
                template = _fileService.GetTemplate(model.DefaultTemplate.Alias());
            }

            contentType.SetDefaultTemplate(template);
        }
    }
}