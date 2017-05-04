using Qct.Infrastructure.Extensions;
using Qct.Objects.ValueObjects;
using Qct.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Qct.Services
{
    public class Company : ICompany
    {
        public int GetCompanyId()
        {
            var cid = "";
            try
            {
                var item = Convert.ToString(HttpContext.Current.Items["CID"]);
                var companyId = !item.IsNullOrEmpty() ? item : HttpContext.Current.Request["CID"];
                if(companyId.IsNullOrEmpty())
                {
                    var user= SysUserService.GetCurrentUser();
                    if (user != null) companyId = user.CompanyId.ToString();
                }

                if (!companyId.IsNullOrEmpty())
                    cid = companyId;
            }
            catch { }
            cid = cid.Replace(",", "");
            int c = 0;
            int.TryParse(cid, out c);
            return c;
        }
    }
}
