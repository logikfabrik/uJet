// <copyright file="JetModelBinder.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Data;
    using global::Umbraco.Web.Models;

    public class JetModelBinder : DefaultModelBinder
    {
        // Key found by examining the Umbraco source code.
        private const string RouteDataTokenKey = "umbraco";

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!ShouldBind(bindingContext.ModelType))
            {
                return base.BindModel(controllerContext, bindingContext);
            }

            if (!controllerContext.RouteData.DataTokens.ContainsKey(RouteDataTokenKey))
            {
                return base.BindModel(controllerContext, bindingContext);
            }

            var renderModel = controllerContext.RouteData.DataTokens[RouteDataTokenKey] as RenderModel;

            return renderModel == null
                ? base.BindModel(controllerContext, bindingContext)
                : new DocumentService().GetDocument(renderModel.Content, bindingContext.ModelType);
        }

        private static bool ShouldBind(Type modelType)
        {
            return TypeService.Instance.DocumentTypes.Contains(modelType);
        }
    }
}
