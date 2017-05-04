using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Infrastructure.Data;
using Qct.Objects.Entities;

namespace Qct.IRepository
{
    public interface ISaleOrderRepository:IEFRepository<SaleOrders>
    {
        /// <summary>
        /// 获取最大流入号
        /// </summary>
        /// <param name="dates">日期</param>
        /// <returns></returns>
        Dictionary<string, int> GetMaxNumByDate(string[] dates);
    }
}
