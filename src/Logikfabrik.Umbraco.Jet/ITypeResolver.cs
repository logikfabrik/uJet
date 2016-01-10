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
        /// Gets the document type type models.
        /// </summary>
        /// <value>
        /// The document type type models.
        /// </value>
        IEnumerable<DocumentType> DocumentTypes { get; }

        /// <summary>
        /// Gets the media type type models.
        /// </summary>
        /// <value>
        /// The media type type models.
        /// </value>
        IEnumerable<MediaType> MediaTypes { get; }

        /// <summary>
        /// Gets the member type type models.
        /// </summary>
        /// <value>
        /// The member type type models.
        /// </value>
        IEnumerable<MemberType> MemberTypes { get; }

        /// <summary>
        /// Gets the data type type models.
        /// </summary>
        /// <value>
        /// The data type type models.
        /// </value>
        IEnumerable<DataType> DataTypes { get; }

        /// <summary>
        /// Resolves the type model for the specified document type.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <returns>The type model.</returns>
        DocumentType ResolveTypeModel(IContentType documentType);

        /// <summary>
        /// Resolves type model for the specified media type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <returns>The type model.</returns>
        MediaType ResolveTypeModel(IMediaType mediaType);

        /// <summary>
        /// Resolves the type model for the specified member type.
        /// </summary>
        /// <param name="memberType">The member type.</param>
        /// <returns>The type model.</returns>
        MemberType ResolveTypeModel(IMemberType memberType);

        /// <summary>
        /// Resolves the type model for the specified data type.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <returns>The type model.</returns>
        DataType ResolveTypeModel(IDataTypeDefinition dataType);

        /// <summary>
        /// Resolves the document type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="documentTypes">The document types.</param>
        /// <returns>The document type.</returns>
        IContentType ResolveType(DocumentType model, IContentType[] documentTypes);

        /// <summary>
        /// Resolves the media type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="mediaTypes">The media types.</param>
        /// <returns>The media type.</returns>
        IMediaType ResolveType(MediaType model, IMediaType[] mediaTypes);

        /// <summary>
        /// Resolves the member type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="memberTypes">The member types.</param>
        /// <returns>The member type.</returns>
        IMemberType ResolveType(MemberType model, IMemberType[] memberTypes);

        /// <summary>
        /// Resolves the data type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <returns>The data type.</returns>
        IDataTypeDefinition ResolveType(DataType model, IDataTypeDefinition[] dataTypes);

        /// <summary>
        /// Resolves the property type for the specified type model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyTypes">The property types.</param>
        /// <returns>The property type.</returns>
        PropertyType ResolveType(TypeProperty model, PropertyType[] propertyTypes);

        /// <summary>
        /// Resolves the content type for the specified type model.
        /// </summary>
        /// <typeparam name="T1">The <see cref="ContentTypeModel{T}" /> type.</typeparam>
        /// <typeparam name="T2">The <see cref="ContentTypeModelAttribute" /> type.</typeparam>
        /// <typeparam name="T3">The content type.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="contentTypes">The content types.</param>
        /// <returns>The content type.</returns>
        T3 ResolveType<T1, T2, T3>(T1 model, T3[] contentTypes)
            where T1 : ContentTypeModel<T2>
            where T2 : ContentTypeModelAttribute
            where T3 : IContentTypeBase;
    }
}