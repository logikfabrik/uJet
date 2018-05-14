// <copyright file="JetMvcApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

using Logikfabrik.Umbraco.Jet.Web.Data;

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System;
    using System.Web.Mvc;
    using Configuration;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Web.Mvc;
    using Jet.Data;
    using Logging;

    /// <summary>
    /// The <see cref="JetMvcApplicationHandler" /> class.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class JetMvcApplicationHandler : ApplicationHandler
    {
        private static readonly object Lock = new object();

        private bool _configured;

        /// <inheritdoc />
        public override void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var resolver = FilteredControllerFactoriesResolver.Current;

            if (resolver.ContainsType(typeof(JetControllerFactory)))
            {
                return;
            }

            resolver.InsertTypeBefore(typeof(RenderControllerFactory), typeof(JetControllerFactory));
        }

        /// <inheritdoc />
        public override void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (!IsInstalled)
            {
                return;
            }

            if (_configured)
            {
                return;
            }

            lock (Lock)
            {
                var logService = new LogService();
                var modelTypeService = new ModelTypeService(logService, new AssemblyLoader(AppDomain.CurrentDomain, JetConfigurationManager.Assemblies));
                var modelService = new ModelService(modelTypeService);

                // Synchronize.
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationModes.DocumentTypes))
                {
                    var typeRepository = new TypeRepository(new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax)), new DataTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax)));

                    new PreviewTemplateSynchronizer(
                        logService,
                        ApplicationContext.Current.Services.ContentTypeService,
                        ApplicationContext.Current.Services.FileService,
                        modelService,
                        typeRepository).Run();
                }

                ModelBinders.Binders.DefaultBinder = new JetModelBinder(new DocumentService(), modelTypeService.DocumentTypes);

                // Adds the Jet view engine. The Jet view engine allows views to be structured using the ASP.NET MVC convention.
                ViewEngines.Engines.Insert(0, new JetViewEngine());

                _configured = true;
            }
        }
    }
}
