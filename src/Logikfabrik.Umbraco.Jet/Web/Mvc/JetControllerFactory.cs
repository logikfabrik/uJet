// <copyright file="JetControllerFactory.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using global::Umbraco.Web.Mvc;

    /// <summary>
    /// The <see cref="JetControllerFactory" /> class.
    /// </summary>
    public class JetControllerFactory : RenderControllerFactory
    {
        /// <summary>
        /// Creates the controller.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>The controller.</returns>
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
