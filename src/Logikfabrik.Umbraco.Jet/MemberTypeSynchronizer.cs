// <copyright file="MemberTypeSynchronizer.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="MemberTypeSynchronizer" /> class. Synchronizes model types annotated using the <see cref="MemberTypeAttribute" />.
    /// </summary>
    public class MemberTypeSynchronizer : ContentTypeSynchronizer<MemberType, MemberTypeAttribute, IMemberType>
    {
        private readonly IMemberTypeService _memberTypeService;
        private readonly Lazy<MemberType[]> _memberTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSynchronizer" /> class.
        /// </summary>
        /// <param name="memberTypeService">The member type service.</param>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="typeRepository">The type repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberTypeService" />, or <paramref name="typeResolver" /> are <c>null</c>.</exception>
        public MemberTypeSynchronizer(
            IMemberTypeService memberTypeService,
            ITypeResolver typeResolver,
            ITypeRepository typeRepository)
            : base(typeRepository)
        {
            if (memberTypeService == null)
            {
                throw new ArgumentNullException(nameof(memberTypeService));
            }

            if (typeResolver == null)
            {
                throw new ArgumentNullException(nameof(typeResolver));
            }

            _memberTypeService = memberTypeService;
            _memberTypes = new Lazy<MemberType[]>(() => typeResolver.MemberTypes.ToArray());
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>The models.</value>
        protected override MemberType[] Models => _memberTypes.Value;

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
        /// Creates a content type.
        /// </summary>
        /// <returns>
        /// The created content type.
        /// </returns>
        protected override IMemberType CreateContentType()
        {
            return new global::Umbraco.Core.Models.MemberType(-1);
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