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
    // ReSharper disable once InheritdocConsiderUsage
    public class JetControllerFactory : RenderControllerFactory
    {
        /// <inheritdoc />
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
