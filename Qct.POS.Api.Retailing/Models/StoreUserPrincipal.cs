using Qct.Domain.CommonObject.User;
using System;
using System.Linq;
using System.Security.Principal;

namespace Qct.POS.Api.Retailing.Models
{
    public class StoreUserPrincipal : IPrincipal
    {
        public StoreUserPrincipal(UserCredentials user)
        {
            Identity = new GenericIdentity(user.FullName, "StoreUserCode");
            UserCredentials = user;
        }

        public UserCredentials UserCredentials { get; private set; }
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            StoreUserRoleType roleType;
            if (Enum.TryParse(role, out roleType))
            {
                return UserCredentials.StoreUserRoles.Contains(roleType);
            }
            return false;
        }
    }
}