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

using System.Web.Mvc;
using Logikfabrik.Umbraco.Jet.Web.Mvc.Utilities;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    public abstract class JetController : Controller, IRenderMvcController
    {
        [ActionName("404")]
        public virtual ActionResult Index(RenderModel model)
        {
            return HttpNotFound();
        }

        private int GetPageId()
        {
            // Key found by examining the Umbraco source code.
            const string key = "umbraco";

            var renderModel = (RenderModel)ControllerContext.RouteData.DataTokens[key];

            return renderModel.Content.Id;
        }

        protected RedirectResult RedirectToPage()
        {
            return RedirectToPage(GetPageId());
        }

        protected RedirectResult RedirectToPage(int id)
        {
            return RedirectToPage(id, null);
        }

        protected RedirectResult RedirectToPage(object query)
        {
            return RedirectToPage(GetPageId(), query);
        }

        protected RedirectResult RedirectToPage(int id, object query)
        {
            var url = UrlUtility.GetUrl(id, query);

            return Redirect(url);
        }
    }
}
