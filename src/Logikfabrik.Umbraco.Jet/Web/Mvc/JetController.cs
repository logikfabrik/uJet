// <copyright file="JetController.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

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
            return HttpNotFound();
        }

        /// <summary>
        /// Redirect to the current page.
        /// </summary>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage()
        {
            return RedirectToPage(GetCurrentPageId());
        }

        /// <summary>
        /// Redirect to a page.
        /// </summary>
        /// <param name="id">The ID of the page to redirect to.</param>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage(int id)
        {
            return RedirectToPage(id, null);
        }

        /// <summary>
        /// Redirect to the current page.
        /// </summary>
        /// <param name="query">The query to redirect to.</param>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage(object query)
        {
            return RedirectToPage(GetCurrentPageId(), query);
        }

        /// <summary>
        /// Redirect to a page.
        /// </summary>
        /// <param name="id">The ID of the page to redirect to.</param>
        /// <param name="query">The query to redirect to.</param>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage(int id, object query)
        {
            var url = UrlUtility.GetUrl(id, query);

            return Redirect(url);
        }

        /// <summary>
        /// Gets the current page ID.
        /// </summary>
        /// <returns>The page ID.</returns>
        private int GetCurrentPageId()
        {
            var renderModel = ControllerContext.RouteData.DataTokens[RouteDataTokenKey] as RenderModel;

            if (renderModel == null)
            {
                return -1;
            }

            return renderModel.Content.Id;
        }
    }
}
