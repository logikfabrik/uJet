// <copyright file="MemberService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using EnsureThat;
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="MemberService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to member models.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class MemberService : ContentService<MemberTypeAttribute>
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberService" /> class.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InheritdocConsiderUsage
        public MemberService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public MemberService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            EnsureArg.IsNotNull(umbracoHelperWrapper);

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <inheritdoc />
        protected override IPublishedContent GetContent(int id)
        {
            return _umbracoHelperWrapper.TypedMember(id);
        }
    }
}
