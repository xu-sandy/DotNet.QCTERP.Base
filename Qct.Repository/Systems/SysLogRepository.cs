using Qct.IRepository;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using Qct.Infrastructure.Data.Extensions;
using Qct.Infrastructure.Extensions;

namespace Qct.Repository.Systems
{
    public class SysLogRepository : BaseEFRepository<SysLog>, ISysLogRepository
    {
        public void DeleteAll()
        {
            string sql = "delete from SysLog where companyId=" + CompanyId;
            _context.ExecuteSqlCommand(sql);
        }

        public void DeleteIds(int[] ids)
        {
            var list = GetEntities().Where(o => ids.Contains(o.Id)).ToArray();
            base.RemoveWithSaveChanges(list);
        }

        public PageInformaction FindPageList(NameValueCollection nvl)
        {
            SqlParameter[] parms = {
                    new SqlParameter("@startDate", nvl["date"]),
                    new SqlParameter("@endDate", nvl["date2"]),
                    new SqlParameter("@Type", nvl["date"].ToType<int>()),
                    new SqlParameter("@Key", nvl["keyword"].ToTrim()),
                    new SqlParameter("@CurrentPage", nvl["page"]),
                    new SqlParameter("@PageSize", nvl["rows"]),
                    new SqlParameter("@CompanyId",CompanyId)
            };
            var dt= GetDataTableBySql("Sys_LogList @startDate,@endDate,@Type,@Key,@CurrentPage,@PageSize,@CompanyId", parms);
            var page = new PageInformaction();
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("RecordTotal"))
            {
                page.CollectinSize= Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                dt.Columns.Remove("RecordTotal");
            }
            page.Datas = new List<DataTable>() { dt };
            return page.ToPageCount();
        }
    }
}
