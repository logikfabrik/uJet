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
        /// Gets the Umbraco document with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The Umbraco document with the specified identifier.</returns>
        IPublishedContent TypedDocument(int id);

        /// <summary>
        /// Gets the Umbraco media with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The Umbraco media with the specified identifier.</returns>
        IPublishedContent TypedMedia(int id);

        /// <summary>
        /// Gets the Umbraco member with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The Umbraco member with the specified identifier.</returns>
        IPublishedContent TypedMember(int id);
    }
}