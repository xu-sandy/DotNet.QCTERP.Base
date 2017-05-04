using Qct.Infrastructure.Exceptions;
using Qct.Objects.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Qct.POS.Api.Retailing.Filters
{
    public class StoreApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 拦截异常，并响应请求，记录异常日志
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null) return;
            var result = OperateResult.Fail();
            if (actionExecutedContext.Exception is QCTException)
            {
                var ex = actionExecutedContext.Exception as QCTException;
                result.Code = ex.ErrorCode ?? "501";
                result.Message = ex.Message;
                result.ErrorData = ex.Datas;
                var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, result, "application/json");
                actionExecutedContext.Response = response;
            }
            else
            {
                var ex = actionExecutedContext.Exception;
                result.Code = "500";
                result.Message = string.Format("服务器配置出错或发生异常：{0}！", ex.Message);
                var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, result, "application/json");
                actionExecutedContext.Response = response;
            }

            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

        }
    }
}