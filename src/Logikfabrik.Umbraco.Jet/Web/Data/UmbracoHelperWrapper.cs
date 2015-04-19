//----------------------------------------------------------------------------------
// <copyright file="UmbracoHelperWrapper.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet.Web.Data
{
    using System;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    public class UmbracoHelperWrapper : IUmbracoHelperWrapper
    {
        private readonly UmbracoHelper umbracoHelper;

        public UmbracoHelperWrapper()
            : this(new UmbracoHelper(UmbracoContext.Current))
        {
        }

        public UmbracoHelperWrapper(UmbracoHelper umbracoHelper)
        {
            if (umbracoHelper == null)
            {
                throw new ArgumentNullException("umbracoHelper");
            }

            this.umbracoHelper = umbracoHelper;
        }

        public IPublishedContent TypedDocument(int id)
        {
            return this.umbracoHelper.TypedContent(id);
        }

        public IPublishedContent TypedMedia(int id)
        {
            return this.umbracoHelper.TypedMedia(id);
        }
    }
}