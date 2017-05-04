using Autofac;
using System;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace Qct.Infrastructure.DI.WebAPI
{
    public abstract class AutofacAPIBootstapper : AutofacBootstapper
    {
        HttpConfiguration config;
        public AutofacAPIBootstapper(HttpConfiguration config)
        {
            if (config == null)
                throw new Exception();
            this.config = config;
        }
        public override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(GetType().Assembly).PropertiesAutowired();
            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);
            Configure(builder);
        }

        public override IContainer CreateContainer(ContainerBuilder containerBuilder)
        {
            var container = base.CreateContainer(containerBuilder);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }
        public abstract void Configure(ContainerBuilder builder);
    }
}
