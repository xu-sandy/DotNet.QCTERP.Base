using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;
using Autofac.Integration.Mvc;
using System.Web.Mvc;

namespace Qct.Infrastructure.DI.WebMVC
{
    public abstract class AutofacMVCBootstapper : AutofacBootstapper
    {
        public override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            builder.RegisterControllers(GetType().Assembly).PropertiesAutowired();
            Configure(builder);
        }
        public override IContainer CreateContainer(ContainerBuilder containerBuilder)
        {
            var container = base.CreateContainer(containerBuilder);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            return container;
        }
        public abstract void Configure(ContainerBuilder builder);
    }
}
