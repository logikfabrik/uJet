// <copyright file="MediaTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Logging;

    /// <summary>
    /// The <see cref="MediaTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="MediaTypeAttribute" />.
    /// </summary>
    public class MediaTypeSynchronizer : ComposableContentTypeSynchronizer<MediaType, MediaTypeAttribute, IMediaType>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly Lazy<MediaType[]> _mediaTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypeService" />, or <paramref name="typeResolver" /> are <c>null</c>.</exception>
        public MediaTypeSynchronizer(
            ILogService logService,
            IContentTypeService contentTypeService,
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

            _contentTypeService = contentTypeService;
            _mediaTypes = new Lazy<MediaType[]>(() => typeResolver.MediaTypes.ToArray());
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>The models.</value>
        protected override MediaType[] Models => _mediaTypes.Value;

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
        /// Creates a content type.
        /// </summary>
        /// <returns>
        /// The created content type.
        /// </returns>
        protected override IMediaType CreateContentType()
        {
            return new global::Umbraco.Core.Models.MediaType(-1);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected override void SaveContentType(IMediaType contentType)
        {
            _contentTypeService.Save(contentType);
        }
    }
}