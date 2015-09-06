// <copyright file="IUmbracoHelperWrapper.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using global::Umbraco.Core.Models;

    /// <summary>
    /// The <see cref="IUmbracoHelperWrapper" /> interface.
    /// </summary>
    public interface IUmbracoHelperWrapper
    {
        /// <summary>
        /// Gets typed document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Typed document.</returns>
        IPublishedContent TypedDocument(int id);

        /// <summary>
        /// Gets typed media.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Typed media.</returns>
        IPublishedContent TypedMedia(int id);
    }
}