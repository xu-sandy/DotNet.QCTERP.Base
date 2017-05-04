using Qct.POS.Api.Retailing.Attributes;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Qct.POS.Api.Retailing.Filters
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            var isNeedLogin = apiDescription.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<StoreUserAuthorizeAttribute>().Any();
            if (!isNeedLogin)
            {
                isNeedLogin = apiDescription.ActionDescriptor.GetCustomAttributes<StoreUserAuthorizeAttribute>().Any(); //是否有验证用户标记

            }
            if (isNeedLogin)
            {
                if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
                {
                    return;
                }
                operation.parameters.Add(new Parameter { name = "Authorization", @in = "header", description = "登陆凭证号（token）", @default="Basic ", required = false, type = "string" });
            }
            if(apiDescription.ActionDescriptor.ActionName== "Login")
            {
                operation.parameters.Add(new Parameter { name = "Authorization", @in = "header", description = "登陆凭证号（token）", required = false, type = "string" });
            }
        }
    }
}
