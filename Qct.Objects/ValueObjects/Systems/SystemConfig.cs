using Qct.Infrastructure.Extensions;
using Qct.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects
{
    public class SystemConfig
    {
        public static string Page_Title
        {
            get { return "总部后台管理"; }
        }
        public static int SuperRoleId
        {
            get
            {
                var roleId = ConfigHelper.GetAppSettings("superRoleId");
                if (roleId.IsNullOrEmpty())
                    roleId = "9";
                return int.Parse(roleId);
            }
        }
        public static int AdminRoleId
        {
            get
            {
                var roleId = ConfigHelper.GetAppSettings("adminRoleId");
                if (roleId.IsNullOrEmpty())
                    roleId = "4";
                return int.Parse(roleId);
            }
        }
    }
}
