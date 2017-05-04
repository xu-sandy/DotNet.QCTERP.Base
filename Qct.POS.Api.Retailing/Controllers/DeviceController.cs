using Qct.Infrastructure.Exceptions;
using Qct.IRepository;
using Qct.ISevices.Systems;
using Qct.Objects.Entities.Systems;
using Qct.Objects.ValueObjects.Systems;
using System;
using System.Web.Http;

namespace Qct.POS.Api.Retailing.Controllers
{
    /// <summary>
    /// 设备管理
    /// </summary>
    public class DeviceController : ApiController
    {
        IDeviceRepository deviceRepository;
        IDeviceService deviceService;
        /// <summary>
        /// 设备管理构造器
        /// </summary>
        /// <param name="_deviceRepository">设备仓储</param>
        /// <param name="_deviceService">设备服务</param>
        public DeviceController(IDeviceRepository _deviceRepository, IDeviceService _deviceService)
        {
            deviceRepository = _deviceRepository;
            deviceService = _deviceService;
        }
        /// <summary>
        /// 设备注册
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="storeId">门店Id</param>
        /// <param name="deviceSn">设备标识</param>
        /// <param name="securityCode">安全码</param>
        /// <param name="deviceType">设备类型</param>
        /// <returns>返回设备编码</returns>
        public string Post(int companyId, string storeId, string deviceSn, string securityCode, DeviceType deviceType)
        {
            try
            {
                var info = deviceService.DecryptSecurityCode(securityCode);
                if (info == null)
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new QCTException("无法通过设备安全码验证！"); 
            }

            var machineSns = deviceRepository.GetMachineSns(companyId, storeId);
            var machineSn = deviceService.CreateMachineSn(machineSns, companyId, storeId);
            deviceRepository.CreateWithSaveChanges(
                new DeviceRegInfo()
                {
                    CompanyId = companyId,
                    CreateDT = DateTime.Now,
                    DeviceSN = deviceSn,
                    SecurityCode = securityCode,
                    State = 0,
                    StoreId = storeId,
                    Type = deviceType,
                    MachineSN = machineSn
                });
            return machineSn;
        }
    }
}
