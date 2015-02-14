// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Logikfabrik.Umbraco.Jet.Web.Mvc.Utilities
{
    public static class UrlUtility
    {
        public static string GetUrl(int id)
        {
            return GetUrl(id, null);
        }

        public static string GetUrl(int id, object query)
        {
            var url = umbraco.library.NiceUrl(id);

            return GetUrl(url, query);
        }

        public static string GetUrl(string url)
        {
            return GetUrl(url, null);
        }

        public static string GetUrl(string url, object query)
        {
            var q = new NameValueCollection();

            if (query == null) 
                return string.Concat(url, GetQuery(q));

            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(query))
            {
                var v = propertyDescriptor.GetValue(query) ?? string.Empty;

                q.Add(propertyDescriptor.Name, v.ToString());
            }

            return string.Concat(url, GetQuery(q));
        }

        private static string GetQuery(NameValueCollection query)
        {
            if (query.Count == 0)
                return null;

            var q = query.AllKeys.SelectMany(query.GetValues,
                                             (key, value) =>
                                             string.Format("{0}={1}", HttpUtility.UrlEncode(key),
                                                           HttpUtility.UrlEncode(value)));

            return string.Concat("?", string.Join("&", q));
        }
    }
}
