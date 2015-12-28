// <copyright file="MemberTypeSynchronizationService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="MemberTypeSynchronizationService" /> class. Synchronizes types annotated using the <see cref="MemberTypeAttribute" />.
    /// </summary>
    public class MemberTypeSynchronizationService : BaseTypeSynchronizationService
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
        /// Synchronizes this instance.
        /// </summary>
        public override void Synchronize()
        {
            var jetMemberTypes = _typeService.MemberTypes.Select(t => new MemberType(t)).ToArray();

            // No member types; there's nothing to sync.
            if (!jetMemberTypes.Any())
            {
                return;
            }

            ValidateMemberTypeId(jetMemberTypes);
            ValidateMemberTypeAlias(jetMemberTypes);

            var memberTypes = _memberTypeService.GetAll().Cast<IContentTypeBase>().ToArray();

            foreach (var jetMemberType in jetMemberTypes)
            {
                Synchronize(memberTypes, jetMemberType);
            }
        }

        /// <summary>
        /// Validates the uJet member type identifiers.
        /// </summary>
        /// <param name="jetMemberTypes">The uJet member types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetMemberTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an identifier in <paramref name="jetMemberTypes" /> is conflicting.</exception>
        private static void ValidateMemberTypeId(IEnumerable<MemberType> jetMemberTypes)
        {
            if (jetMemberTypes == null)
            {
                throw new ArgumentNullException(nameof(jetMemberTypes));
            }

            var set = new HashSet<Guid>();

            foreach (var jetMemberType in jetMemberTypes)
            {
                if (!jetMemberType.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(jetMemberType.Id.Value))
                {
                    throw new InvalidOperationException($"ID conflict for member type {jetMemberType.Name}. ID {jetMemberType.Id.Value} is already in use.");
                }

                set.Add(jetMemberType.Id.Value);
            }
        }

        /// <summary>
        /// Validates the uJet member type aliases.
        /// </summary>
        /// <param name="jetMemberTypes">The uJet member types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetMemberTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an alias in <paramref name="jetMemberTypes" /> is conflicting.</exception>
        private static void ValidateMemberTypeAlias(IEnumerable<MemberType> jetMemberTypes)
        {
            if (jetMemberTypes == null)
            {
                throw new ArgumentNullException(nameof(jetMemberTypes));
            }

            var set = new HashSet<string>();

            foreach (var jetMemberType in jetMemberTypes)
            {
                if (set.Contains(jetMemberType.Alias))
                {
                    throw new InvalidOperationException(string.Format("Alias conflict for member type {0}. Alias {0} is already in use.", jetMemberType.Alias));
                }

                set.Add(jetMemberType.Alias);
            }
        }

        private void Synchronize(IContentTypeBase[] memberTypes, MemberType jetMemberType)
        {
            if (memberTypes == null)
            {
                throw new ArgumentNullException(nameof(memberTypes));
            }

            if (jetMemberType == null)
            {
                throw new ArgumentNullException(nameof(jetMemberType));
            }

            var memberType = GetBaseContentType(memberTypes, jetMemberType) as IMemberType;

            if (memberType == null)
            {
                memberType = CreateMemberType(jetMemberType);

                if (jetMemberType.Id.HasValue)
                {
                    ContentTypeRepository.SetContentTypeId(jetMemberType.Id.Value, memberType.Id);
                }
            }
            else
            {
                UpdateMemberType(memberType, jetMemberType);
            }
        }

        /// <summary>
        /// Creates a new member type using the uJet member type.
        /// </summary>
        /// <param name="jetMemberType">The uJet member type.</param>
        /// <returns>The created member type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="jetMemberType" /> is <c>null</c>.</exception>
        internal virtual IMemberType CreateMemberType(MemberType jetMemberType)
        {
            if (jetMemberType == null)
            {
                throw new ArgumentNullException(nameof(jetMemberType));
            }

            var memberType = (IMemberType)CreateBaseContentType(() => new global::Umbraco.Core.Models.MemberType(-1), jetMemberType);

            SynchronizePropertyTypes(memberType, jetMemberType.Properties);

            _memberTypeService.Save(memberType);

            // We get the member type once more to refresh it after updating it.
            memberType = _memberTypeService.Get(memberType.Alias);

            // Update tracking.
            SetPropertyTypeId(memberType, jetMemberType);

            return memberType;
        }

        /// <summary>
        /// Updates the member type to match the uJet member type.
        /// </summary>
        /// <param name="memberType">The member type to update.</param>
        /// <param name="jetMemberType">The uJet member type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberType" />, or <paramref name="jetMemberType" /> are <c>null</c>.</exception>
        internal virtual void UpdateMemberType(IMemberType memberType, MemberType jetMemberType)
        {
            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            if (jetMemberType == null)
            {
                throw new ArgumentNullException(nameof(jetMemberType));
            }

            UpdateBaseContentType(memberType, () => new global::Umbraco.Core.Models.ContentType(-1), jetMemberType);

            SynchronizePropertyTypes(memberType, jetMemberType.Properties);

            _memberTypeService.Save(memberType);

            // Update tracking. We get the member type once more to refresh it after updating it.
            SetPropertyTypeId(_memberTypeService.Get(memberType.Alias), jetMemberType);
        }
    }
}
