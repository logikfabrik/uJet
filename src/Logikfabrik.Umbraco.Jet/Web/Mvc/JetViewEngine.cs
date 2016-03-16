// <copyright file="JetViewEngine.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Linq;
    using System.Web.Mvc;
    using global::Umbraco.Web.Mvc;

    /// <summary>
    /// The <see cref="JetViewEngine" /> class.
    /// </summary>
    public class JetViewEngine : RenderViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JetViewEngine" /> class.
        /// </summary>
        public JetViewEngine()
        {
            ViewLocationFormats =
                ViewLocationFormats.Concat(new[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"
                }).Distinct().ToArray();

            PartialViewLocationFormats =
                PartialViewLocationFormats.Concat(new[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"
                }).Distinct().ToArray();
        }

        /// <summary>
        /// Finds the view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="masterName">Name of the master.</param>
        /// <param name="useCache">If set to <c>true</c> use cache.</param>
        /// <returns>The view.</returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, JetControllerActionInvoker.GetActionName(viewName), masterName, useCache);
        }
    }
}
