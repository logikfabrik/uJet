// <copyright file="JetApplicationHandler.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using Configuration;
    using Data;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Configuration;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.ObjectResolution;
    using global::Umbraco.Core.Services;
    using Logging;
    using Mappings;

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
                var logService = new LogService();

                logService.Log<JetApplicationHandler>(new LogEntry(LogEntryType.Information, "Begin register built-in data type definition mappings."));

                BuiltInDataTypeDefinitionMappingsRegistrar.RegisterAll();

                logService.Log<JetApplicationHandler>(new LogEntry(LogEntryType.Information, "Begin synchronizing types."));

                var modelService = new ModelService(new ModelTypeService(logService, new AssemblyLoader(AppDomain.CurrentDomain, JetConfigurationManager.Assemblies)));
                var typeRepository = new TypeRepository(new ContentTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax)), new DataTypeRepository(new DatabaseWrapper(ApplicationContext.Current.DatabaseContext.Database, ResolverBase<LoggerResolver>.Current.Logger, ApplicationContext.Current.DatabaseContext.SqlSyntax)));
                var dataTypeDefinitionService = new DataTypeDefinitionService(applicationContext.Services.DataTypeService, UmbracoConfig.For.UmbracoSettings().Content.EnablePropertyValueConverters);

                // Synchronize.
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationModes.DataTypes))
                {
                    logService.Log<JetApplicationHandler>(new LogEntry(LogEntryType.Information, "Data type synchronization enabled. Begin synchronizing data types."));

                    new DataTypeSynchronizer(
                        applicationContext.Services.DataTypeService,
                        new DataTypeDefinitionFinder(typeRepository),
                        modelService,
                        typeRepository).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationModes.DocumentTypes))
                {
                    logService.Log<JetApplicationHandler>(new LogEntry(LogEntryType.Information, "Document type synchronization enabled. Begin synchronizing templates."));

                    new TemplateSynchronizer(
                        applicationContext.Services.FileService,
                        TemplateService.Instance).Run();

                    logService.Log<JetApplicationHandler>(new LogEntry(LogEntryType.Information, "Document type synchronization enabled. Begin synchronizing document types."));

                    new DocumentTypeSynchronizer(
                        logService,
                        typeRepository,
                        modelService,
                        applicationContext.Services.ContentTypeService,
                        applicationContext.Services.FileService,
                        dataTypeDefinitionService).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationModes.MediaTypes))
                {
                    logService.Log<JetApplicationHandler>(new LogEntry(LogEntryType.Information, "Media type synchronization enabled. Begin synchronizing media types."));

                    new MediaTypeSynchronizer(
                        logService,
                        typeRepository,
                        modelService,
                        applicationContext.Services.ContentTypeService,
                        dataTypeDefinitionService).Run();
                }

                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationModes.MemberTypes))
                {
                    logService.Log<JetApplicationHandler>(new LogEntry(LogEntryType.Information, "Member type synchronization enabled. Begin synchronizing member types."));

                    new MemberTypeSynchronizer(
                        logService,
                        applicationContext.Services.MemberTypeService,
                        modelService,
                        typeRepository,
                        dataTypeDefinitionService).Run();
                }

                var defaultValueService = new DefaultValueService(
                    modelService,
                    new ContentTypeModelFinder<DocumentType, DocumentTypeAttribute, IContentType>(logService, typeRepository),
                    new ContentTypeModelFinder<MediaType, MediaTypeAttribute, IMediaType>(logService, typeRepository),
                    new ContentTypeModelFinder<MemberType, MemberTypeAttribute, IMemberType>(logService, typeRepository));

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