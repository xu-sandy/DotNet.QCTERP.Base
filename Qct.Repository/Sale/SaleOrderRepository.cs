using Qct.IRepository;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Repository
{
    public class SaleOrderRepository : BaseEFRepository<SaleOrders>, ISaleOrderRepository
    {
        public Dictionary<string, int> GetMaxNumByDate(string[] dates)
        {
            var sql = string.Format(@"SELECT CAST(MAX(sort) AS INT) maxnum,t.prefix FROM(
				 SELECT SUBSTRING(CustomOrderSn,11,4) sort,SUBSTRING(CustomOrderSn,0,11) prefix FROM dbo.SaleOrders WHERE CompanyId={0} AND MachineSN='00' and CONVERT(VARCHAR(10),CreateDT,120) IN({1})
				) t GROUP BY prefix", CompanyId, string.Join(",", dates.Select(o => "'" + o + "'")));
            var dicts= _context.ExecuteQuery<dynamic>(sql).ToDictionary(o => o.maxnum, o => o.prefix);
            return null;
        }

    }
}
