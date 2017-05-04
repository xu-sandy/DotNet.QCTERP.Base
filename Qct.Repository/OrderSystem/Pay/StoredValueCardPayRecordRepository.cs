using Qct.Domain.Objects;
using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.Objects.Entities;
using Qct.Pay;
using Qct.Persistance.Data;

namespace Qct.Persistance.Repositories
{
    public class StoredValueCardPayRecordRepository : EFRepositoryWithIntegerIdEntity<StoredValueCardPayRecord>
    {
        public StoredValueCardPayRecordRepository() : base(DataContextFactory.Create<DataContext>())
        { }
    }
}

