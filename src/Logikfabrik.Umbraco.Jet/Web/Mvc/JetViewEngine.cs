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
    // ReSharper disable once InheritdocConsiderUsage
    public class JetViewEngine : RenderViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JetViewEngine" /> class.
        /// </summary>
        // ReSharper disable once InheritdocConsiderUsage
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

        /// <inheritdoc />
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, JetControllerActionInvoker.GetActionName(viewName), masterName, useCache);
        }
    }
}
