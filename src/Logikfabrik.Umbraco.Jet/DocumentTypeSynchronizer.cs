// <copyright file="DocumentTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Extensions;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Logging;

    /// <summary>
    /// The <see cref="DocumentTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="DocumentTypeAttribute" />.
    /// </summary>
    public class DocumentTypeSynchronizer : ComposableContentTypeSynchronizer<DocumentType, DocumentTypeAttribute, IContentType>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFileService _fileService;
        private readonly Lazy<DocumentType[]> _documentTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, <paramref name="fileService" />, or <paramref name="typeResolver" /> are <c>null</c>.</exception>
        public DocumentTypeSynchronizer(
            ILogService logService,
            IContentTypeService contentTypeService,
            IFileService fileService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository)
            : base(logService, typeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (typeResolver == null)
            {
                throw new ArgumentNullException(nameof(typeResolver));
            }

            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _documentTypes = new Lazy<DocumentType[]>(() => typeResolver.DocumentTypes.ToArray());
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>The models.</value>
        protected override DocumentType[] Models => _documentTypes.Value;

        /// <summary>
        /// Creates a content type.
        /// </summary>
        /// <param name="model">The model to use when creating the content type.</param>
        /// <returns>The created content type.</returns>
        internal override IContentType CreateContentType(DocumentType model)
        {
            var contentType = base.CreateContentType(model);

            SetAllowedTemplates(contentType, model);
            SetDefaultTemplate(contentType, model);

            return contentType;
        }

        /// <summary>
        /// Updates the specified content type.
        /// </summary>
        /// <param name="contentType">The content type to update.</param>
        /// <param name="model">The model to use when updating the content type.</param>
        /// <returns>The updated content type.</returns>
        internal override IContentType UpdateContentType(IContentType contentType, DocumentType model)
        {
            contentType = base.UpdateContentType(contentType, model);

            SetAllowedTemplates(contentType, model);
            SetDefaultTemplate(contentType, model);

            return contentType;
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>
        /// The content types.
        /// </returns>
        protected override IContentType[] GetContentTypes()
        {
            return _contentTypeService.GetAllContentTypes().ToArray();
        }

        /// <summary>
        /// Creates a content type.
        /// </summary>
        /// <returns>
        /// The created content type.
        /// </returns>
        protected override IContentType CreateContentType()
        {
            return new global::Umbraco.Core.Models.ContentType(-1);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
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