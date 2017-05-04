using System.Web.Http;
using WebActivatorEx;
using Qct.POS.Api.Retailing;
using Swashbuckle.Application;
using System;
using Qct.POS.Api.Retailing.Filters;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Qct.POS.Api.Retailing
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "QCT Store Api Document");
                    c.IncludeXmlComments(string.Format(@"{0}BIN\Qct.POS.Api.Retailing.XML", AppDomain.CurrentDomain.BaseDirectory));
                    c.IncludeXmlComments(string.Format(@"{0}BIN\Qct.Objects.XML", AppDomain.CurrentDomain.BaseDirectory));
                    c.DescribeAllEnumsAsStrings(); 
                    c.OperationFilter<SwaggerOperationFilter>();
                }).EnableSwaggerUi(c =>
                {
                    c.InjectStylesheet(thisAssembly, "Qct.POS.Api.Retailing.Content.SwaggerInjectStylesheet.css");
                    c.InjectJavaScript(thisAssembly, "Qct.POS.Api.Retailing.Scripts.SwaggerInjectJavascript.js");
                });
        }
    }
}
