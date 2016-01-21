// <copyright file="JetApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2015 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using Configuration;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Services;

    /// <summary>
    /// The <see cref="JetApplicationHandler" /> class. Class for subscribing to Umbraco application events, setting up uJet.
    /// </summary>
    public class JetApplicationHandler : ApplicationHandler
    {
        private static readonly object Lock = new object();

        private static bool configured;

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

            if (configured)
            {
                return;
            }

            lock (Lock)
            {
                // Synchronize.
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DataTypes))
                {
                    new DataTypeSynchronizer(
                        ApplicationContext.Current.Services.DataTypeService,
                        TypeResolver.Instance,
                        TypeRepository.Instance).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes))
                {
                    new TemplateSynchronizer(
                        ApplicationContext.Current.Services.FileService,
                        TemplateService.Instance).Run();
                    new DocumentTypeSynchronizer(
                        ApplicationContext.Current.Services.ContentTypeService,
                        TypeResolver.Instance,
                        TypeRepository.Instance,
                        ApplicationContext.Current.Services.FileService).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MediaTypes))
                {
                    new MediaTypeSynchronizer(
                        ApplicationContext.Current.Services.ContentTypeService,
                        TypeResolver.Instance,
                        TypeRepository.Instance).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MemberTypes))
                {
                    new MemberTypeSynchronizer(
                        ApplicationContext.Current.Services.MemberTypeService,
                        TypeResolver.Instance,
                        TypeRepository.Instance).Run();
                }

                var defaultValueService = new DefaultValueService(TypeResolver.Instance, TypeRepository.Instance);

                // Wire up handler for document type default values.
                ContentService.Saving += (sender, args) => defaultValueService.SetDefaultValues(args.SavedEntities);

                // Wire up handler for media type default values.
                MediaService.Saving += (sender, args) => defaultValueService.SetDefaultValues(args.SavedEntities);

                // Wire up handler for member type default values.
                MemberService.Saving += (sender, args) => defaultValueService.SetDefaultValues(args.SavedEntities);

                configured = true;
            }
        }
    }
}