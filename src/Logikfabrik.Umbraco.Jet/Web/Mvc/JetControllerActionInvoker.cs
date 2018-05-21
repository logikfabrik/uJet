// <copyright file="JetControllerActionInvoker.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using global::Umbraco.Web.Mvc;

    /// <summary>
    /// The <see cref="JetControllerActionInvoker" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class JetControllerActionInvoker : RenderActionInvoker
    {
        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The name of the action.</returns>
        public static string GetActionName(string name)
        {
            return name == PreviewTemplateAttribute.TemplateName.Alias() ? "Index" : name;
        }

        /// <inheritdoc />
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
