// <copyright file="JetModelBinder.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Web.Mvc;
    using Data;
    using global::Umbraco.Web.Models;

    /// <summary>
    /// The <see cref="JetControllerFactory" /> class.
    /// </summary>
    public class JetModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// The route data token key. Key found by examining the Umbraco source code.
        /// </summary>
        private const string RouteDataTokenKey = "umbraco";

        private readonly ITypeService _typeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetModelBinder" /> class.
        /// </summary>
        public JetModelBinder()
            : this(TypeService.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JetModelBinder" /> class.
        /// </summary>
        /// <param name="typeService">The type service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeService" /> is <c>null</c>.</exception>
        public JetModelBinder(ITypeService typeService)
        {
            if (typeService == null)
            {
                throw new ArgumentNullException();
            }

            _typeService = typeService;
        }

        /// <summary>
        /// Binds the model.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The bound model.</returns>
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

            var renderModel = controllerContext.RouteData.DataTokens[RouteDataTokenKey] as IRenderModel;

            return renderModel == null
                ? base.BindModel(controllerContext, bindingContext)
                : new DocumentService().GetDocument(renderModel.Content, bindingContext.ModelType);
        }

        /// <summary>
        /// Determines if the model type should be bound.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns><c>true</c> if the model type should be bound; otherwise, <c>false</c>.</returns>
        private bool ShouldBind(Type modelType)
        {
            return _typeService.DocumentTypes.Contains(modelType);
        }
    }
}
