using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qct.IRepository;
using Qct.Repository.Pos;
using Qct.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.POS.Test
{
    [TestClass]
    public class StoreUserServiceTest
    {
        [TestMethod]
        public void TestLoginAndLogout()
        {

            StoreUserService storeUserService = new StoreUserService();
           var data = storeUserService.Login("1008", "123456", true);
            storeUserService.Logout();
        }
    }
}
