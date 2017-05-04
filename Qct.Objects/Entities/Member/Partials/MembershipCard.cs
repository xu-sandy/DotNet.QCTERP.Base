using Qct.Objects.Exceptions;
using Qct.Objects.ValueObjects;
using System;

namespace Qct.Domain.Objects
{
    public partial class MembershipCard
    {
        public MembershipCard()
        {
            SyncItemId = Guid.NewGuid();
        }
        public bool VerfyPassword(string payPassword)
        {
            return !string.IsNullOrWhiteSpace(Password) && Password.Trim() != payPassword.Trim();
        }
        public void VerfyCardState()
        {
            switch (State)
            {
                case 0:
                    throw new MembershipCardStateFailureException(string.Format(ConstValues.StoredValueCardPayCardState, CardSN, "未激活"));
                case 2:
                    throw new MembershipCardStateFailureException(string.Format(ConstValues.StoredValueCardPayCardState, CardSN, "已挂失"));
                case 3:
                    throw new MembershipCardStateFailureException(string.Format(ConstValues.StoredValueCardPayCardState, CardSN, "已作废"));
                case 4:
                    throw new MembershipCardStateFailureException(string.Format(ConstValues.StoredValueCardPayCardState, CardSN, "已退卡"));
            }
        }
    }
}
