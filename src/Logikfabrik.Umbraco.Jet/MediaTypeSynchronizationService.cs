// <copyright file="MediaTypeSynchronizationService.cs" company="Logikfabrik">
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
    /// The <see cref="MediaTypeSynchronizationService" /> class. Synchronizes types annotated using the <see cref="MediaTypeAttribute" />.
    /// </summary>
    public class MediaTypeSynchronizationService : ContentTypeSynchronizationService
    {
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeService" /> is <c>null</c>.</exception>
        public MediaTypeSynchronizationService(
            IContentTypeService contentTypeService,
            IContentTypeRepository contentTypeRepository,
            ITypeService typeService)
            : base(contentTypeService, contentTypeRepository)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            _typeService = typeService;
        }

        /// <summary>
        /// Synchronizes this instance.
        /// </summary>
        public override void Synchronize()
        {
            var jetMediaTypes = _typeService.MediaTypes.Select(t => new MediaType(t, _typeService.GetComposition(t, Extensions.TypeExtensions.IsMediaType))).ToArray();

            // No media types; there's nothing to sync.
            if (!jetMediaTypes.Any())
            {
                return;
            }

            ValidateMediaTypeId(jetMediaTypes);
            ValidateMediaTypeAlias(jetMediaTypes);

            var mediaTypes = ContentTypeService.GetAllMediaTypes().Cast<IContentTypeBase>().ToArray();

            foreach (var jetMediaType in jetMediaTypes)
            {
                Synchronize(mediaTypes, jetMediaType);
            }

            // We get all media types once more to refresh them after creating/updating them.
            mediaTypes = ContentTypeService.GetAllMediaTypes().Cast<IContentTypeBase>().ToArray();

            Func<Type, ContentType<MediaTypeAttribute>> constructor = t => new MediaType(t, _typeService.GetComposition(t, Extensions.TypeExtensions.IsMediaType));

            SetAllowedContentTypes(mediaTypes, jetMediaTypes.Cast<ContentType<MediaTypeAttribute>>().ToArray(), constructor);
            SetComposition(mediaTypes, jetMediaTypes.Cast<ContentType<MediaTypeAttribute>>().ToArray(), constructor);
        }

        /// <summary>
        /// Validates the uJet media type identifiers.
        /// </summary>
        /// <param name="jetMediaTypes">The uJet media types.</param>
        /// <exception cref="ArgumentNullException">>Thrown if <paramref name="jetMediaTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an identifier in <paramref name="jetMediaTypes" /> is conflicting.</exception>
        private static void ValidateMediaTypeId(MediaType[] jetMediaTypes)
        {
            if (jetMediaTypes == null)
            {
                throw new ArgumentNullException(nameof(jetMediaTypes));
            }

            var set = new HashSet<Guid>();

            foreach (var jetMediaType in jetMediaTypes)
            {
                if (!jetMediaType.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(jetMediaType.Id.Value))
                {
                    throw new InvalidOperationException($"ID conflict for media type {jetMediaType.Name}. ID {jetMediaType.Id.Value} is already in use.");
                }

                set.Add(jetMediaType.Id.Value);
            }
        }

        /// <summary>
        /// Validates the uJet media type aliases.
        /// </summary>
        /// <param name="jetMediaTypes">The uJet media types.</param>
        /// <exception cref="ArgumentNullException">>Thrown if <paramref name="jetMediaTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an alias in <paramref name="jetMediaTypes" /> is conflicting.</exception>
        private static void ValidateMediaTypeAlias(MediaType[] jetMediaTypes)
        {
            if (jetMediaTypes == null)
            {
                throw new ArgumentNullException(nameof(jetMediaTypes));
            }

            var set = new HashSet<string>();

            foreach (var jetMediaType in jetMediaTypes)
            {
                if (set.Contains(jetMediaType.Alias))
                {
                    throw new InvalidOperationException(string.Format("Alias conflict for media type {0}. Alias {0} is already in use.", jetMediaType.Alias));
                }

                set.Add(jetMediaType.Alias);
            }
        }

        private void Synchronize(IContentTypeBase[] mediaTypes, MediaType jetMediaType)
        {
            if (mediaTypes == null)
            {
                throw new ArgumentNullException(nameof(mediaTypes));
            }

            if (jetMediaType == null)
            {
                throw new ArgumentNullException(nameof(jetMediaType));
            }

            var mediaType = GetBaseContentType(mediaTypes, jetMediaType) as IMediaType;

            if (mediaType == null)
            {
                mediaType = CreateMediaType(jetMediaType);

                if (jetMediaType.Id.HasValue)
                {
                    ContentTypeRepository.SetContentTypeId(jetMediaType.Id.Value, mediaType.Id);
                }
            }
            else
            {
                UpdateMediaType(mediaType, jetMediaType);
            }
        }

        /// <summary>
        /// Creates a new media type using the uJet media type.
        /// </summary>
        /// <param name="jetMediaType">The uJet media type.</param>
        /// <returns>The created media type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetMediaType" /> is <c>null</c>.</exception>
        internal virtual IMediaType CreateMediaType(MediaType jetMediaType)
        {
            if (jetMediaType == null)
            {
                throw new ArgumentNullException(nameof(jetMediaType));
            }

            var mediaType = (IMediaType)CreateContentType(() => new global::Umbraco.Core.Models.MediaType(-1), jetMediaType);

            ContentTypeService.Save(mediaType);

            // We get the media type once more to refresh it after updating it.
            mediaType = ContentTypeService.GetMediaType(mediaType.Alias);

            // Update tracking.
            SetPropertyTypeId(mediaType, jetMediaType);

            return mediaType;
        }

        /// <summary>
        /// Updates the media type to match the uJet media type.
        /// </summary>
        /// <param name="mediaType">The media type to update.</param>
        /// <param name="jetMediaType">The uJet media type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mediaType" />, or <paramref name="jetMediaType" /> are <c>null</c>.</exception>
        internal virtual void UpdateMediaType(IMediaType mediaType, MediaType jetMediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            if (jetMediaType == null)
            {
                throw new ArgumentNullException(nameof(jetMediaType));
            }

            UpdateContentType(mediaType, () => new global::Umbraco.Core.Models.MediaType(-1), jetMediaType);

            ContentTypeService.Save(mediaType);

            // Update tracking. We get the media type once more to refresh it after updating it.
            SetPropertyTypeId(ContentTypeService.GetMediaType(mediaType.Alias), jetMediaType);
        }
    }
}