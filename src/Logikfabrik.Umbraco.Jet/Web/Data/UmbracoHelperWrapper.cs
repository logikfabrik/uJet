// <copyright file="UmbracoHelperWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using EnsureThat;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    /// <summary>
    /// The <see cref="UmbracoHelperWrapper" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class UmbracoHelperWrapper : IUmbracoHelperWrapper
    {
        private readonly UmbracoHelper _umbracoHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoHelperWrapper" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
        public UmbracoHelperWrapper()
            : this(new UmbracoHelper(UmbracoContext.Current))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoHelperWrapper" /> class.
        /// </summary>
        /// <param name="umbracoHelper">The Umbraco helper.</param>
        public UmbracoHelperWrapper(UmbracoHelper umbracoHelper)
        {
            Ensure.That(umbracoHelper).IsNotNull();

            _umbracoHelper = umbracoHelper;
        }

        /// <inheritdoc />
        public IPublishedContent TypedDocument(int id)
        {
            return _umbracoHelper.TypedContent(id);
        }

        /// <inheritdoc />
        public IPublishedContent TypedMedia(int id)
        {
            return _umbracoHelper.TypedMedia(id);
        }

        /// <inheritdoc />
        public IPublishedContent TypedMember(int id)
        {
            return _umbracoHelper.TypedMember(id);
        }
    }
}