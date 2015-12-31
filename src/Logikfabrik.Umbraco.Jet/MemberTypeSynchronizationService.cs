// <copyright file="MemberTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="MemberTypeSynchronizationService" /> class. Synchronizes model types annotated using the <see cref="MemberTypeAttribute" />.
    /// </summary>
    public class MemberTypeSynchronizationService : BaseTypeSynchronizationService<MemberType, MemberTypeAttribute>
    {
        private readonly IMemberTypeService _memberTypeService;
        private readonly ITypeService _typeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSynchronizationService" /> class.
        /// </summary>
        public MemberTypeSynchronizationService()
            : this(
                ApplicationContext.Current.Services.MemberTypeService,
                new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax)),
                TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTypeSynchronizationService"/> class.
        /// </summary>
        /// <param name="memberTypeService">The member type service.</param>
        /// <param name="contentTypeRepository">The content type repository.</param>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberTypeService" />, or <paramref name="typeService" /> are <c>null</c>.</exception>
        public MemberTypeSynchronizationService(
            IMemberTypeService memberTypeService,
            IContentTypeRepository contentTypeRepository,
            ITypeService typeService)
            : base(contentTypeRepository)
        {
            if (memberTypeService == null)
            {
                throw new ArgumentNullException(nameof(memberTypeService));
            }

            if (typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }

            _memberTypeService = memberTypeService;
            _typeService = typeService;
        }

        /// <summary>
        /// Gets the content type models.
        /// </summary>
        /// <value>
        /// The content type models.
        /// </value>
        protected override MemberType[] ContentTypeModels
        {
            get { return _typeService.MemberTypes.Select(t => new MemberType(t)).ToArray(); }
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <returns>
        /// The content types.
        /// </returns>
        protected override IContentTypeBase[] GetContentTypes()
        {
            return _memberTypeService.GetAll().Cast<IContentTypeBase>().ToArray();
        }

        /// <summary>
        /// Gets a content type.
        /// </summary>
        /// <returns>
        /// A content type.
        /// </returns>
        protected override IContentTypeBase GetContentType()
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
        protected override IContentTypeBase GetContentType(string alias)
        {
            return _memberTypeService.Get(alias);
        }

        /// <summary>
        /// Saves the specified content type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected override void SaveContentType(IContentTypeBase contentType)
        {
            _memberTypeService.Save((IMemberType)contentType);
        }
    }
}
