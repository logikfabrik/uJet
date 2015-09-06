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
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    public class DocumentTypeSynchronizationService : ContentTypeSynchronizationService
    {
        private readonly IContentTypeRepository contentTypeRepository;
        private readonly IContentTypeService contentTypeService;
        private readonly IFileService fileService;
        private readonly ITypeService typeService;

        public DocumentTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database)),
                ApplicationContext.Current.Services.FileService,
                TypeService.Instance)
        {
        }

        public DocumentTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository,
            IFileService fileService,
            ITypeService typeService)
            : base(contentTypeService, contentTypeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            if (contentTypeRepository == null)
            {
                throw new ArgumentNullException(nameof(contentTypeRepository));
            }

            if (fileService == null)
            {
                throw new ArgumentNullException(nameof(fileService));
            }

            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            this.contentTypeService = contentTypeService;
            this.contentTypeRepository = contentTypeRepository;
            this.fileService = fileService;
            this.typeService = typeService;
        }

        /// <summary>
        /// Synchronizes document types.
        /// </summary>
        public override void Synchronize()
        {
            var documentTypes = typeService.DocumentTypes.Select(t => new DocumentType(t)).ToArray();

            ValidateDocumentTypeId(documentTypes);
            ValidateDocumentTypeAlias(documentTypes);

            foreach (var documentType in documentTypes.Where(dt => dt.Id.HasValue))
            {
                SynchronizeById(contentTypeService.GetAllContentTypes(), documentType);
            }

            foreach (var documentType in documentTypes.Where(dt => !dt.Id.HasValue))
            {
                SynchronizeByName(contentTypeService.GetAllContentTypes(), documentType);
            }

            SetAllowedContentTypes(
                contentTypeService.GetAllContentTypes().Cast<IContentTypeBase>().ToArray(), documentTypes);
        }

        private static void ValidateDocumentTypeId(IEnumerable<DocumentType> documentTypes)
        {
            if (documentTypes == null)
            {
                throw new ArgumentNullException(nameof(documentTypes));
            }

            var set = new HashSet<Guid>();

            foreach (var documentType in documentTypes)
            {
                if (!documentType.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(documentType.Id.Value))
                {
                    throw new InvalidOperationException(
                        $"ID conflict for document type {documentType.Name}. ID {documentType.Id.Value} is already in use.");
                }

                set.Add(documentType.Id.Value);
            }
        }

        private static void ValidateDocumentTypeAlias(IEnumerable<DocumentType> documentTypes)
        {
            if (documentTypes == null)
            {
                throw new ArgumentNullException(nameof(documentTypes));
            }

            var set = new HashSet<string>();

            foreach (var documentType in documentTypes)
            {
                if (set.Contains(documentType.Alias))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Alias conflict for document type {0}. Alias {0} is already in use.",
                            documentType.Alias));
                }

                set.Add(documentType.Alias);
            }
        }

        private void SynchronizeByName(IEnumerable<IContentType> contentTypes, DocumentType documentType)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (documentType.Id.HasValue)
            {
                throw new ArgumentException("Document type ID must be null.", nameof(documentType));
            }

            var ct = contentTypes.FirstOrDefault(type => type.Alias == documentType.Alias);

            if (ct == null)
            {
                CreateDocumentType(documentType);
            }
            else
            {
                UpdateDocumentType(ct, documentType);
            }
        }

        private void SynchronizeById(IEnumerable<IContentType> contentTypes, DocumentType documentType)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            if (!documentType.Id.HasValue)
            {
                throw new ArgumentException("Document type ID cannot be null.", nameof(documentType));
            }

            IContentType ct = null;

            var id = contentTypeRepository.GetContentTypeId(documentType.Id.Value);

            if (id.HasValue)
            {
                // The document type has been synchronized before. Get the matching content type.
                // It might have been removed using the back office.
                ct = contentTypes.FirstOrDefault(type => type.Id == id.Value);
            }

            if (ct == null)
            {
                CreateDocumentType(documentType);

                // Get the created content type.
                ct = contentTypeService.GetContentType(documentType.Alias);

                // Connect the document type and the created content type.
                contentTypeRepository.SetContentTypeId(documentType.Id.Value, ct.Id);
            }
            else
            {
                UpdateDocumentType(ct, documentType);
            }
        }

        /// <summary>
        /// Creates a document type.
        /// </summary>
        /// <param name="documentType">The reflected document type to create.</param>
        private void CreateDocumentType(DocumentType documentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            var contentType = (IContentType)CreateContentType(() => new ContentType(-1), documentType);

            SetTemplates(contentType, documentType);
            SetDefaultTemplate(contentType, documentType);

            contentTypeService.Save(contentType);

            // Update tracking.
            SetPropertyTypeId(contentTypeService.GetContentType(contentType.Alias), documentType);
        }

        /// <summary>
        /// Updates a document type.
        /// </summary>
        /// <param name="contentType">The document type to update.</param>
        /// <param name="documentType">The reflected document type.</param>
        private void UpdateDocumentType(IContentType contentType, DocumentType documentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            UpdateContentType(contentType, () => new ContentType(-1), documentType);
            SetTemplates(contentType, documentType);
            SetDefaultTemplate(contentType, documentType);

            contentTypeService.Save(contentType);

            // Update tracking.
            SetPropertyTypeId(contentTypeService.GetContentType(contentType.Alias), documentType);
        }

        /// <summary>
        /// Sets document type templates.
        /// </summary>
        /// <param name="contentType">The document type to set templates for.</param>
        /// <param name="documentType">The reflected document type to set templates for.</param>
        private void SetTemplates(IContentType contentType, DocumentType documentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            IEnumerable<ITemplate> templates = new ITemplate[] { };

            if (documentType.Templates != null && !documentType.Templates.Any())
            {
                templates =
                    fileService.GetTemplates(documentType.Templates.ToArray()).Where(template => template != null);
            }

            contentType.AllowedTemplates = templates;
        }

        /// <summary>
        /// Sets a document type default template.
        /// </summary>
        /// <param name="contentType">The document type to set default template for.</param>
        /// <param name="documentType">The reflected document type to set default template for.</param>
        private void SetDefaultTemplate(IContentType contentType, DocumentType documentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            ITemplate template = null;

            if (!string.IsNullOrWhiteSpace(documentType.DefaultTemplate))
            {
                template = fileService.GetTemplate(documentType.DefaultTemplate);
            }

            contentType.SetDefaultTemplate(template);
        }
    }
}
