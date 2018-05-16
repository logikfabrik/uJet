// <copyright file="JetApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using Configuration;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="JetApplicationHandler" /> class. Class for subscribing to Umbraco application events, setting up uJet.
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public class JetApplicationHandler : ApplicationHandler
    {
        private static readonly object Lock = new object();

        private bool _configured;

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

                var factory = new SynchronizerFactory(container);

                foreach (var synchronizer in factory.Create(JetConfigurationManager.Synchronize))
                {
                    synchronizer.Run();
                }

                var defaultValueService = container.Resolve<IDefaultValueService>();

                // Wire up handler for document type default values.
                ContentService.Saving += (sender, args) => defaultValueService.SetDefaultValues(args.SavedEntities);

                // Wire up handler for media type default values.
                MediaService.Saving += (sender, args) => defaultValueService.SetDefaultValues(args.SavedEntities);

                // Wire up handler for member type default values.
                MemberService.Saving += (sender, args) => defaultValueService.SetDefaultValues(args.SavedEntities);

                _configured = true;
            }
        }
    }
}