using Qct.Objects.ValueObjects;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Qct.POS.Api.Retailing.Filters
{
    public class StoreApiActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response == null) return;
            var content = actionExecutedContext.Response.Content;
            var x = content == null ? null : content.ReadAsAsync<object>().Result;
            var result = OperateResult.Success(code: "200", data: x);
            var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, result, "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            actionExecutedContext.Response = response;
        }
    }
}