using Qct.Infrastructure.Data;
using Qct.IServices;
using Qct.Objects.Entities;
using Qct.Repository;

namespace Qct.Services
{
    public class OrderService : IOrderService
    {
        private string GettOrderSnIncreasingNumberKey(int companyId, string storeId, string machineSn)
        {
            return string.Format("{0}-{1}-{2}-OrderSnIncreasingNumber", companyId, storeId, machineSn);
        }
        public int GetOrderSnIncreasingNumber(int companyId, string storeId, string machineSn)
        {
            IEFRepository<IncreasingNumber> repository = new IncreasingNumberRepository();
            IncreasingNumber number = repository.Get(GettOrderSnIncreasingNumberKey(companyId, storeId, machineSn));
            return number.Number;
        }

        public void SaveOrderSnIncreasingNumber(int companyId, string storeId, string machineSn, int num)
        {
            IEFRepository<IncreasingNumber> repository = new IncreasingNumberRepository();
            string id = GettOrderSnIncreasingNumberKey(companyId, storeId, machineSn);
            IncreasingNumber number = repository.Get(id);
            if (number != null)
            {
                number.Number = num;
            }
            else
            {
                repository.Create(new IncreasingNumber() { Number = num, Id = id });
            }
            repository.SaveChanges();
        }

        public void InitOrderSnIncreasingNumber(int companyId, string storeId, string machineSn)
        {
            var num = GetOrderSnIncreasingNumber(companyId, storeId, machineSn);
            var orderId = new OrderSn(companyId, storeId, machineSn);
            var currentIncreasingNumber = orderId.GetCurrentIncreasingNumber();
            if (num == 0 && currentIncreasingNumber == 0)
            {
                orderId.NextNumber();
            }
            else
            {
                orderId.ResetNumber(num);
            }
        }
    }
}
