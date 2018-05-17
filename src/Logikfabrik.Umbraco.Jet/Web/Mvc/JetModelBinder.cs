// <copyright file="JetModelBinder.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Data;
    using EnsureThat;

    /// <summary>
    /// The <see cref="JetModelBinder" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class JetModelBinder : DefaultModelBinder
    {
        private readonly IContentService _documentService;
        private readonly IEnumerable<Type> _documentModelTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetModelBinder" /> class.
        /// </summary>
        /// <param name="documentService">The document service.</param>
        /// <param name="documentModelTypes">The document model types.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public JetModelBinder(IContentService documentService, IEnumerable<Type> documentModelTypes)
        {
            Ensure.That(documentService).IsNotNull();
            Ensure.That(documentModelTypes).IsNotNull();

            _documentService = documentService;
            _documentModelTypes = documentModelTypes;
        }

        /// <inheritdoc />
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!ShouldBind(bindingContext.ModelType))
            {
                return base.BindModel(controllerContext, bindingContext);
            }

            var renderModel = controllerContext.GetRenderModel();

            return renderModel == null
                ? base.BindModel(controllerContext, bindingContext)
                : _documentService.GetContent(renderModel.Content, bindingContext.ModelType);
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
            return _documentModelTypes.Contains(modelType);
        }
    }
}
