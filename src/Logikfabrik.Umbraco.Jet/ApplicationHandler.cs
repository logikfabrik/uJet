// <copyright file="ApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Configuration;
    using global::Umbraco.Core;

    /// <summary>
    /// The <see cref="ApplicationHandler" /> class. Base class for subscribing to Umbraco application events.
    /// </summary>
    public abstract class ApplicationHandler : IApplicationEventHandler
    {
        /// <summary>
        /// Gets a value indicating whether Umbraco is installed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if Umbraco is installed; otherwise, <c>false</c>.
        /// </value>
        protected bool IsInstalled => !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["umbracoConfigurationStatus"]);

        /// <summary>
        /// Called when the application is initialized.
        /// </summary>
        /// <param name="umbracoApplication">The Umbraco application.</param>
        /// <param name="applicationContext">The application context.</param>
        public virtual void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        /// <summary>
        /// Called when the application is starting.
        /// </summary>
        /// <param name="umbracoApplication">The Umbraco application.</param>
        /// <param name="applicationContext">The application context.</param>
        public virtual void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        /// <summary>
        /// Called when the application is started.
        /// </summary>
        /// <param name="umbracoApplication">The Umbraco application.</param>
        /// <param name="applicationContext">The application context.</param>
        public virtual void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
    }
}