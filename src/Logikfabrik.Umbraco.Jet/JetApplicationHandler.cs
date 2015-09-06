// <copyright file="JetApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using Configuration;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Services;

    public class JetApplicationHandler : ApplicationHandler
    {
        private static readonly object Lock = new object();
        private static bool configured;

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
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DataTypes))
                {
                    new DataTypeSynchronizationService().Synchronize();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes))
                {
                    new DocumentTypeSynchronizationService().Synchronize();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MediaTypes))
                {
                    new MediaTypeSynchronizationService().Synchronize();
                }

                // Wire up handler for document type default values.
                ContentService.Saving +=
                    (sender, args) => new DefaultValueService().SetDefaultValues(args.SavedEntities);

                // Wire up handler for media type default values.
                MediaService.Saving += (sender, args) => new DefaultValueService().SetDefaultValues(args.SavedEntities);

                configured = true;
            }
        }
    }
}
