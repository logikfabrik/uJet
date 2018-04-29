// <copyright file="JetController.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;
    using Utilities;

    /// <summary>
    /// The <see cref="JetController" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class JetController : Controller, IRenderMvcController
    {
        /// <summary>
        /// The route data token key. Key found by examining the Umbraco source code.
        /// </summary>
        private const string RouteDataTokenKey = "umbraco";

        /// <inheritdoc />
        [ActionName("404")]
        public virtual ActionResult Index(RenderModel model)
        {
            return HttpNotFound();
        }

        /// <summary>
        /// Redirects to the current page.
        /// </summary>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage()
        {
            return RedirectToPage(GetCurrentPageId());
        }

        /// <summary>
        /// Redirects to the current page.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage(object query)
        {
            return RedirectToPage(GetCurrentPageId(), query);
        }

        /// <summary>
        /// Redirects to page.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage(int id)
        {
            return RedirectToPage(id, null);
        }

        /// <summary>
        /// Redirects to page.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="query">The query.</param>
        /// <returns>A redirect result.</returns>
        protected RedirectResult RedirectToPage(int id, object query)
        {
            var url = UrlUtility.GetUrl(id, query);

            return Redirect(url);
        }

        /// <summary>
        /// Gets the current page identifier.
        /// </summary>
        /// <returns>The page identifier.</returns>
        private int GetCurrentPageId()
        {
            if (ControllerContext.RouteData.DataTokens[RouteDataTokenKey] is RenderModel renderModel)
            {
                return renderModel.Content.Id;
            }

            return -1;
        }
    }
}
