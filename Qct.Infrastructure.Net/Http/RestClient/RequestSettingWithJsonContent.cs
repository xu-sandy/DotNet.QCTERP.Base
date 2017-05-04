using Qct.Infrastructure.Json;
using System;

namespace Qct.Infrastructure.Net.Http.RestClient
{
    public class RequestSettingWithJsonContent<TParameter> : RequestSetting
    {
        public RequestSettingWithJsonContent(TParameter parameter) : base()
        {
            if (parameter == null)
                throw new ArgumentNullException("请求参数不能为空！");
            Content = JsonHelper.ToJson(parameter);
            ContentType = "application/json; charset=utf-8";
        }


    }
}
