// <copyright file="JetModelBinder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Configuration;
    using Data;
    using EnsureThat;
    using global::Umbraco.Web.Models;
    using Logging;

    /// <summary>
    /// The <see cref="JetControllerFactory" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
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
        // ReSharper disable once InheritdocConsiderUsage
        public JetModelBinder()
            : this(new TypeService(new LogService(), new AssemblyLoader(AppDomain.CurrentDomain, JetConfigurationManager.Assemblies)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JetModelBinder" /> class.
        /// </summary>
        /// <param name="typeService">The type service.</param>
        public JetModelBinder(ITypeService typeService)
        {
            EnsureArg.IsNotNull(typeService);

            _typeService = typeService;
        }

        /// <inheritdoc />
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

            return controllerContext.RouteData.DataTokens[RouteDataTokenKey] is IRenderModel renderModel
                ? new DocumentService().GetDocument(renderModel.Content, bindingContext.ModelType)
                : base.BindModel(controllerContext, bindingContext);
        }

        /// <summary>
        /// Determines if the model type should be bound.
        /// </summary>
        /// <param name="modelType">The model type.</param>
        /// <returns>
        ///   <c>true</c> if the model type should be bound; otherwise, <c>false</c>.
        /// </returns>
        private bool ShouldBind(Type modelType)
        {
            return _typeService.DocumentTypes.Contains(modelType);
        }
    }
}
