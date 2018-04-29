// <copyright file="ApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System.Configuration;
    using global::Umbraco.Core;

    /// <summary>
    /// The <see cref="ApplicationHandler" /> class. Base class for subscribing to Umbraco application events.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public abstract class ApplicationHandler : IApplicationEventHandler
    {
        /// <summary>
        /// Gets a value indicating whether Umbraco is installed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if Umbraco is installed; otherwise, <c>false</c>.
        /// </value>
        protected bool IsInstalled => !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["umbracoConfigurationStatus"]);

        /// <inheritdoc />
        public virtual void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        /// <inheritdoc />
        public virtual void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        /// <inheritdoc />
        public virtual void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
    }
}