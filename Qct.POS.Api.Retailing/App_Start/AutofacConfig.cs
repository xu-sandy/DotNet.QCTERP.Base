using Autofac;
using Qct.Infrastructure.DI.Extensions;
using Qct.Infrastructure.DI.WebAPI;
using Qct.IServices;
using Qct.Services.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Qct.POS.Api.Retailing
{
    public class AutofacConfig : AutofacAPIBootstapper
    {
        public AutofacConfig(HttpConfiguration config) : base(config)
        {

        }

        public override void Configure(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesAsImplementedInterfaces("Qct.Repository", "Qct.Services");
        }
    }
}