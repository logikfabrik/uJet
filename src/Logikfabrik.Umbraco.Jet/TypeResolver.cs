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
        /// Gets a singleton instance of the <see cref="TypeResolver" />.
        /// </summary>
        public static ITypeResolver Instance => instance ?? (instance = new TypeResolver());

        /// <summary>
        /// Gets the document type type models.
        /// </summary>
        /// <value>
        /// The document type type models.
        /// </value>
        public IEnumerable<DocumentType> DocumentTypes => _documentTypes.Value;

        /// <summary>
        /// Gets the media type type models.
        /// </summary>
        /// <value>
        /// The media type type models.
        /// </value>
        public IEnumerable<MediaType> MediaTypes => _mediaTypes.Value;

        /// <summary>
        /// Gets the member type type models.
        /// </summary>
        /// <value>
        /// The member type type models.
        /// </value>
        public IEnumerable<MemberType> MemberTypes => _memberTypes.Value;

        /// <summary>
        /// Gets the data type type models.
        /// </summary>
        /// <value>
        /// The data type type models.
        /// </value>
        public IEnumerable<DataType> DataTypes => _dataTypes.Value;

        /// <summary>
        /// Resolves the type model for the specified document type.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <returns>The type model.</returns>
        public DocumentType ResolveTypeModel(IContentType documentType)
        {
            if (documentType == null)
            {
                throw new ArgumentNullException(nameof(documentType));
            }

            return ResolveContentTypeModel<DocumentType, DocumentTypeAttribute>(documentType, DocumentTypes.ToArray());
        }

        /// <summary>
        /// Resolves type model for the specified media type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <returns>The type model.</returns>
        public MediaType ResolveTypeModel(IMediaType mediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException(nameof(mediaType));
            }

            return ResolveContentTypeModel<MediaType, MediaTypeAttribute>(mediaType, MediaTypes.ToArray());
        }

        /// <summary>
        /// Resolves the type model for the specified member type.
        /// </summary>
        /// <param name="memberType">The member type.</param>
        /// <returns>The type model.</returns>
        public MemberType ResolveTypeModel(IMemberType memberType)
        {
            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            return ResolveContentTypeModel<MemberType, MemberTypeAttribute>(memberType, MemberTypes.ToArray());
        }

        /// <summary>
        /// Resolves the type model for the specified data type.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <returns>The type model.</returns>
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
        /// Resolves the document type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="documentTypes">The document types.</param>
        /// <returns>The document type.</returns>
        public IContentType ResolveType(DocumentType model, IContentType[] documentTypes)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (documentTypes == null)
            {
                throw new ArgumentNullException(nameof(documentTypes));
            }

            return ResolveType<DocumentType, DocumentTypeAttribute, IContentType>(model, documentTypes);
        }

        /// <summary>
        /// Resolves the media type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="mediaTypes">The media types.</param>
        /// <returns>The media type.</returns>
        public IMediaType ResolveType(MediaType model, IMediaType[] mediaTypes)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (mediaTypes == null)
            {
                throw new ArgumentNullException(nameof(mediaTypes));
            }

            return ResolveType<MediaType, MediaTypeAttribute, IMediaType>(model, mediaTypes);
        }

        /// <summary>
        /// Resolves the member type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="memberTypes">The member types.</param>
        /// <returns>The member type.</returns>
        public IMemberType ResolveType(MemberType model, IMemberType[] memberTypes)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (memberTypes == null)
            {
                throw new ArgumentNullException(nameof(memberTypes));
            }

            return ResolveType<MemberType, MemberTypeAttribute, IMemberType>(model, memberTypes);
        }

        /// <summary>
        /// Resolves the data type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <returns>The data type.</returns>
        public IDataTypeDefinition ResolveType(DataType model, IDataTypeDefinition[] dataTypes)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (dataTypes == null)
            {
                throw new ArgumentNullException(nameof(dataTypes));
            }

            IDataTypeDefinition dataType;

            // Step 1; we try to find a match using the type ID (if any).
            if (model.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _typeRepository.GetDefinitionId(model.Id.Value);

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
            dataType = dataTypes.SingleOrDefault(ct => ct.Name == model.Name);

            // We might have found a match.
            return dataType;
        }

        /// <summary>
        /// Resolves the property type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyTypes">The property types.</param>
        /// <returns>The property type.</returns>
        public global::Umbraco.Core.Models.PropertyType ResolveType(TypeProperty model, global::Umbraco.Core.Models.PropertyType[] propertyTypes)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (propertyTypes == null)
            {
                throw new ArgumentNullException(nameof(propertyTypes));
            }

            global::Umbraco.Core.Models.PropertyType propertyType;

            // Step 1; we try to find a match using the type ID (if any).
            if (model.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _typeRepository.GetPropertyTypeId(model.Id.Value);

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
            propertyType = propertyTypes.SingleOrDefault(pt => pt.Alias == model.Alias);

            // We might have found a match.
            return propertyType;
        }

        /// <summary>
        /// Resolves the content type for the specified type model.
        /// </summary>
        /// <typeparam name="T1">The <see cref="ContentTypeModel{T}" /> type.</typeparam>
        /// <typeparam name="T2">The <see cref="ContentTypeModelAttribute" /> type.</typeparam>
        /// <typeparam name="T3">The <see cref="IContentTypeBase" /> type.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="contentTypes">The content types.</param>
        /// <returns>The content type.</returns>
        public T3 ResolveType<T1, T2, T3>(T1 model, T3[] contentTypes)
            where T1 : ContentTypeModel<T2>
            where T2 : ContentTypeModelAttribute
            where T3 : IContentTypeBase
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            T3 contentType;

            // Step 1; we try to find a match using the type ID (if any).
            if (model.Id.HasValue)
            {
                // The type has an ID. We try to find the Umbraco ID using that ID.
                var umbracoId = _typeRepository.GetContentTypeId(model.Id.Value);

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
            contentType = contentTypes.SingleOrDefault(ct => ct.Alias == model.Alias);

            // We might have found a match.
            return contentType;
        }

        private T1 ResolveContentTypeModel<T1, T2>(IContentTypeBase contentType, T1[] models)
            where T1 : ContentTypeModel<T2>
            where T2 : ContentTypeModelAttribute
        {
            var id = _typeRepository.GetContentTypeModelId(contentType.Id);

            T1 typeModel;

            if (id.HasValue)
            {
                typeModel = models.SingleOrDefault(m => m.Id == id.Value);

                if (typeModel != null)
                {
                    return typeModel;
                }
            }

            typeModel = models.SingleOrDefault(m => m.Alias.Equals(contentType.Alias, StringComparison.InvariantCultureIgnoreCase));

            return typeModel;
        }
    }
}