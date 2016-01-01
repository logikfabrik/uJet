// <copyright file="TypeResolver.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="TypeResolver" /> class.
    /// </summary>
    public class TypeResolver : ITypeResolver
    {
        private static ITypeResolver instance;

        private readonly ITypeService _typeService;
        private readonly ITypeRepository _typeRepository;

        private readonly Lazy<IEnumerable<DocumentType>> _documentTypes;
        private readonly Lazy<IEnumerable<MediaType>> _mediaTypes;
        private readonly Lazy<IEnumerable<MemberType>> _memberTypes;
        private readonly Lazy<IEnumerable<DataType>> _dataTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolver" /> class.
        /// </summary>
        /// <param name="typeService">The type service.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeService" />, or <paramref name="typeRepository" /> are <c>null</c>.</exception>
        internal TypeResolver(
            ITypeService typeService,
            ITypeRepository typeRepository)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            if (typeRepository == null)
            {
                throw new ArgumentNullException(nameof(typeRepository));
            }

            _typeService = typeService;
            _typeRepository = typeRepository;

            _documentTypes = new Lazy<IEnumerable<DocumentType>>(() => _typeService.DocumentTypes.Select(type => new DocumentType(type)));
            _mediaTypes = new Lazy<IEnumerable<MediaType>>(() => _typeService.MediaTypes.Select(type => new MediaType(type)));
            _memberTypes = new Lazy<IEnumerable<MemberType>>(() => _typeService.MemberTypes.Select(type => new MemberType(type)));
            _dataTypes = new Lazy<IEnumerable<DataType>>(() => _typeService.DataTypes.Select(type => new DataType(type)));
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TypeResolver" /> class from being created.
        /// </summary>
        private TypeResolver()
            : this(TypeService.Instance, TypeRepository.Instance)
        {
        }

        /// <summary>
        /// Gets a singleton instance of the type resolver.
        /// </summary>
        public static ITypeResolver Instance => instance ?? (instance = new TypeResolver());

        /// <summary>
        /// Gets the document type models.
        /// </summary>
        /// <value>
        /// The document type models.
        /// </value>
        public IEnumerable<DocumentType> DocumentTypes => _documentTypes.Value;

        /// <summary>
        /// Gets the media type models.
        /// </summary>
        /// <value>
        /// The media type models.
        /// </value>
        public IEnumerable<MediaType> MediaTypes => _mediaTypes.Value;

        /// <summary>
        /// Gets the member type models.
        /// </summary>
        /// <value>
        /// The member type models.
        /// </value>
        public IEnumerable<MemberType> MemberTypes => _memberTypes.Value;

        /// <summary>
        /// Gets the data type models.
        /// </summary>
        /// <value>
        /// The data type models.
        /// </value>
        public IEnumerable<DataType> DataTypes => _dataTypes.Value;

        /// <summary>
        /// Resolves the document type model for the specified document type.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <returns>The document type model for the specified document type.</returns>
        public DocumentType ResolveTypeModel(IContentType documentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            return ResolveContentTypeModel(documentType, DocumentTypes.ToArray());
        }

        /// <summary>
        /// Resolves the media type model for the specified media type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <returns>The media type model for the specified media type.</returns>
        public MediaType ResolveTypeModel(IMediaType mediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            return ResolveContentTypeModel(mediaType, MediaTypes.ToArray());
        }

        /// <summary>
        /// Resolves the member type model for the specified member type.
        /// </summary>
        /// <param name="memberType">The member type.</param>
        /// <returns>The member type model for the specified member type.</returns>
        public MemberType ResolveTypeModel(IMemberType memberType)
        {
            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            return ResolveContentTypeModel(memberType, MemberTypes.ToArray());
        }

        /// <summary>
        /// Resolves the data type model for the specified data type.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <returns>The data type model for the specified data type.</returns>
        public DataType ResolveTypeModel(IDataTypeDefinition dataType)
        {
            if (dataType == null)
            {
                throw new ArgumentNullException(nameof(dataType));
            }

            var id = _typeRepository.GetDefinitionModelId(dataType.Id);

            DataType typeModel;

            if (id.HasValue)
            {
                typeModel = DataTypes.SingleOrDefault(dt => dt.Id == id.Value);

                if (typeModel != null)
                {
                    return typeModel;
                }
            }

            typeModel = DataTypes.SingleOrDefault(dt => dt.Name == dataType.Name);

            return typeModel;
        }

        /// <summary>
        /// Resolves the document type for the specified document type model.
        /// </summary>
        /// <param name="documentTypeModel">The document type model.</param>
        /// <param name="documentTypes">The document types.</param>
        /// <returns>The document type for the specified document type model.</returns>
        public IContentType ResolveType(DocumentType documentTypeModel, IContentType[] documentTypes)
        {
            if (documentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(documentTypeModel));
            }

            if (documentTypes == null)
            {
                throw new ArgumentNullException(nameof(documentTypes));
            }

            return ResolveContentType(documentTypeModel, documentTypes);
        }

        /// <summary>
        /// Resolves the media type for the specified media type model.
        /// </summary>
        /// <param name="mediaTypeModel">The media type model.</param>
        /// <param name="mediaTypes">The media types.</param>
        /// <returns>The media type for the specified media type model.</returns>
        public IMediaType ResolveType(MediaType mediaTypeModel, IMediaType[] mediaTypes)
        {
            if (mediaTypeModel == null)
            {
                throw new ArgumentNullException(nameof(mediaTypeModel));
            }

            if (mediaTypes == null)
            {
                throw new ArgumentNullException(nameof(mediaTypes));
            }

            return ResolveContentType(mediaTypeModel, mediaTypes);
        }

        /// <summary>
        /// Resolves the member type for the specified member type model.
        /// </summary>
        /// <param name="memberTypeModel">The member type model.</param>
        /// <param name="memberTypes">The member types.</param>
        /// <returns>The member type for the specified member type model.</returns>
        public IMemberType ResolveType(MemberType memberTypeModel, IMemberType[] memberTypes)
        {
            if (memberTypeModel == null)
            {
                throw new ArgumentNullException(nameof(memberTypeModel));
            }

            if (memberTypes == null)
            {
                throw new ArgumentNullException(nameof(memberTypes));
            }

            return ResolveContentType(memberTypeModel, memberTypes);
        }

        /// <summary>
        /// Resolves the data type for the specified data type model.
        /// </summary>
        /// <param name="dataTypeModel">The data type model.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <returns>The data type for the specified data type model.</returns>
        public IDataTypeDefinition ResolveType(DataType dataTypeModel, IDataTypeDefinition[] dataTypes)
        {
            if (dataTypeModel == null)
            {
                throw new ArgumentNullException(nameof(dataTypeModel));
            }

            if (dataTypes == null)
            {
                throw new ArgumentNullException(nameof(dataTypes));
            }

            IDataTypeDefinition dataType;

            // Step 1; we try to find a match using the type ID (if any).
            if (dataTypeModel.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _typeRepository.GetDefinitionId(dataTypeModel.Id.Value);

                if (umbracoId.HasValue)
                {
                    // There was an Umbraco ID matching the type ID in the uJet tables. We try to match that ID with an existing data type.
                    dataType = dataTypes.SingleOrDefault(ct => ct.Id == umbracoId.Value);

                    if (dataType != null)
                    {
                        // We've found a match. The type matches an existing Umbraco data type.
                        return dataType;
                    }
                }
            }

            // Step 2; we try to find a match using the type name.
            dataType = dataTypes.SingleOrDefault(ct => ct.Name == dataTypeModel.Name);

            // We might have found a match.
            return dataType;
        }

        /// <summary>
        /// Resolves the property type for the specified property type model.
        /// </summary>
        /// <param name="propertyTypeModel">The property type model.</param>
        /// <param name="propertyTypes">The property types.</param>
        /// <returns>The property type for the specified property type model.</returns>
        public global::Umbraco.Core.Models.PropertyType ResolveType(TypeProperty propertyTypeModel, global::Umbraco.Core.Models.PropertyType[] propertyTypes)
        {
            if (propertyTypeModel == null)
            {
                throw new ArgumentNullException(nameof(propertyTypeModel));
            }

            if (propertyTypes == null)
            {
                throw new ArgumentNullException(nameof(propertyTypes));
            }

            global::Umbraco.Core.Models.PropertyType propertyType;

            // Step 1; we try to find a match using the type ID (if any).
            if (propertyTypeModel.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _typeRepository.GetPropertyTypeId(propertyTypeModel.Id.Value);

                if (umbracoId.HasValue)
                {
                    // There was an Umbraco ID matching the type ID in the uJet tables. We try to match that ID with an existing property type.
                    propertyType = propertyTypes.SingleOrDefault(pt => pt.Id == umbracoId.Value);

                    if (propertyType != null)
                    {
                        // We've found a match. The type matches an existing Umbraco property type.
                        return propertyType;
                    }
                }
            }

            // Step 2; we try to find a match using the type alias.
            propertyType = propertyTypes.SingleOrDefault(pt => pt.Alias == propertyTypeModel.Alias);

            // We might have found a match.
            return propertyType;
        }

        /// <summary>
        /// Resolves the content type for the specified content type model.
        /// </summary>
        /// <typeparam name="T1">The type type.</typeparam>
        /// <typeparam name="T2">The attribute type.</typeparam>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <param name="contentTypes">The content types.</param>
        /// <returns>The content type for the specified content type model.</returns>
        public IContentTypeBase ResolveType<T1, T2>(T1 contentTypeModel, IContentTypeBase[] contentTypes)
            where T1 : BaseType<T2>
            where T2 : BaseTypeAttribute
        {
            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            return ResolveContentType(contentTypeModel, contentTypes);
        }

        private T ResolveContentTypeModel<T>(IContentTypeBase contentType, T[] contentTypeModels)
            where T : ITypeModel
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentTypeModels == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModels));
            }

            var id = _typeRepository.GetContentTypeModelId(contentType.Id);

            T typeModel;

            if (id.HasValue)
            {
                typeModel = contentTypeModels.SingleOrDefault(dt => dt.Id == id.Value);

                if (typeModel != null)
                {
                    return typeModel;
                }
            }

            typeModel = contentTypeModels.SingleOrDefault(dt => dt.Alias == contentType.Alias);

            return typeModel;
        }

        private T ResolveContentType<T>(ITypeModel contentTypeModel, T[] contentTypes)
            where T : IContentTypeBase
        {
            if (contentTypeModel == null)
            {
                throw new ArgumentNullException(nameof(contentTypeModel));
            }

            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            T contentType;

            // Step 1; we try to find a match using the type ID (if any).
            if (contentTypeModel.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _typeRepository.GetContentTypeId(contentTypeModel.Id.Value);

                if (umbracoId.HasValue)
                {
                    // There was an Umbraco ID matching the type ID in the uJet tables. We try to match that ID with an existing content type.
                    contentType = contentTypes.SingleOrDefault(ct => ct.Id == umbracoId.Value);

                    if (contentType != null)
                    {
                        // We've found a match. The type matches an existing Umbraco content type.
                        return contentType;
                    }
                }
            }

            // Step 2; we try to find a match using the type alias.
            contentType = contentTypes.SingleOrDefault(ct => ct.Alias == contentTypeModel.Alias);

            // We might have found a match.
            return contentType;
        }
    }
}