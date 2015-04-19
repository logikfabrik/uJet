//----------------------------------------------------------------------------------
// <copyright file="JetController.cs" company="Logikfabrik">
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

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;
    using Utilities;

    public abstract class JetController : Controller, IRenderMvcController
    {
        // Key found by examining the Umbraco source code.
        private const string RouteDataTokenKey = "umbraco";

        [ActionName("404")]
        public virtual ActionResult Index(RenderModel model)
        {
            return this.HttpNotFound();
        }
        
        protected RedirectResult RedirectToPage()
        {
            return this.RedirectToPage(this.GetPageId());
        }

        protected RedirectResult RedirectToPage(int id)
        {
            return this.RedirectToPage(id, null);
        }

        protected RedirectResult RedirectToPage(object query)
        {
            return this.RedirectToPage(this.GetPageId(), query);
        }

        protected RedirectResult RedirectToPage(int id, object query)
        {
            var url = UrlUtility.GetUrl(id, query);

            return this.Redirect(url);
        }

        private int GetPageId()
        {
            var renderModel = (RenderModel)ControllerContext.RouteData.DataTokens[RouteDataTokenKey];

            return renderModel.Content.Id;
        }
    }
}
