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
            var memberTypes = _typeService.MemberTypes.Select(t => new MemberType(t)).ToArray();

            // No member types; there's nothing to sync.
            if (!memberTypes.Any())
            {
                return;
            }

            ValidateMemberTypeId(memberTypes);
            ValidateMemberTypeAlias(memberTypes);

            // WARNING: This might cause issues; the array of types only contains the initial types, not including ones added/updated during sync.
            var types = _memberTypeService.GetAll().ToArray();

            foreach (var memberType in memberTypes.Where(dt => dt.Id.HasValue))
            {
                SynchronizeById(types, memberType);
            }

            foreach (var memberType in memberTypes.Where(dt => !dt.Id.HasValue))
            {
                SynchronizeByAlias(types, memberType);
            }
        }

        /// <summary>
        /// Synchronizes member type by alias.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="memberType">The member type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, or <paramref name="memberType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the member type identifier is not <c>null</c>.</exception>
        internal virtual void SynchronizeByAlias(IEnumerable<IMemberType> contentTypes, MemberType memberType)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            if (memberType.Id.HasValue)
            {
                throw new ArgumentException("Member type ID must be null.", nameof(memberType));
            }

            var ct = contentTypes.FirstOrDefault(type => type.Alias == memberType.Alias);

            if (ct == null)
            {
                CreateMemberType(memberType);
            }
            else
            {
                UpdateMemberType(ct, memberType);
            }
        }

        /// <summary>
        /// Updates the member type.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="memberType">The member type to update.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentType" />, or <paramref name="memberType" /> are <c>null</c>.</exception>
        internal virtual void UpdateMemberType(IMemberType contentType, MemberType memberType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            UpdateBaseType(contentType, () => new ContentType(-1), memberType);

            SynchronizePropertyTypes(contentType, memberType.Properties);

            _memberTypeService.Save(contentType);

            // Update tracking.
            SetPropertyTypeId(_memberTypeService.Get(contentType.Alias), memberType);
        }

        /// <summary>
        /// Synchronizes member type by identifier.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <param name="memberType">The member type.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contentTypes" />, or <paramref name="memberType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the member type identifier is <c>null</c>.</exception>
        internal virtual void SynchronizeById(IEnumerable<IMemberType> contentTypes, MemberType memberType)
        {
            if (contentTypes == null)
            {
                throw new ArgumentNullException(nameof(contentTypes));
            }

            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            if (!memberType.Id.HasValue)
            {
                throw new ArgumentException("Member type ID cannot be null.", nameof(memberType));
            }

            IMemberType ct = null;

            var id = ContentTypeRepository.GetContentTypeId(memberType.Id.Value);

            if (id.HasValue)
            {
                // The member type has been synchronized before. Get the matching content type.
                // It might have been removed using the back office.
                ct = contentTypes.FirstOrDefault(type => type.Id == id.Value);
            }

            if (ct == null)
            {
                CreateMemberType(memberType);

                // Get the created member type.
                ct = _memberTypeService.Get(memberType.Alias);

                // Connect the member type and the created content type.
                ContentTypeRepository.SetContentTypeId(memberType.Id.Value, ct.Id);
            }
            else
            {
                UpdateMemberType(ct, memberType);
            }
        }

        /// <summary>
        /// Validates the member type identifier.
        /// </summary>
        /// <param name="memberTypes">The member types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an identifier in <paramref name="memberTypes" /> is conflicting.</exception>
        private static void ValidateMemberTypeId(IEnumerable<MemberType> memberTypes)
        {
            if (memberTypes == null)
            {
                throw new ArgumentNullException(nameof(memberTypes));
            }

            var set = new HashSet<Guid>();

            foreach (var memberType in memberTypes)
            {
                if (!memberType.Id.HasValue)
                {
                    continue;
                }

                if (set.Contains(memberType.Id.Value))
                {
                    throw new InvalidOperationException(
                        $"ID conflict for member type {memberType.Name}. ID {memberType.Id.Value} is already in use.");
                }

                set.Add(memberType.Id.Value);
            }
        }

        /// <summary>
        /// Validates the member type alias.
        /// </summary>
        /// <param name="memberTypes">The document types.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberTypes" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if an alias in <paramref name="memberTypes" /> is conflicting.</exception>
        private static void ValidateMemberTypeAlias(IEnumerable<MemberType> memberTypes)
        {
            if (memberTypes == null)
            {
                throw new ArgumentNullException(nameof(memberTypes));
            }

            var set = new HashSet<string>();

            foreach (var memberType in memberTypes)
            {
                if (set.Contains(memberType.Alias))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Alias conflict for member type {0}. Alias {0} is already in use.",
                            memberType.Alias));
                }

                set.Add(memberType.Alias);
            }
        }

        /// <summary>
        /// Creates the member type.
        /// </summary>
        /// <param name="memberType">The member type to create.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberType" /> is <c>null</c>.</exception>
        private void CreateMemberType(MemberType memberType)
        {
            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            var t = (IMemberType)CreateBaseType(() => new global::Umbraco.Core.Models.MemberType(-1), memberType);

            SynchronizePropertyTypes(t, memberType.Properties);

            _memberTypeService.Save(t);

            // Update tracking.
            SetPropertyTypeId(_memberTypeService.Get(t.Alias), memberType);
        }
    }
}
