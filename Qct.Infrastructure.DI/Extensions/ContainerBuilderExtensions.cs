using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Qct.Infrastructure.DI.Extensions
{
    public static class ContainerBuilderExtensions
    {

        public static ContainerBuilder RegisterAssemblyTypesAsImplementedInterfaces(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces().PropertiesAutowired();
            return builder;
        }

        public static ContainerBuilder RegisterAssemblyTypesAsImplementedInterfaces(this ContainerBuilder builder, params string[] assemblyNames)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(o => assemblyNames.Contains(o.GetName().Name)).ToArray();
            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces().PropertiesAutowired();
            return builder;
        }
    }
}
