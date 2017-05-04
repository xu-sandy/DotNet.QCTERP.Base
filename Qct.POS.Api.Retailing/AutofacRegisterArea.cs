using Qct.Infrastructure.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace Qct.POS.Api.Retailing
{
    public class AutofacRegisterArea : IAutofacRegisterArea
    {
        public void Register(ContainerBuilder builder)
        {
            builder.Register<>
        }
    }
}