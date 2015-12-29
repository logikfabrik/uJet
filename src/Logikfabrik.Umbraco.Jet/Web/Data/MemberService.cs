// <copyright file="MemberService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="MemberService" /> class.
    /// </summary>
    public class MemberService : ContentService
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberService" /> class.
        /// </summary>
        public MemberService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="umbracoHelperWrapper" /> is <c>null</c>.</exception>
        public MemberService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            if (umbracoHelperWrapper == null)
            {
                throw new ArgumentNullException(nameof(umbracoHelperWrapper));
            }

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <summary>
        /// Gets the member with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The member type.</typeparam>
        /// <param name="id">The member identifier.</param>
        /// <returns>The member with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a member type.</exception>
        public T GetMember<T>(int id)
            where T : class, new()
        {
            if (!typeof(T).IsMemberType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a member type.");
            }

            return GetMember<T>(_umbracoHelperWrapper.TypedMember(id));
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <typeparam name="T">The member type.</typeparam>
        /// <param name="content">The member content.</param>
        /// <returns>The member.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a member type.</exception>
        public T GetMember<T>(IPublishedContent content)
            where T : class, new()
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!typeof(T).IsMemberType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a member type.");
            }

            return (T)GetMember(content, typeof(T));
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        /// <param name="content">The member content.</param>
        /// <param name="memberType">The member type.</param>
        /// <returns>The member.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="memberType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="memberType" /> is not a member type.</exception>
        public object GetMember(IPublishedContent content, Type memberType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            if (!memberType.IsMemberType())
            {
                throw new ArgumentException($"Type {memberType} is not a member type.", nameof(memberType));
            }

            return GetMappedContent(content, memberType);
        }
    }
}
