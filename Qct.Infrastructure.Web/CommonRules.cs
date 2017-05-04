using System;
using System.Web;
using Qct.Infrastructure.Extensions;

namespace Qct.Infrastructure.Web
{
    public class CommonRules
    {
        public static int CompanyId
        {
            get
            {
                var cid = "";
                try
                {
                    var item = Convert.ToString(HttpContext.Current.Items["CID"]);
                    var companyId = !item.IsNullOrEmpty() ? item : (HttpContext.Current.Request["CID"] ?? CurrentUser.CID);
                    if (!companyId.IsNullOrEmpty())
                        cid = companyId;
                }
                catch { }
                if (cid.IsNullOrEmpty())
                    cid = Config.GetAppSettings("CompanyId");
                cid = cid.Replace(",", "");
                int c = 0;
                int.TryParse(cid, out c);
                if (c <= 0) throw new Exception("企业ID不存在！");
                return c;
            }
        }
        /// <summary>
        /// 生成新的GUID
        /// </summary>
        public static string GUID
        {
            get { return Guid.NewGuid().ToString().Replace("-", ""); }
        }
        public static int SuperRoleId
        {
            get
            {
                var roleId = Config.GetAppSettings("superRoleId");
                if (roleId.IsNullOrEmpty())
                    roleId = "9";
                return int.Parse(roleId);
            }
        }
        public static int AdminRoleId
        {
            get
            {
                var roleId = Config.GetAppSettings("adminRoleId");
                if (roleId.IsNullOrEmpty())
                    roleId = "4";
                return int.Parse( roleId);
            }
        }

    }
}
