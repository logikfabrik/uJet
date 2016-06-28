// <copyright file="JetApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using Configuration;
    using Data;
    using Extensions;
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
                this.Info("Begin synchronizing types.");

                var typeResolver = TypeResolver.Instance;
                var typeRepository = TypeRepository.Instance;

                // Synchronize.
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DataTypes))
                {
                    this.Info("Data type synchronization enabled. Begin synchronizing data types.");

                    new DataTypeSynchronizer(
                        applicationContext.Services.DataTypeService,
                        typeResolver,
                        typeRepository).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes))
                {
                    this.Info("Document type synchronization enabled. Begin synchronizing templates.");

                    new TemplateSynchronizer(
                        applicationContext.Services.FileService,
                        TemplateService.Instance).Run();

                    this.Info("Document type synchronization enabled. Begin synchronizing document types.");
                    new DocumentTypeSynchronizer(
                        applicationContext.Services.ContentTypeService,
                        applicationContext.Services.FileService,
                        typeResolver,
                        typeRepository).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MediaTypes))
                {
                    this.Info("Media type synchronization enabled. Begin synchronizing media types.");

                    new MediaTypeSynchronizer(
                        applicationContext.Services.ContentTypeService,
                        typeResolver,
                        typeRepository).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.MemberTypes))
                {
                    this.Info("Member type synchronization enabled. Begin synchronizing member types.");

                    new MemberTypeSynchronizer(
                        applicationContext.Services.MemberTypeService,
                        typeResolver,
                        typeRepository).Run();
                }

                var defaultValueService = new DefaultValueService(typeResolver, typeRepository);

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