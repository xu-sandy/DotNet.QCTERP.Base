using Qct.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Objects.Entities;

namespace Qct.Repository.Sale
{
    public class ConsumptionPaymentRepository : BaseEFRepository<ConsumptionPayment>, IConsumptionPaymentRepository
    {
        public List<ConsumptionPayment> GetListBysn(string[] apiOrderSns)
        {
            return GetEntities().Where(o => apiOrderSns.Contains(o.ApiOrderSN)).ToList();
        }
    }
}
