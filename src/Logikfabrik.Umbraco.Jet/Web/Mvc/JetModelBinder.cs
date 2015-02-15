﻿// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Logikfabrik.Umbraco.Jet.Web.Data;
using System;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    public class JetModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!ShouldBind(bindingContext.ModelType)) 
                return base.BindModel(controllerContext, bindingContext);

            // Key found by examining the Umbraco source code.
            const string key = "umbraco";

            if (!controllerContext.RouteData.DataTokens.ContainsKey(key))
                return base.BindModel(controllerContext, bindingContext);

            var renderModel = controllerContext.RouteData.DataTokens[key] as RenderModel;

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