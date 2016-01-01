// <copyright file="ITypeResolver.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Collections.Generic;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="ITypeResolver" /> interface.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Gets the document type models.
        /// </summary>
        /// <value>
        /// The document type models.
        /// </value>
        IEnumerable<DocumentType> DocumentTypes { get; }

        /// <summary>
        /// Gets the media type models.
        /// </summary>
        /// <value>
        /// The media type models.
        /// </value>
        IEnumerable<MediaType> MediaTypes { get; }

        /// <summary>
        /// Gets the member type models.
        /// </summary>
        /// <value>
        /// The member type models.
        /// </value>
        IEnumerable<MemberType> MemberTypes { get; }

        /// <summary>
        /// Gets the data type models.
        /// </summary>
        /// <value>
        /// The data type models.
        /// </value>
        IEnumerable<DataType> DataTypes { get; }

        /// <summary>
        /// Resolves the document type model for the specified document type.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <returns>The document type model for the specified document type.</returns>
        DocumentType ResolveTypeModel(IContentType documentType);

        /// <summary>
        /// Resolves the media type model for the specified media type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <returns>The media type model for the specified media type.</returns>
        MediaType ResolveTypeModel(IMediaType mediaType);

        /// <summary>
        /// Resolves the member type model for the specified member type.
        /// </summary>
        /// <param name="memberType">The member type.</param>
        /// <returns>The member type model for the specified member type.</returns>
        MemberType ResolveTypeModel(IMemberType memberType);

        /// <summary>
        /// Resolves the data type model for the specified data type.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <returns>The data type model for the specified data type.</returns>
        DataType ResolveTypeModel(IDataTypeDefinition dataType);

        /// <summary>
        /// Resolves the document type for the specified document type model.
        /// </summary>
        /// <param name="documentTypeModel">The document type model.</param>
        /// <param name="documentTypes">The document types.</param>
        /// <returns>The document type for the specified document type model.</returns>
        IContentType ResolveType(DocumentType documentTypeModel, IContentType[] documentTypes);

        /// <summary>
        /// Resolves the media type for the specified media type model.
        /// </summary>
        /// <param name="mediaTypeModel">The media type model.</param>
        /// <param name="mediaTypes">The media types.</param>
        /// <returns>The media type for the specified media type model.</returns>
        IMediaType ResolveType(MediaType mediaTypeModel, IMediaType[] mediaTypes);

        /// <summary>
        /// Resolves the member type for the specified member type model.
        /// </summary>
        /// <param name="memberTypeModel">The member type model.</param>
        /// <param name="memberTypes">The member types.</param>
        /// <returns>The member type for the specified member type model.</returns>
        IMemberType ResolveType(MemberType memberTypeModel, IMemberType[] memberTypes);

        /// <summary>
        /// Resolves the data type for the specified data type model.
        /// </summary>
        /// <param name="dataTypeModel">The data type model.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <returns>The data type for the specified data type model.</returns>
        IDataTypeDefinition ResolveType(DataType dataTypeModel, IDataTypeDefinition[] dataTypes);

        /// <summary>
        /// Resolves the property type for the specified property type model.
        /// </summary>
        /// <param name="propertyTypeModel">The property type model.</param>
        /// <param name="propertyTypes">The property types.</param>
        /// <returns>The property type for the specified property type model.</returns>
        PropertyType ResolveType(TypeProperty propertyTypeModel, PropertyType[] propertyTypes);

        /// <summary>
        /// Resolves the content type for the specified content type model.
        /// </summary>
        /// <typeparam name="T1">The type type.</typeparam>
        /// <typeparam name="T2">The attribute type.</typeparam>
        /// <param name="contentTypeModel">The content type model.</param>
        /// <param name="contentTypes">The content types.</param>
        /// <returns>The content type for the specified content type model.</returns>
        IContentTypeBase ResolveType<T1, T2>(T1 contentTypeModel, IContentTypeBase[] contentTypes)
            where T1 : BaseType<T2>
            where T2 : BaseTypeAttribute;
    }
}