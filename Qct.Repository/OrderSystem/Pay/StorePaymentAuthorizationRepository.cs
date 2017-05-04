using Qct.Domain.Objects;
using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.Objects.Entities;
using Qct.Persistance.Data;

namespace Qct.Persistance.Repositories
{
    public class StorePaymentAuthorizationRepository : EFRepositoryWithIntegerIdEntity<StorePaymentAuthorization>
    {
        public StorePaymentAuthorizationRepository() : base(DataContextFactory.Create<DataContext>())
        { }
    }
}
