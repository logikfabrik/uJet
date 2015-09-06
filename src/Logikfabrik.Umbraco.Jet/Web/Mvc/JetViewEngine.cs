// <copyright file="JetViewEngine.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Linq;
    using System.Web.Mvc;
    using global::Umbraco.Web.Mvc;

    public class JetViewEngine : RenderViewEngine
    {
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

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, JetControllerActionInvoker.GetActionName(viewName), masterName, useCache);
        }
    }
}
