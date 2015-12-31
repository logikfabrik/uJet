// <copyright file="MemberService.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using Extensions;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="MemberService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to member models.
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
        /// Gets a model for the member with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The member model type.</typeparam>
        /// <param name="id">The member identifier.</param>
        /// <returns>A model for the member with the specified identifier.</returns>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a member model type.</exception>
        public T GetMember<T>(int id)
            where T : class, new()
        {
            if (!typeof(T).IsMemberType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a member model type.");
            }

            return GetMember<T>(_umbracoHelperWrapper.TypedMember(id));
        }

        /// <summary>
        /// Gets a model for the member.
        /// </summary>
        /// <typeparam name="T">The member model type.</typeparam>
        /// <param name="content">The member content.</param>
        /// <returns>A model for the member.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="T" /> is not a member model type.</exception>
        public T GetMember<T>(IPublishedContent content)
            where T : class, new()
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!typeof(T).IsMemberType())
            {
                throw new ArgumentException($"Type {typeof(T)} is not a member model type.");
            }

            return (T)GetMember(content, typeof(T));
        }

        /// <summary>
        /// Gets a model for the member.
        /// </summary>
        /// <param name="content">The member content.</param>
        /// <param name="memberModelType">The member model type.</param>
        /// <returns>A model for the member.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="content" />, or <paramref name="memberModelType" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="memberModelType" /> is not a member model type.</exception>
        public object GetMember(IPublishedContent content, Type memberModelType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (memberModelType == null)
            {
                throw new ArgumentNullException(nameof(memberModelType));
            }

            if (!memberModelType.IsMemberType())
            {
                throw new ArgumentException($"Type {memberModelType} is not a member model type.", nameof(memberModelType));
            }

            return GetMappedContent(content, memberModelType);
        }
    }
}
