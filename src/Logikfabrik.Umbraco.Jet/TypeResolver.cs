// <copyright file="TypeResolver.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;

    /// <summary>
    /// The <see cref="TypeResolver" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class TypeResolver : ITypeResolver
    {
        private readonly Lazy<IEnumerable<DocumentType>> _documentTypes;
        private readonly Lazy<IEnumerable<MediaType>> _mediaTypes;
        private readonly Lazy<IEnumerable<MemberType>> _memberTypes;
        private readonly Lazy<IEnumerable<DataType>> _dataTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolver" /> class.
        /// </summary>
        /// <param name="typeService">The type service.</param>
        public TypeResolver(ITypeService typeService)
        {
            Ensure.That(typeService).IsNotNull();

            _documentTypes = new Lazy<IEnumerable<DocumentType>>(() => typeService.DocumentTypes.Select(type => new DocumentType(type)).ToArray());
            _mediaTypes = new Lazy<IEnumerable<MediaType>>(() => typeService.MediaTypes.Select(type => new MediaType(type)).ToArray());
            _memberTypes = new Lazy<IEnumerable<MemberType>>(() => typeService.MemberTypes.Select(type => new MemberType(type)).ToArray());
            _dataTypes = new Lazy<IEnumerable<DataType>>(() => typeService.DataTypes.Select(type => new DataType(type)).ToArray());
        }

        /// <inheritdoc />
        public IEnumerable<DocumentType> DocumentTypes => _documentTypes.Value;

        /// <inheritdoc />
        public IEnumerable<MediaType> MediaTypes => _mediaTypes.Value;

        /// <inheritdoc />
        public IEnumerable<MemberType> MemberTypes => _memberTypes.Value;

        /// <inheritdoc />
        public IEnumerable<DataType> DataTypes => _dataTypes.Value;
    }
}