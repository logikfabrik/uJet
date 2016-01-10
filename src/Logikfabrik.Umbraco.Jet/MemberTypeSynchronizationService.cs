// <copyright file="MemberTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="MemberTypeSynchronizationService" /> class. Synchronizes model types annotated using the <see cref="MemberTypeAttribute" />.
    /// </summary>
    public class MemberTypeSynchronizationService : ContentTypeModelSynchronizationService<MemberType, MemberTypeAttribute, IMemberType>
    {
        private readonly IMemberTypeService _memberTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSynchronizationService" /> class.
        /// </summary>
        public MemberTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.MemberTypeService,
                TypeResolver.Instance,
                TypeRepository.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSynchronizationService" /> class.
        /// </summary>
        /// <param name="memberTypeService">The member type service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberTypeService" /> is <c>null</c>.</exception>
        public MemberTypeSynchronizationService(
            IMemberTypeService memberTypeService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository)
            : base(typeResolver, typeRepository)
        {
            if (memberTypeService == null)
            {
                throw new ArgumentNullException(nameof(memberTypeService));
            }

            _memberTypeService = memberTypeService;
        }

        /// <summary>
        /// Gets the content type models.
        /// </summary>
        /// <value>
        /// The content type models.
        /// </value>
        protected override MemberType[] Models => Resolver.MemberTypes.ToArray();

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>
        /// The content types.
        /// </returns>
        protected override IMemberType[] GetContentTypes()
        {
            return _memberTypeService.GetAll().ToArray();
        }

        /// <summary>
        /// Gets a content type.
        /// </summary>
        /// <returns>
        /// A content type.
        /// </returns>
        protected override IMemberType GetContentType()
        {
            return new global::Umbraco.Core.Models.MemberType(-1);
        }

        /// <summary>
        /// Gets the content type with the specified alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>
        /// The content type with the specified alias.
        /// </returns>
        protected override IMemberType GetContentType(string alias)
        {
            return _memberTypeService.Get(alias);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected override void SaveContentType(IMemberType contentType)
        {
            _memberTypeService.Save(contentType);
        }
    }
}