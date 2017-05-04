using Pharos.Domain.CommonObject;
using Pharos.Infrastructure.Net.Http.RestClient;
using Pharos.IServices.System;
using Pharos.OrderSystem.Services.Exceptions;
using System.Collections.Generic;

namespace Pharos.OrderSystem.Services.Common
{
    public static class POSRestClient
    {
        public static string Token { get; set; }
        public static ApiRetrunResult<TResult> Post<TParameter, TResult>(string apiUrl, TParameter parameter, Dictionary<string, string> uriParameters = null)
            where TResult : class
        {
            RequestSetting setting = LoadDefaultSetting(
                new RequestSettingWithJsonContent<TParameter>(parameter)
                {
                    Method = "POST"
                },
                apiUrl);
            if (uriParameters == null)
            {
                uriParameters = new Dictionary<string, string>();
            }
            if (!string.IsNullOrWhiteSpace(Token))
            {
                uriParameters.Add("token", Token);
            }
            setting = setting.SetUriParameters(uriParameters);
            return GetResult<ApiRetrunResult<TResult>>(setting);
        }

        public static ApiRetrunResult<TResult> Post<TResult>(string apiUrl, Dictionary<string, string> parameters = null)
    where TResult : class
        {
            var setting = LoadDefaultSetting(
                new RequestSetting()
                {
                    Method = "POST"
                },
                apiUrl);
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            if (!string.IsNullOrWhiteSpace(Token))//加载默认参数
            {
                parameters.Add("token", Token);
            }
            setting = setting.SetUriParameters(parameters);
            return GetResult<ApiRetrunResult<TResult>>(setting);
        }

        private static TResult GetResult<TResult>(RequestSetting setting)
    where TResult : class
        {
            using (var request = new RestRequest<TResult>(setting))
            {
                if (typeof(TResult) == typeof(string))
                {
                    return request.ExecuteWithString() as TResult;
                }
                else if (typeof(TResult) == typeof(byte[]))
                {
                    return request.ExecuteWithBinary() as TResult;
                }
                else
                    return request.ExecuteWithJsonToObject();
            }
        }



        private static RequestSetting LoadDefaultSetting(RequestSetting setting, string aipUrl)
        {
            if (setting == null)
                setting = new RequestSetting();
            //设置uri 信息等默认配置
            IPosSettingService settingService = new SettingService();
            var systemSettings = settingService.GetSystemSettings();
            if (systemSettings == null)
            {
                throw new NetException("系统配置不完善，请先配置系统设置项目！");
            }
            var serverConfig = systemSettings.GetCurrentServer();
            if (serverConfig == null && !serverConfig.Verfy())
            {
                throw new NetException("服务访问失败，请检查相关服务地址是否配置正确！");
            }
            setting.Host = serverConfig.Host;
            setting.Port = serverConfig.Port;
            setting.Schema = serverConfig.Schema;
            setting.Path = aipUrl;
            return setting;
        }
    }
}
