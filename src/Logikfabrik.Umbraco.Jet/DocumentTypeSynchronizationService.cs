// <copyright file="DocumentTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Extensions;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DocumentTypeSynchronizationService" /> class. Synchronizes model types annotated using the <see cref="DocumentTypeAttribute" />.
    /// </summary>
    public class DocumentTypeSynchronizationService : ComposableContentTypeModelSynchronizationService<DocumentType, DocumentTypeAttribute, IContentType>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeSynchronizationService" /> class.
        /// </summary>
        public DocumentTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                TypeResolver.Instance,
                TypeRepository.Instance,
                ApplicationContext.Current.Services.FileService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <param name="fileService">The file service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, or <paramref name="fileService" /> are <c>null</c>.</exception>
        public DocumentTypeSynchronizationService(
            IContentTypeService contentTypeService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository,
            IFileService fileService)
            : base(typeResolver, typeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            _contentTypeService = contentTypeService;
            _fileService = fileService;
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>The models.</value>
        protected override DocumentType[] Models => Resolver.DocumentTypes.ToArray();

        /// <summary>
        /// Creates a content type for the specified content type model.
        /// </summary>
        /// <param name="model">The content type model.</param>
        /// <returns>
        /// The created content type.
        /// </returns>
        internal override IContentType CreateContentType(DocumentType model)
        {
            var contentType = base.CreateContentType(model);

            SetAllowedTemplates(contentType, model);
            SetDefaultTemplate(contentType, model);

            return contentType;
        }

        /// <summary>
        /// Updates the content type for the specified content type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="model">The content type model.</param>
        /// <returns>
        /// The updated content type.
        /// </returns>
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
        /// Gets the content type with the specified alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>
        /// The content type with the specified alias.
        /// </returns>
        protected override IContentType GetContentType(string alias)
        {
            return _contentTypeService.GetContentType(alias);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected override void SaveContentType(IContentType contentType)
        {
            _contentTypeService.Save(contentType);
        }

        /// <summary>
        /// Gets a content type model for the specified model type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        /// A content type model for the specified model type.
        /// </returns>
        protected override DocumentType GetModel(Type modelType)
        {
            return Models.SingleOrDefault(ctm => ctm.ModelType == modelType);
        }

        /// <summary>
        /// Sets the document type allowed templates.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <param name="documentTypeModel">The document type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="documentType" />, or <paramref name="documentTypeModel" /> are <c>null</c>.</exception>
        private void SetAllowedTemplates(IContentType documentType, DocumentType documentTypeModel)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (documentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(documentTypeModel));
            }

            IEnumerable<ITemplate> allowedTemplates = new ITemplate[] { };

            var templates = documentTypeModel.Templates.Select(t => t.Alias()).ToList();
            var defaultTemplate = documentType.DefaultTemplate?.Alias;

            if (!templates.Contains(defaultTemplate))
            {
                templates.Add(documentTypeModel.DefaultTemplate.Alias());
            }

            if (templates.Any())
            {
                allowedTemplates = _fileService.GetTemplates(templates.ToArray());
            }

            if (allowedTemplates.Any())
            {
                documentType.AllowedTemplates = allowedTemplates;
            }
            else
            {
                foreach (var allowedTemplate in documentType.AllowedTemplates)
                {
                    documentType.RemoveTemplate(allowedTemplate);
                }
            }
        }

        /// <summary>
        /// Sets the document type default template.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <param name="documentTypeModel">The document type model.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="documentType" />, or <paramref name="documentTypeModel" /> are <c>null</c>.</exception>
        private void SetDefaultTemplate(IContentType documentType, DocumentType documentTypeModel)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (documentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(documentTypeModel));
            }

            ITemplate template = null;

            if (!string.IsNullOrWhiteSpace(documentTypeModel.DefaultTemplate))
            {
                template = _fileService.GetTemplate(documentTypeModel.DefaultTemplate);
            }

            documentType.SetDefaultTemplate(template);
        }
    }
}