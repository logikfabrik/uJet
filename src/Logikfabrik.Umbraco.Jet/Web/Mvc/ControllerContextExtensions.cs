﻿// <copyright file="ControllerContextExtensions.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using global::Umbraco.Web.Models;

    public static class ControllerContextExtensions
    {
        public static IRenderModel GetRenderModel(this ControllerContext context)
        {
            if (!context.RouteData.DataTokens.TryGetValue(RouteDataTokenKey.Key, out var renderModel))
            {
                return null;
            }

            return renderModel as IRenderModel;
        }
    }
}
