using Qct.POS.Api.Retailing.Filters;
using System.Web;
using System.Web.Mvc;

namespace Qct.POS.Api.Retailing
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
        
            filters.Add(new HandleErrorAttribute());
        }
    }
}
