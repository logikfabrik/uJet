// <copyright file="MemberTypeSynchronizer.cs" company="Logikfabrik">
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
    /// The <see cref="MemberTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="MemberTypeAttribute" />.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class MemberTypeSynchronizer : ContentTypeModelSynchronizer<MemberType, MemberTypeAttribute, IMemberType>
    {
        private readonly IMemberTypeService _memberTypeService;
        private readonly Lazy<MemberType[]> _memberTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="memberTypeService">The member type service.</param>
        /// <param name="contentTypeFinder">The content type finder.</param>
        /// <param name="propertyTypeFinder">The property type finder.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="dataTypeDefinitionService">The data type definition service.</param>
        /// <param name="typeRepository">The type repository.</param>
        // ReSharper disable once InheritdocConsiderUsage
        // ReSharper disable once UnusedMember.Global
        public MemberTypeSynchronizer(
            IMemberTypeService memberTypeService,
            IContentTypeFinder<MemberType, MemberTypeAttribute, IMemberType> contentTypeFinder,
            IPropertyTypeFinder propertyTypeFinder,
            IModelService modelService,
            IDataTypeDefinitionService dataTypeDefinitionService,
            ITypeRepository typeRepository)
            : base(contentTypeFinder, propertyTypeFinder, dataTypeDefinitionService, typeRepository)
        {
            Ensure.That(memberTypeService).IsNotNull();
            Ensure.That(modelService).IsNotNull();

            _memberTypeService = memberTypeService;
            _memberTypes = new Lazy<MemberType[]>(() => modelService.MemberTypes.ToArray());
        }

        /// <inheritdoc />
        protected override MemberType[] Models => _memberTypes.Value;

        /// <inheritdoc />
        protected override IMemberType[] GetContentTypes()
        {
            return _memberTypeService.GetAll().ToArray();
        }

        /// <inheritdoc />
        protected override IMemberType CreateContentType()
        {
            return new global::Umbraco.Core.Models.MemberType(-1);
        }

        /// <inheritdoc />
        protected override void SaveContentType(IMemberType contentType)
        {
            _memberTypeService.Save(contentType);
        }
    }
}