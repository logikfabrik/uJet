// <copyright file="MediaTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="MediaTypeSynchronizationService" /> class. Synchronizes model types annotated using the <see cref="MediaTypeAttribute" />.
    /// </summary>
    public class MediaTypeSynchronizationService : ComposableContentTypeModelSynchronizationService<MediaType, MediaTypeAttribute, IMediaType>
    {
        private readonly IContentTypeService _contentTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizationService" /> class.
        /// </summary>
        public MediaTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.ContentTypeService,
                TypeResolver.Instance,
                TypeRepository.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" /> is <c>null</c>.</exception>
        public MediaTypeSynchronizationService(
            IContentTypeService contentTypeService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository)
            : base(typeResolver, typeRepository)
        {
            if (contentTypeService == null)
            {
                throw new ArgumentNullException(nameof(contentTypeService));
            }

            _contentTypeService = contentTypeService;
        }

        /// <summary>
        /// Gets the content type models.
        /// </summary>
        /// <value>
        /// The content type models.
        /// </value>
        protected override MediaType[] Models => Resolver.MediaTypes.ToArray();

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>
        /// The content types.
        /// </returns>
        protected override IMediaType[] GetContentTypes()
        {
            return _contentTypeService.GetAllMediaTypes().ToArray();
        }

        /// <summary>
        /// Gets a content type.
        /// </summary>
        /// <returns>
        /// A content type.
        /// </returns>
        protected override IMediaType GetContentType()
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
        protected override IMediaType GetContentType(string alias)
        {
            return _contentTypeService.GetMediaType(alias);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected override void SaveContentType(IMediaType contentType)
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
        protected override MediaType GetContentTypeModel(Type modelType)
        {
            return Models.SingleOrDefault(ctm => ctm.ModelType == modelType);
        }
    }
}