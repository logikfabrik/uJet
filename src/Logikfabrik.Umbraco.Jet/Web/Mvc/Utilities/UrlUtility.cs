// <copyright file="UrlUtility.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

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
                    $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}");

            return string.Concat("?", string.Join("&", q));
        }
    }
}
