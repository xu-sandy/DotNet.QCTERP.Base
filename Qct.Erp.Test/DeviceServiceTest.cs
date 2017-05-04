using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qct.ISevices.Systems;
using Qct.Objects.ValueObjects;
using Qct.Services.Systems;
using Qct.Infrastructure.Json;
using Qct.Infrastructure.Security;
using Qct.Services;

namespace Qct.Erp.Test
{
    [TestClass]
    public class DeviceServiceTest
    {
        [TestMethod]
        public void GetSecurityCode()
        {
            IDeviceService ds = new DeviceService();
            var code =ds.GetSecurityCode(new StoreInformation() { CompanyId = 104, CompanyName = "专业测试公司", CompanyShorterName = "专业测试公司", StoreId = "16", StoreName = "专业测试公司18门店", Timestamp = DateTime.Now });
            Assert.IsNotNull(code);
        }
    }
}
