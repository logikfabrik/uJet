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
    /// The <see cref="DocumentTypeSynchronizationService" /> class. Synchronizes types annotated using the <see cref="DocumentTypeAttribute" />.
    /// </summary>
    public class DocumentTypeSynchronizationService : ContentTypeSynchronizationService
    {
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeService" />, or <paramref name="fileService" /> are <c>null</c>.</exception>
        public DocumentTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository,
            ITypeService typeService,
            IFileService fileService)
            : base(contentTypeService, contentTypeRepository)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            _fileService = fileService;
            _typeService = typeService;
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public override void Synchronize()
        {
            var jetDocumentTypes = _typeService.DocumentTypes.Select(t => new DocumentType(t, _typeService.GetComposition(t, Extensions.TypeExtensions.IsDocumentType))).ToArray();

            // No document types; there's nothing to sync.
            if (!jetDocumentTypes.Any())
            {
                return;
            }

            ValidateDocumentTypeId(jetDocumentTypes);
            ValidateDocumentTypeAlias(jetDocumentTypes);

            var documentTypes = ContentTypeService.GetAllContentTypes().Cast<IContentTypeBase>().ToArray();

            foreach (var jetMediaType in jetDocumentTypes)
            {
                Synchronize(documentTypes, jetMediaType);
            }

            // We get all document types once more to refresh them after creating/updating them.
            documentTypes = ContentTypeService.GetAllContentTypes().Cast<IContentTypeBase>().ToArray();

            Func<Type, ContentType<DocumentTypeAttribute>> constructor = t => new DocumentType(t, _typeService.GetComposition(t, Extensions.TypeExtensions.IsDocumentType));

            SetAllowedContentTypes(documentTypes, jetDocumentTypes.Cast<ContentType<DocumentTypeAttribute>>().ToArray(), constructor);
            SetComposition(documentTypes, jetDocumentTypes.Cast<ContentType<DocumentTypeAttribute>>().ToArray(), constructor);
        }

        /// <summary>
        /// Validates the uJet document type identifiers.
        /// </summary>
        /// <param name="jetDocumentTypes">The uJet document types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetDocumentTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an identifier in <paramref name="jetDocumentTypes" /> is conflicting.</exception>
        private static void ValidateDocumentTypeId(IEnumerable<DocumentType> jetDocumentTypes)
        {
            if (jetDocumentTypes == null)
            {
                throw new ArgumentNullException(nameof(jetDocumentTypes));
            }

            var set = new HashSet<Guid>();

            foreach (var jetDocumentType in jetDocumentTypes)
            {
                if (!jetDocumentType.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(jetDocumentType.Id.Value))
                {
                    throw new InvalidOperationException($"ID conflict for document type {jetDocumentType.Name}. ID {jetDocumentType.Id.Value} is already in use.");
                }

                set.Add(jetDocumentType.Id.Value);
            }
        }

        /// <summary>
        /// Validates the uJet document type aliases.
        /// </summary>
        /// <param name="jetDocumentTypes">The uJet document types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetDocumentTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an alias in <paramref name="jetDocumentTypes" /> is conflicting.</exception>
        private static void ValidateDocumentTypeAlias(IEnumerable<DocumentType> jetDocumentTypes)
        {
            if (jetDocumentTypes == null)
            {
                throw new ArgumentNullException(nameof(jetDocumentTypes));
            }

            var set = new HashSet<string>();

            foreach (var jetDocumentType in jetDocumentTypes)
            {
                if (set.Contains(jetDocumentType.Alias))
                {
                    throw new InvalidOperationException(string.Format("Alias conflict for document type {0}. Alias {0} is already in use.", jetDocumentType.Alias));
                }

                set.Add(jetDocumentType.Alias);
            }
        }

        private void Synchronize(IContentTypeBase[] documentTypes, DocumentType jetDocumentType)
        {
            if (documentTypes == null)
            {
                throw new ArgumentNullException(nameof(documentTypes));
            }

            if (jetDocumentType == null)
            {
                throw new ArgumentNullException(nameof(jetDocumentType));
            }

            var documentType = GetBaseContentType(documentTypes, jetDocumentType) as IContentType;

            if (documentType == null)
            {
                documentType = CreateDocumentType(jetDocumentType);

                if (jetDocumentType.Id.HasValue)
                {
                    ContentTypeRepository.SetContentTypeId(jetDocumentType.Id.Value, documentType.Id);
                }
            }
            else
            {
                UpdateDocumentType(documentType, jetDocumentType);
            }
        }

        /// <summary>
        /// Creates a new document type using the uJet document type.
        /// </summary>
        /// <param name="jetDocumentType">The uJet document type.</param>
        /// <returns>The created document type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetDocumentType" /> is <c>null</c>.</exception>
        internal virtual IContentType CreateDocumentType(DocumentType jetDocumentType)
        {
            if (jetDocumentType == null)
            {
                throw new ArgumentNullException(nameof(jetDocumentType));
            }

            var documentType = (IContentType)CreateContentType(() => new global::Umbraco.Core.Models.ContentType(-1), jetDocumentType);

            SetAllowedTemplates(documentType, jetDocumentType);
            SetDefaultTemplate(documentType, jetDocumentType);

            ContentTypeService.Save(documentType);

            // We get the document type once more to refresh it after updating it.
            documentType = ContentTypeService.GetContentType(documentType.Alias);

            // Update tracking.
            SetPropertyTypeId(documentType, jetDocumentType);

            return documentType;
        }

        /// <summary>
        /// Updates the document type to match the uJet document type.
        /// </summary>
        /// <param name="documentType">The document type to update.</param>
        /// <param name="jetDocumentType">The uJet document type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="documentType" />, or <paramref name="jetDocumentType" /> are <c>null</c>.</exception>
        internal virtual void UpdateDocumentType(IContentType documentType, DocumentType jetDocumentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (jetDocumentType == null)
            {
                throw new ArgumentNullException(nameof(jetDocumentType));
            }

            UpdateContentType(documentType, () => new global::Umbraco.Core.Models.ContentType(-1), jetDocumentType);
            SetAllowedTemplates(documentType, jetDocumentType);
            SetDefaultTemplate(documentType, jetDocumentType);

            ContentTypeService.Save(documentType);

            // Update tracking. We get the document type once more to refresh it after updating it.
            SetPropertyTypeId(ContentTypeService.GetContentType(documentType.Alias), jetDocumentType);
        }

        /// <summary>
        /// Sets the document type allowed templates.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <param name="jetDocumentType">The uJet document type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="documentType" />, or <paramref name="jetDocumentType" /> are <c>null</c>.</exception>
        private void SetAllowedTemplates(IContentType documentType, DocumentType jetDocumentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (jetDocumentType == null)
            {
                throw new ArgumentNullException(nameof(jetDocumentType));
            }

            IEnumerable<ITemplate> templates = new ITemplate[] { };

            if (!jetDocumentType.Templates.Any())
            {
                templates = _fileService.GetTemplates(jetDocumentType.Templates.ToArray());
            }

            documentType.AllowedTemplates = templates;
        }

        /// <summary>
        /// Sets the document type default template.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <param name="jetDocumentType">The uJet document type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="documentType" />, or <paramref name="jetDocumentType" /> are <c>null</c>.</exception>
        private void SetDefaultTemplate(IContentType documentType, DocumentType jetDocumentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (jetDocumentType == null)
            {
                throw new ArgumentNullException(nameof(jetDocumentType));
            }

            ITemplate template = null;

            if (!string.IsNullOrWhiteSpace(jetDocumentType.DefaultTemplate))
            {
                template = _fileService.GetTemplate(jetDocumentType.DefaultTemplate);
            }

            documentType.SetDefaultTemplate(template);
        }
    }
}
