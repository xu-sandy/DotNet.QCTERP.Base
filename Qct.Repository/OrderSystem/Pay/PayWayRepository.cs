using Qct.Domain.Objects;
using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.Objects.Entities;
using Qct.Persistance.Data;

namespace Qct.Persistance.Repositories
{
    public class PayWayRepository : EFRepositoryWithIntegerIdEntity<PayWay>
    {
        public PayWayRepository() : base(DataContextFactory.Create<DataContext>())
        { }
    }
}
