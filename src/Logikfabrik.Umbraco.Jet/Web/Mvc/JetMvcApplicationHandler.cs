// <copyright file="JetMvcApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using Configuration;
    using global::Umbraco.Core;
    using global::Umbraco.Web.Mvc;

    public class JetMvcApplicationHandler : ApplicationHandler
    {
        private static readonly object Lock = new object();
        private static bool configured;

        public override void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            FilteredControllerFactoriesResolver.Current.InsertTypeBefore(typeof(RenderControllerFactory), typeof(JetControllerFactory));
        }

        public override void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (!IsInstalled)
            {
                return;
            }

            if (configured)
            {
                return;
            }

            lock (Lock)
            {
                // Synchronize.
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes))
                {
                    new TemplateSynchronizationService().Synchronize();
                    new PreviewTemplateSynchronizationService().Synchronize();
                }

                ModelBinders.Binders.DefaultBinder = new JetModelBinder();

                // Adds the Jet view engine. The Jet view engine allows views to be structured using the ASP.NET MVC convention.
                ViewEngines.Engines.Insert(0, new JetViewEngine());

                configured = true;
            }
        }
    }
}
