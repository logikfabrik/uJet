//----------------------------------------------------------------------------------
// <copyright file="UrlHelperExtensions.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

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
