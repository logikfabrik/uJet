// <copyright file="JetControllerFactory.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using global::Umbraco.Web.Mvc;

    public class JetControllerFactory : RenderControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var controller = base.CreateController(requestContext, controllerName) as Controller;

            if (controller != null)
            {
                controller.ActionInvoker = new JetControllerActionInvoker();
            }

            return controller;
        }
    }
}
