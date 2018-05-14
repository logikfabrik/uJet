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
    using Logging;
    using Mappings;

    /// <summary>
    /// The <see cref="MemberTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="MemberTypeAttribute" />.
    /// </summary>
    public class MemberTypeSynchronizer : ContentTypeModelSynchronizer<MemberType, MemberTypeAttribute, IMemberType>
    {
        private readonly IMemberTypeService _memberTypeService;
        private readonly Lazy<MemberType[]> _memberTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="memberTypeService">The member type service.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <param name="dataTypeDefinitionService">The data type definition service.</param>
        public MemberTypeSynchronizer(
            ILogService logService,
            IMemberTypeService memberTypeService,
            IModelService modelService,
            ITypeRepository typeRepository,
            IDataTypeDefinitionService dataTypeDefinitionService)
            : base(logService, typeRepository, dataTypeDefinitionService)
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