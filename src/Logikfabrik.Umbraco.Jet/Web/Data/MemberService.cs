// <copyright file="MemberService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using EnsureThat;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="MemberService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to member models.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class MemberService : ContentService
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberService" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public MemberService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        public MemberService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            EnsureArg.IsNotNull(umbracoHelperWrapper);

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <summary>
        /// Gets a model for the member with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The member model type.</typeparam>
        /// <param name="id">The member identifier.</param>
        /// <returns>A model for the member with the specified identifier.</returns>
        public T GetMember<T>(int id)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<MemberTypeAttribute>(), nameof(T));

            return GetMember<T>(_umbracoHelperWrapper.TypedMember(id));
        }

        /// <summary>
        /// Gets a model for the member.
        /// </summary>
        /// <typeparam name="T">The member model type.</typeparam>
        /// <param name="content">The member content.</param>
        /// <returns>A model for the member.</returns>
        public T GetMember<T>(IPublishedContent content)
            where T : class, new()
        {
            EnsureArg.IsTrue(typeof(T).IsModelType<MemberTypeAttribute>(), nameof(T));
            EnsureArg.IsNotNull(content);

            return (T)GetMember(content, typeof(T));
        }

        /// <summary>
        /// Gets a model for the member.
        /// </summary>
        /// <param name="content">The member content.</param>
        /// <param name="memberModelType">The member model type.</param>
        /// <returns>A model for the member.</returns>
        public object GetMember(IPublishedContent content, Type memberModelType)
        {
            EnsureArg.IsNotNull(content);
            EnsureArg.IsNotNull(memberModelType);
            EnsureArg.IsTrue(memberModelType.IsModelType<MemberTypeAttribute>(), nameof(memberModelType));

            return GetMappedContent(content, memberModelType);
        }
    }
}
