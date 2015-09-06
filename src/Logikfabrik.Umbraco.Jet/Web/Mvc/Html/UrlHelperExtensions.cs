// <copyright file="UrlHelperExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc.Html
{
    using System.Web.Mvc;
    using Utilities;

    /// <summary>
    /// The <see cref="UrlHelperExtensions" /> class.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Gets an URL for the specified identifier.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>An URL.</returns>
        public static string Url(this UrlHelper urlHelper, int id)
        {
            return UrlUtility.GetUrl(id);
        }

        /// <summary>
        /// Gets an URL for the specified identifier with query.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="query">The query.</param>
        /// <returns>An URL.</returns>
        public static string Url(this UrlHelper urlHelper, int id, object query)
        {
            return UrlUtility.GetUrl(id, query);
        }

        /// <summary>
        /// Gets an URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="url">The URL.</param>
        /// <returns>An URL.</returns>
        public static string Url(this UrlHelper urlHelper, string url)
        {
            return UrlUtility.GetUrl(url);
        }

        /// <summary>
        /// Gets an URL with query
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="query">The query.</param>
        /// <returns>An URL.</returns>
        public static string Url(this UrlHelper urlHelper, string url, object query)
        {
            return UrlUtility.GetUrl(url, query);
        }
    }
}
