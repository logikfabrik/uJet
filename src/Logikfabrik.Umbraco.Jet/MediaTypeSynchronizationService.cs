// <copyright file="MediaTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="MediaTypeSynchronizationService" /> class. Synchronizes model types annotated using the <see cref="MediaTypeAttribute" />.
    /// </summary>
    public class MediaTypeSynchronizationService : ContentTypeSynchronizationService<MediaType, MediaTypeAttribute>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly ITypeService _typeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizationService" /> class.
        /// </summary>
        public MediaTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax)),
                TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, or <paramref name="typeService" /> are <c>null</c>.</exception>
        public MediaTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository,
            ITypeService typeService)
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

            _contentTypeService = contentTypeService;
            _typeService = typeService;
        }

        /// <summary>
        /// Gets the content type models.
        /// </summary>
        /// <value>
        /// The content type models.
        /// </value>
        protected override MediaType[] ContentTypeModels
        {
            get
            {
                return _typeService.MediaTypes.Select(t => new MediaType(t)).ToArray();
            }
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>
        /// The content types.
        /// </returns>
        protected override IContentTypeBase[] GetContentTypes()
        {
            return _contentTypeService.GetAllMediaTypes().Cast<IContentTypeBase>().ToArray();
        }

        /// <summary>
        /// Gets a content type.
        /// </summary>
        /// <returns>
        /// A content type.
        /// </returns>
        protected override IContentTypeBase GetContentType()
        {
            return new global::Umbraco.Core.Models.MediaType(-1);
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
            return _contentTypeService.GetMediaType(alias);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected override void SaveContentType(IContentTypeBase contentType)
        {
            _contentTypeService.Save((IMediaType)contentType);
        }

        /// <summary>
        /// Gets a content type model for the specified model type.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        /// A content type model for the specified model type.
        /// </returns>
        protected override MediaType GetContentTypeModel(Type modelType)
        {
            return new MediaType(modelType);
        }
    }
}