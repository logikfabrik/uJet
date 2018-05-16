// <copyright file="MediaTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using Mappings;

    /// <summary>
    /// The <see cref="MediaTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="MediaTypeAttribute" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class MediaTypeSynchronizer : ComposableContentTypeModelSynchronizer<MediaType, MediaTypeAttribute, IMediaType>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly Lazy<MediaType[]> _mediaTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="contentTypeService">The content type service.</param>
        /// <param name="contentTypeFinder">The content type finder.</param>
        /// <param name="propertyTypeFinder">The property type finder.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="dataTypeDefinitionService">The data type definition service.</param>
        /// <param name="typeRepository">The type repository.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public MediaTypeSynchronizer(
            IContentTypeService contentTypeService,
            IContentTypeFinder<MediaType, MediaTypeAttribute, IMediaType> contentTypeFinder,
            IPropertyTypeFinder propertyTypeFinder,
            IModelService modelService,
            IDataTypeDefinitionService dataTypeDefinitionService,
            ITypeRepository typeRepository)
            : base(contentTypeFinder, propertyTypeFinder, dataTypeDefinitionService, typeRepository)
        {
            Ensure.That(contentTypeService).IsNotNull();
            Ensure.That(modelService).IsNotNull();

            _contentTypeService = contentTypeService;
            _mediaTypes = new Lazy<MediaType[]>(() => modelService.MediaTypes.ToArray());
        }

        /// <inheritdoc />
        protected override MediaType[] Models => _mediaTypes.Value;

        /// <inheritdoc />
        protected override IMediaType[] GetContentTypes()
        {
            return _contentTypeService.GetAllMediaTypes().ToArray();
        }

        /// <inheritdoc />
        protected override IMediaType CreateContentType()
        {
            return new global::Umbraco.Core.Models.MediaType(-1);
        }

        /// <inheritdoc />
        protected override void SaveContentType(IMediaType contentType)
        {
            _contentTypeService.Save(contentType);
        }
    }
}