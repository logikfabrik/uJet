// <copyright file="JetControllerActionInvoker.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using Extensions;
    using global::Umbraco.Web.Mvc;

    public class JetControllerActionInvoker : RenderActionInvoker
    {
        public static string GetActionName(string name)
        {
            return name == PreviewTemplateAttribute.TemplateName.Alias() ? "Index" : name;
        }

        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            var descriptor = base.FindAction(controllerContext, controllerDescriptor, actionName);

            if (descriptor != null || !(controllerContext.Controller is IRenderMvcController))
            {
                return descriptor;
            }

            return controllerDescriptor.FindAction(controllerContext, GetActionName(actionName));
        }
    }
}
