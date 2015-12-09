// <copyright file="UrlUtility.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc.Utilities
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The <see cref="UrlUtility" /> class. Utility class for URL generation.
    /// </summary>
    public static class UrlUtility
    {
        /// <summary>
        /// Gets a URL using the specified node identifier.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>A URL for the specified node.</returns>
        public static string GetUrl(int nodeId)
        {
            return GetUrl(nodeId, null);
        }

        /// <summary>
        /// Gets a URL using the specified node identifier and query.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="query">The query.</param>
        /// <returns>A URL for the specified node with query.</returns>
        public static string GetUrl(int nodeId, object query)
        {
            var url = umbraco.library.NiceUrl(nodeId);

            return GetUrl(url, query);
        }

        /// <summary>
        /// Gets a URL using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A URL.</returns>
        public static string GetUrl(string url)
        {
            return GetUrl(url, null);
        }

        /// <summary>
        /// Gets a URL using the specified URL and query.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="query">The query.</param>
        /// <returns>A URL with query.</returns>
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
        /// Gets a query using the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A query.</returns>
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
