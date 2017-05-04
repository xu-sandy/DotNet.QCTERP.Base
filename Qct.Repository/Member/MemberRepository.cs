using Qct.Domain.Objects;
using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.Persistance.Data;
using System.Linq;

namespace Qct.Persistance.Repositories
{
    public class MemberRepository : EFRepositoryWithIntegerIdEntity<Members>
    {
        public MemberRepository() : base(DataContextFactory.Create<DataContext>())
        {
        }
        public Members FindReadOnlyMember(string memberId)
        {
            return GetReadOnlyEntities().FirstOrDefault(o => o.MemberId == memberId);
        }

        public Members FindMember(string memberId)
        {
            return GetEntities().FirstOrDefault(o => o.MemberId == memberId);
        }
    }
}
