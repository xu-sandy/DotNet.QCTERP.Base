using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.Objects.Entities;
using Qct.Pay;
using Qct.Persistance.Data;
using Qct.Repository.Exceptions;
using System.Linq;

namespace Qct.Persistance.Repositories
{
    public class PayConfigurationRepository : EFRepositoryWithIntegerIdEntity<PayConfiguration>
    {
        public PayConfigurationRepository() : base(DataContextFactory.Create<DataContext>())
        { }

        public PayConfiguration FindByCompanyId(int companyId, PayType type, bool isNotFindThrowException = false)
        {
            var result = GetReadOnlyEntities().FirstOrDefault(o => o.CompanyId == companyId && o.PayType == type);
            if (isNotFindThrowException && result == null)
                throw new NotFoundPayConfigurationException("支付失败，未找到支付配置信息！");
            return result;
        }
    }
}
