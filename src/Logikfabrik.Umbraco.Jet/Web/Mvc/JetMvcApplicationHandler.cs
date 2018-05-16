// <copyright file="JetMvcApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Web.Mvc;

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
                var container = new ContainerFactory(applicationContext).Create();

                ModelBinders.Binders.DefaultBinder = new JetModelBinder(new DocumentService(), container.Resolve<IModelTypeService>().DocumentTypes);

                // Adds the Jet view engine. The Jet view engine allows views to be structured using the ASP.NET MVC convention.
                ViewEngines.Engines.Insert(0, new JetViewEngine());

                _configured = true;
            }
        }
    }
}
