using Qct.Domain.Objects;
using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.Objects.ValueObjects;
using Qct.Persistance.Data;
using Qct.Repository.Exceptions;
using System.Linq;

namespace Qct.Persistance.Repositories
{
    public class MembershipCardRepository : EFRepositoryWithIntegerIdEntity<MembershipCard>
    {
        public MembershipCardRepository() : base(DataContextFactory.Create<DataContext>())
        {
        }
        public MembershipCard FindMembershipCard(int companyId, string cardSn, bool notFindThrowException = false)
        {

            var card = GetReadOnlyEntities().FirstOrDefault(o => o.CompanyId == companyId && o.CardSN == cardSn);
            if (notFindThrowException && card == null)
            {
                throw new NotFoundMembershipCardException(string.Format(ConstValues.StoredValueCardPayCardNotFind, cardSn));
            }
            return card;
        }
    }
}
