// <copyright file="JetMvcApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using Configuration;
    using global::Umbraco.Core;
    using global::Umbraco.Web.Mvc;
    using Jet.Data;
    using Logging;

    /// <summary>
    /// The <see cref="JetMvcApplicationHandler" /> class.
    /// </summary>
    public class JetMvcApplicationHandler : ApplicationHandler
    {
        private static readonly object Lock = new object();

        private bool _configured;

        /// <summary>
        /// Called when the application is starting.
        /// </summary>
        /// <param name="umbracoApplication">The Umbraco application.</param>
        /// <param name="applicationContext">The application context.</param>
        public override void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var resolver = FilteredControllerFactoriesResolver.Current;

            if (resolver.ContainsType(typeof(JetControllerFactory)))
            {
                return;
            }

            resolver.InsertTypeBefore(typeof(RenderControllerFactory), typeof(JetControllerFactory));
        }

        /// <summary>
        /// Called when the application is started.
        /// </summary>
        /// <param name="umbracoApplication">The Umbraco application.</param>
        /// <param name="applicationContext">The application context.</param>
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
                // Synchronize.
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes))
                {
                    new PreviewTemplateSynchronizer(
                        new LogService(),
                        ApplicationContext.Current.Services.ContentTypeService,
                        ApplicationContext.Current.Services.FileService,
                        TypeResolver.Instance,
                        TypeRepository.Instance).Run();
                }

                ModelBinders.Binders.DefaultBinder = new JetModelBinder();

                // Adds the Jet view engine. The Jet view engine allows views to be structured using the ASP.NET MVC convention.
                ViewEngines.Engines.Insert(0, new JetViewEngine());

                _configured = true;
            }
        }
    }
}
