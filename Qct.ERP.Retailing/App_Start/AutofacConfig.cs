using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Qct.Infrastructure.DI.Extensions;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Qct.IServices;
using Qct.IRepository;
using Qct.Repository;
using Qct.Services;
using Qct.ERP.Retailing.Controllers;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.DI.WebMVC;
using Qct.ERP.ApplicationService;

namespace Qct.ERP.Retailing
{
    public class AutofacConfig : AutofacMVCBootstapper
    {
        public override void Configure(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypesAsImplementedInterfaces("Qct.Repository", "Qct.Services");
            builder.RegisterType<Company>().As<ICompany>().SingleInstance();
            builder.RegisterType<SaleImportService>().AsSelf();
            //builder.RegisterType<SysMenuService>().As<ISysMenuService>();
            //builder.RegisterType<IndentOrderRepository>().As<IIndentOrderRepository>();
            //builder.RegisterType<NoticeRepository>().As<INoticeRepository>();
            //builder.RegisterType<SysWebSettingRepository>().As<ISysWebSettingRepository>();
            //builder.RegisterType<WarehouseRepository>().As<IWarehouseRepository>();
            //builder.RegisterType<ProductRepository>().As<IProductRepository>();
            //builder.RegisterType<SysMenusRepository>().As<ISysMenusRepository>();
        }
    }
}