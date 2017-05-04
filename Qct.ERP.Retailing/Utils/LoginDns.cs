using Qct.Infrastructure.Extensions;
using Qct.Infrastructure.Helpers;
using Qct.Objects.ValueObjects;
using System;

namespace Qct.ERP.Retailing
{
    public class LoginDns
    {
        /// <summary>
        /// 获取CID
        /// </summary>
        /// <returns>-2是请求API发生错误，-1是输入的二级域名是空，0是输入的域名不存在商户，大于0是输入的域名存在商户,-3是输入的域名是保留二级域名</returns>
        public static int GetCID(string dom)
        {
            //二级域名
            if (!dom.IsNullOrEmpty())
            {
                var omsurl = ConfigHelper.GetAppSettings("omsurl") + "api/OuterApi/GetCIDByRealm";
                string v = HttpHelper.HttpGet(omsurl, "name=" + dom); 
                if (v == "error")
                {
                    //请求API发生错误
                    return -2;
                }
                else if (v == "-1")
                {
                    //输入的域名是保留二级域名
                    return -3;
                }
                else if (v == "0" || v.IsNullOrEmpty())
                {
                    //输入的域名不存在商户
                    return 0;
                }
                else
                {
                    //输入的域名存在商户
                    return Convert.ToInt32(v);
                }
            }
            //输入的二级域名是空
            else
            {
                return -1;
            }
        }


        /// <summary>
        /// 获取商户号和门店ID（域名）
        /// </summary>
        /// <param name="cidSid"></param>
        /// <returns></returns>
        public LoginDnsModel GetCidSid(string s_cid, string s_sid)
        {
            //测试
            bool aa = IsNumeric("0");

            bool bb = IsNumeric("1");
            LoginDnsModel csid = new LoginDnsModel();

            string Cid = s_cid;
            string Sid = s_sid;

            //格式错误
            if (string.IsNullOrEmpty(Cid) || string.IsNullOrEmpty(Sid))
            {
                csid.Message = "格式错误";
                return csid;
            }
            else
            {
                //Cid或者Sid没有大于0
                if (!IsNumeric(Cid) || !IsNumeric(Sid))
                {
                    csid.Message = "域名的store后面必须是数字";
                    return csid;
                }
                else
                {
                    csid.Message = "success";
                    csid.Cid = Cid;
                    csid.Sid = Sid;
                    return csid;
                }
            }
        }

        /// <summary>
        /// localhost门店登录、ip门店登录
        /// </summary>
        public LoginDnsModel ipLocalhostStore(string localhost, string ip1, string ip2, string ip3, string ip4, string Cid, string Sid)
        {
            LoginDnsModel csid = new LoginDnsModel();
            csid.Cid = Cid;
            csid.Sid = Sid;

            //localhost门店登录
            if (!localhost.IsNullOrEmpty())
            {
                if (localhost.ToLower().Contains("localhost"))
                {
                    csid.Message = "localhost";
                }
                else
                {
                    csid.Message = "发生错误";
                }
            }
            //ip门店登录
            else if (!ip1.IsNullOrEmpty() && !ip2.IsNullOrEmpty() && !ip3.IsNullOrEmpty() && !ip4.IsNullOrEmpty())
            {
                csid.Message = "ip";
            }
            //禁止访问（比如：erp.qcterp.com/store3-1）
            else if (!ip1.IsNullOrEmpty() && !ip2.IsNullOrEmpty() && !ip3.IsNullOrEmpty() && ip4.IsNullOrEmpty())
            {
                csid.Message = "禁止访问";
            }
            else
            {
                csid.Message = "发生错误";
            }
            return csid;
        }

        ///验证输入的数据是不是数字
        ///传入字符串
        ///返回true或者false
        public bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIP(string ip)
        {
            //判断
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}