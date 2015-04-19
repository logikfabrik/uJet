//----------------------------------------------------------------------------------
// <copyright file="UrlUtility.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet.Web.Mvc.Utilities
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;

    public static class UrlUtility
    {
        /// <summary>
        /// Gets a URL using the given node ID.
        /// </summary>
        /// <param name="nodeId">The node ID to get a URL using.</param>
        /// <returns>A URL.</returns>
        public static string GetUrl(int nodeId)
        {
            return GetUrl(nodeId, null);
        }

        /// <summary>
        /// Gets a URL using the given node ID and query.
        /// </summary>
        /// <param name="nodeId">The node ID to get a URL using.</param>
        /// <param name="query">The query to get a URL using.</param>
        /// <returns>A URL.</returns>
        public static string GetUrl(int nodeId, object query)
        {
            var url = umbraco.library.NiceUrl(nodeId);

            return GetUrl(url, query);
        }

        /// <summary>
        /// Gets a URL using the given URL.
        /// </summary>
        /// <param name="url">The URL to get a URL using.</param>
        /// <returns>A URL.</returns>
        public static string GetUrl(string url)
        {
            return GetUrl(url, null);
        }

        /// <summary>
        /// Gets a URL using the given URL and query.
        /// </summary>
        /// <param name="url">The URL to get a URL using.</param>
        /// <param name="query">The query to get a URL using.</param>
        /// <returns>A URL.</returns>
        public static string GetUrl(string url, object query)
        {
            var q = new NameValueCollection();

            if (query == null)
            {
                return string.Concat(url, GetQuery(q));
            }

            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(query))
            {
                var v = propertyDescriptor.GetValue(query) ?? string.Empty;

                q.Add(propertyDescriptor.Name, v.ToString());
            }

            return string.Concat(url, GetQuery(q));
        }

        /// <summary>
        /// Gets a query string using the given query.
        /// </summary>
        /// <param name="query">The query to get a query string using.</param>
        /// <returns>A query string.</returns>
        private static string GetQuery(NameValueCollection query)
        {
            if (query.Count == 0)
            {
                return null;
            }

            var q = query.AllKeys.SelectMany(
                query.GetValues,
                (key, value) =>
                    string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)));

            return string.Concat("?", string.Join("&", q));
        }
    }
}
