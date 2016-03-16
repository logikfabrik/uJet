// <copyright file="TypeResolver.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// The <see cref="TypeResolver" /> class.
    /// </summary>
    public class TypeResolver : ITypeResolver
    {
        private static ITypeResolver instance;

        private readonly Lazy<ReadOnlyCollection<DocumentType>> _documentTypes;
        private readonly Lazy<ReadOnlyCollection<MediaType>> _mediaTypes;
        private readonly Lazy<ReadOnlyCollection<MemberType>> _memberTypes;
        private readonly Lazy<ReadOnlyCollection<DataType>> _dataTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolver" /> class.
        /// </summary>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeService" /> is <c>null</c>.</exception>
        internal TypeResolver(ITypeService typeService)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            _documentTypes = new Lazy<ReadOnlyCollection<DocumentType>>(() => Array.AsReadOnly(typeService.DocumentTypes.Select(type => new DocumentType(type)).ToArray()));
            _mediaTypes = new Lazy<ReadOnlyCollection<MediaType>>(() => Array.AsReadOnly(typeService.MediaTypes.Select(type => new MediaType(type)).ToArray()));
            _memberTypes = new Lazy<ReadOnlyCollection<MemberType>>(() => Array.AsReadOnly(typeService.MemberTypes.Select(type => new MemberType(type)).ToArray()));
            _dataTypes = new Lazy<ReadOnlyCollection<DataType>>(() => Array.AsReadOnly(typeService.DataTypes.Select(type => new DataType(type)).ToArray()));
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TypeResolver" /> class from being created.
        /// </summary>
        private TypeResolver()
            : this(TypeService.Instance)
        {
        }

        /// <summary>
        /// Gets a singleton instance of the <see cref="TypeResolver" />.
        /// </summary>
        public static ITypeResolver Instance => instance ?? (instance = new TypeResolver());

        /// <summary>
        /// Gets the document type models.
        /// </summary>
        /// <value>
        /// The document type models.
        /// </value>
        public ReadOnlyCollection<DocumentType> DocumentTypes => _documentTypes.Value;

        /// <summary>
        /// Gets the media type models.
        /// </summary>
        /// <value>
        /// The media type models.
        /// </value>
        public ReadOnlyCollection<MediaType> MediaTypes => _mediaTypes.Value;

        /// <summary>
        /// Gets the member type models.
        /// </summary>
        /// <value>
        /// The member type models.
        /// </value>
        public ReadOnlyCollection<MemberType> MemberTypes => _memberTypes.Value;

        /// <summary>
        /// Gets the data type models.
        /// </summary>
        /// <value>
        /// The data type models.
        /// </value>
        public ReadOnlyCollection<DataType> DataTypes => _dataTypes.Value;
    }
}