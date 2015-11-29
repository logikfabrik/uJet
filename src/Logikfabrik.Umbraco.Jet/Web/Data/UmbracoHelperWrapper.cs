// <copyright file="UmbracoHelperWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    /// <summary>
    /// The <see cref="UmbracoHelperWrapper" /> class.
    /// </summary>
    public class UmbracoHelperWrapper : IUmbracoHelperWrapper
    {
        private readonly UmbracoHelper _umbracoHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoHelperWrapper" /> class.
        /// </summary>
        public UmbracoHelperWrapper()
            : this(new UmbracoHelper(UmbracoContext.Current))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoHelperWrapper" /> class.
        /// </summary>
        /// <param name="umbracoHelper">The Umbraco helper.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="umbracoHelper" /> is <c>null</c>.</exception>
        public UmbracoHelperWrapper(UmbracoHelper umbracoHelper)
        {
            if (umbracoHelper == null)
            {
                throw new ArgumentNullException(nameof(umbracoHelper));
            }

            _umbracoHelper = umbracoHelper;
        }

        /// <summary>
        /// Gets typed document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Typed document.
        /// </returns>
        public IPublishedContent TypedDocument(int id)
        {
            return _umbracoHelper.TypedContent(id);
        }

        /// <summary>
        /// Gets typed media.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Typed media.
        /// </returns>
        public IPublishedContent TypedMedia(int id)
        {
            return _umbracoHelper.TypedMedia(id);
        }
    }
}