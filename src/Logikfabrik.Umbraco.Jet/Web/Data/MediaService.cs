// <copyright file="MediaService.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="MediaService" /> class. Service for mapping instances of <see cref="IPublishedContent" /> to media models.
    /// </summary>
    [PublicAPI]

    // ReSharper disable once InheritdocConsiderUsage
    public class MediaService : ContentService<MediaTypeAttribute>
    {
        private readonly IUmbracoHelperWrapper _umbracoHelperWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaService" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public MediaService()
            : this(new UmbracoHelperWrapper())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaService" /> class.
        /// </summary>
        /// <param name="umbracoHelperWrapper">The Umbraco helper wrapper.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public MediaService(IUmbracoHelperWrapper umbracoHelperWrapper)
        {
            Ensure.That(umbracoHelperWrapper).IsNotNull();

            _umbracoHelperWrapper = umbracoHelperWrapper;
        }

        /// <inheritdoc />
        protected override IPublishedContent GetContent(int id)
        {
            return _umbracoHelperWrapper.TypedMedia(id);
        }
    }
}