// <copyright file="DocumentTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="DocumentTypeSynchronizationService" /> class. Synchronizes model types annotated using the <see cref="DocumentTypeAttribute" />.
    /// </summary>
    public class DocumentTypeSynchronizationService : ContentTypeSynchronizationService<DocumentType, DocumentTypeAttribute>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly ITypeService _typeService;
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeSynchronizationService" /> class.
        /// </summary>
        public DocumentTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax)),
                TypeService.Instance,
                ApplicationContext.Current.Services.FileService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <param name="typeService">The type service.</param>
        /// <param name="fileService">The file service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, <paramref name="typeService" />, or <paramref name="fileService" /> are <c>null</c>.</exception>
        public DocumentTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository,
            ITypeService typeService,
            IFileService fileService)
            : base(contentTypeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            _contentTypeService = contentTypeService;
            _fileService = fileService;
            _typeService = typeService;
        }

        /// <summary>
        /// Gets the content type models.
        /// </summary>
        /// <value>
        /// The content type models.
        /// </value>
        protected override DocumentType[] ContentTypeModels
        {
            get
            {
                return _typeService.DocumentTypes.Select(t => new DocumentType(t)).ToArray();
            }
        }

        /// <summary>
        /// Creates a content type for the specified content type model.
        /// </summary>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <returns>
        /// The created content type.
        /// </returns>
        internal override IContentTypeBase CreateContentType(DocumentType contentTypeModel)
        {
            var contentType = base.CreateContentType(contentTypeModel);

            SetAllowedTemplates((IContentType)contentType, contentTypeModel);
            SetDefaultTemplate((IContentType)contentType, contentTypeModel);

            return contentType;
        }

        /// <summary>
        /// Updates the content type for the specified content type model.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <returns>
        /// The updated content type.
        /// </returns>
        internal override IContentTypeBase UpdateContentType(IContentTypeBase contentType, DocumentType contentTypeModel)
        {
            contentType = base.UpdateContentType(contentType, contentTypeModel);

            SetAllowedTemplates((IContentType)contentType, contentTypeModel);
            SetDefaultTemplate((IContentType)contentType, contentTypeModel);

            return contentType;
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>
        /// The content types.
        /// </returns>
        protected override IContentTypeBase[] GetContentTypes()
        {
            return _contentTypeService.GetAllContentTypes().Cast<IContentTypeBase>().ToArray();
        }

        /// <summary>
        /// Gets a content type.
        /// </summary>
        /// <returns>
        /// A content type.
        /// </returns>
        protected override IContentTypeBase GetContentType()
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
        protected override IContentTypeBase GetContentType(string alias)
        {
            return _contentTypeService.GetContentType(alias);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected override void SaveContentType(IContentTypeBase contentType)
        {
            _contentTypeService.Save((IContentType)contentType);
        }

        /// <summary>
        /// Gets a content type model for the specified model type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        /// A content type model for the specified model type.
        /// </returns>
        protected override DocumentType GetContentTypeModel(Type modelType)
        {
            return new DocumentType(modelType);
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

            IEnumerable<ITemplate> templates = new ITemplate[] { };

            if (!documentTypeModel.Templates.Any())
            {
                templates = _fileService.GetTemplates(documentTypeModel.Templates.ToArray());
            }

            documentType.AllowedTemplates = templates;
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