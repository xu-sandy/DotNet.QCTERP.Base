using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qct.IRepository;
using Qct.Repository.Pos;

namespace Qct.POS.Test
{
    [TestClass]
    public class DeviceTest
    {
        [TestMethod]
        public void SaveDeviceInfo()
        {
            IPosDeviceRepository posDeviceRepository = new PosDeviceRepository();
            posDeviceRepository.Save(new Settings.POSDeviceInformation() { CompanyId = 1, CompanyName = "hsaidohsa", CompanyShorterName = "sadas", DeviceSn = "FCAA147DBADC", MachineSn = "01", StoreId = "16", StoreName = "sadasd" });
        }
    }
}
