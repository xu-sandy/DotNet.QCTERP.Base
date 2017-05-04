using Qct.Infrastructure.Net.Http.RestClient;
using Qct.IRepository;
using Qct.Objects.ValueObjects;
using Qct.Repository.Pos.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace Qct.Repository.Pos.Common
{
    public static class POSRestClient
    {
        private static string Token { get; set; }
        private static CookieContainer Cookies { get; set; }
        public static void SetToken(string token)
        {
            Token = token;
        }
        /// <summary>
        /// Delete 请求
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="parameter"></param>
        /// <param name="uriParameters"></param>
        /// <returns></returns>
        public static OperateResult<TResult> Delete<TParameter, TResult>(string apiUrl, TParameter parameter, Dictionary<string, string> uriParameters = null)
        {
            try
            {
                RequestSetting setting = LoadDefaultSetting(
                    new RequestSettingWithJsonContent<TParameter>(parameter)
                    {
                        Method = "DELETE"
                    },
                    apiUrl);
                if (uriParameters == null)
                {
                    uriParameters = new Dictionary<string, string>();
                }
                if (!string.IsNullOrWhiteSpace(Token))
                {
                    setting.Headers.Add(HttpRequestHeader.Authorization, string.Format("Basic {0}", Token));
                }
                setting = setting.SetUriParameters(uriParameters);
                return GetResult<OperateResult<TResult>>(setting);
            }
            catch (Exception ex)
            {
                //   LoggerFactory.Create(ERPModule.POSClient.GetModuleName(), ).Warn("网络请求发生错误！", ex);
                return new OperateResult<TResult>() { Code = "500", Message = "网络请求发生错误，请检查网络是否正常！" + ex.Message };
            }
        }
        /// <summary>
        /// Delete 请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static OperateResult<TResult> Delete<TResult>(string apiUrl, Dictionary<string, string> parameters = null)
        {
            try
            {
                var setting = LoadDefaultSetting(
                new RequestSetting()
                {
                    Method = "DELETE"
                },
                apiUrl);
                if (parameters == null)
                {
                    parameters = new Dictionary<string, string>();
                }
                if (!string.IsNullOrWhiteSpace(Token))//加载默认参数
                {
                    setting.Headers.Add(HttpRequestHeader.Authorization, string.Format("Basic {0}", Token));
                }
                setting = setting.SetUriParameters(parameters);
                return GetResult<OperateResult<TResult>>(setting);
            }
            catch (Exception ex)
            {
                // LoggerFactory.Create(ERPModule.POSClient.GetModuleName(), LoggerType.Log4net).Warn("网络请求发生错误！", ex);
                return new OperateResult<TResult>() { Code = "500", Message = "网络请求发生错误，请检查网络是否正常！" + ex.Message };
            }
        }

        /// <summary>
        /// POST 请求
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="parameter"></param>
        /// <param name="uriParameters"></param>
        /// <returns></returns>
        public static OperateResult<TResult> Post<TParameter, TResult>(string apiUrl, TParameter parameter, Dictionary<string, string> uriParameters = null)
        {
            try
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
                    setting.Headers.Add(HttpRequestHeader.Authorization, string.Format("Basic {0}", Token));
                }
                setting = setting.SetUriParameters(uriParameters);
                return GetResult<OperateResult<TResult>>(setting);
            }
            catch (Exception ex)
            {
                //   LoggerFactory.Create(ERPModule.POSClient.GetModuleName(), ).Warn("网络请求发生错误！", ex);
                return new OperateResult<TResult>() { Code = "500", Message = "网络请求发生错误，请检查网络是否正常！" + ex.Message };
            }
        }
        /// <summary>
        /// POST 请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static OperateResult<TResult> Post<TResult>(string apiUrl, Dictionary<string, string> parameters = null)
        {
            try
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
                    setting.Headers.Add(HttpRequestHeader.Authorization, string.Format("Basic {0}", Token));
                }
                setting = setting.SetUriParameters(parameters);
                return GetResult<OperateResult<TResult>>(setting);
            }
            catch (Exception ex)
            {
                // LoggerFactory.Create(ERPModule.POSClient.GetModuleName(), LoggerType.Log4net).Warn("网络请求发生错误！", ex);
                return new OperateResult<TResult>() { Code = "500", Message = "网络请求发生错误，请检查网络是否正常！" + ex.Message };
            }
        }

        private static TOperateResult GetResult<TOperateResult>(RequestSetting setting)
    where TOperateResult : class
        {
            using (var request = new RestRequest<TOperateResult>(setting))
            {
                if (Cookies != null && Cookies.Count > 0)
                {
                    request.SetRequsetCookie(Cookies);
                }
                var result = request.ExecuteWithJsonToObject();
                var cookies = request.GetResponeCookie();
                if (cookies != null && cookies.Count > 0)
                {
                    Cookies = cookies;
                }
                return result;
            }
        }



        private static RequestSetting LoadDefaultSetting(RequestSetting setting, string aipUrl)
        {
            if (setting == null)
                setting = new RequestSetting();
            //设置uri 信息等默认配置

            IPosSystemSettingsRepository systemSettingsRepository = new PosSystemSettingsRepository();
            var systemSettings = systemSettingsRepository.Get();
            if (systemSettings == null)
            {
                throw new NetException("系统配置不完善，请先配置系统设置项目！");
            }
            var serverConfig = systemSettings.RemoteServer;
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
