namespace Logikfabrik.Umbraco.Jet
{
    using System;
    using Configuration;
    using Data;
    using EnsureThat;
    using global::Umbraco.Core;
    using global::Umbraco.Core.Configuration;
    using global::Umbraco.Core.Services;
    using Logging;
    using Mappings;

    public class ContainerFactory
    {
        private readonly ApplicationContext _applicationContext;

        public ContainerFactory(ApplicationContext applicationContext)
        {
            Ensure.That(applicationContext).IsNotNull();

            _applicationContext = applicationContext;
        }

        public IContainer Create()
        {
            var container = new Container();

            Configure(container);

            return container;
        }

        private void Configure(IContainer container)
        {
            container.Register(_applicationContext.Services.ContentTypeService);
            container.Register(_applicationContext.Services.MemberTypeService);
            container.Register(_applicationContext.Services.DataTypeService);
            container.Register(_applicationContext.Services.FileService);
            container.Register<ILogService, LogService>();
            container.Register(new AssemblyLoader(AppDomain.CurrentDomain, JetConfigurationManager.Assemblies));
            container.Register<IModelTypeService, ModelTypeService>();
            container.Register<IModelService, ModelService>();
            container.Register(_applicationContext.DatabaseContext.Database);
            container.Register(_applicationContext.DatabaseContext.SqlSyntax);
            container.Register(_applicationContext.ProfilingLogger.Logger);
            container.Register<IDatabaseWrapper, DatabaseWrapper>();
            container.Register<IContentTypeRepository, ContentTypeRepository>();
            container.Register<IDataTypeRepository, DataTypeRepository>();
            container.Register<ITypeRepository, TypeRepository>();
            container.Register(new DataTypeDefinitionService(container.Resolve<IDataTypeService>(), UmbracoConfig.For.UmbracoSettings().Content.EnablePropertyValueConverters));
            container.Register<IDefaultValueService, DefaultValueService>();
            container.Register<ITemplateService, TemplateService>();
        }
    }
}