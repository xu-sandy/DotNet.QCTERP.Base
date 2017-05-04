using Microsoft.Win32;
using Qct.Infrastructure.Json;
using Qct.Infrastructure.Security;
using Qct.IRepository.Exceptions;
using Qct.IRepository;
using Qct.IServices;
using Qct.Settings;
using Qct.Repository.Pos.Common;
using Qct.Repository.Pos;
using System;
using System.Collections.Generic;
using Qct.Objects.ValueObjects;
using Qct.Objects.ValueObjects.Systems;

namespace Qct.Services
{
    public class SettingService : IPosSettingService
    {


        #region DeviceInformation POS设备设置
        /// <summary>
        /// 设备注册
        /// </summary>
        /// <param name="certificate">门店证书</param>
        /// <returns>设备注册信息</returns>
        public POSDeviceInformation DeviceRegister(string certificate)
        {
            var securityCode = DES.DESDecryptBase64WithKeyIVToMd5Base64(certificate, ConstValues.DESKEY, ConstValues.DESKEY);
            var storeInfo = JsonHelper.ToObject<StoreInformation>(securityCode);
            var deviceInfo = new POSDeviceInformation(storeInfo);
            var deviceSn = GetDeviceSn();
            var needSaveDeviceSn = false;
            if (string.IsNullOrWhiteSpace(deviceSn))
            {
                deviceSn = CreateDeviceSn();
                needSaveDeviceSn = true;
            }
            var machineSn = DeviceRegisterToServer(storeInfo.CompanyId, storeInfo.StoreId, deviceSn, certificate, DeviceType.PC);
            if (string.IsNullOrWhiteSpace(machineSn))
            {
                throw new SettingException("未能正确从后台获取设备编号！");
            }
            if (needSaveDeviceSn)
            {
                BuildDeviceSn(deviceSn);
            }
            deviceInfo.SetMachine(machineSn, deviceSn);
            IPosDeviceRepository posDeviceRepository = new PosDeviceRepository();
            posDeviceRepository.Save(deviceInfo);
            return deviceInfo;
        }

        private string DeviceRegisterToServer(int companyId, string storeId, string deviceSn, string securityCode, DeviceType deviceType)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("deviceSn", deviceSn);
            parameters.Add("storeId", storeId);
            parameters.Add("companyId", companyId.ToString());
            parameters.Add("securityCode", securityCode);
            parameters.Add("deviceType", ((short)deviceType).ToString());
            var result = POSRestClient.Post<string>("Api/Device", parameters);
            if (result.Successed)
            {
                return result.Data;
            }
            else
            {
                throw new SettingException(result.Message);
            }
        }
        public string CreateDeviceSn()
        {
            return Guid.NewGuid().ToString("N");
        }
        public string BuildDeviceSn(string deviceSn)
        {
            if (string.IsNullOrWhiteSpace(deviceSn))
            {
                throw new SettingException("设备标识不能为空！");
            }
            RegistryKey key = Registry.LocalMachine;
            RegistryKey software = key.CreateSubKey("software\\QCT\\PointOfSaleSoftware");
            software.SetValue("DeviceSn", deviceSn, RegistryValueKind.String);
            software.Close();
            key.Close();
            return deviceSn;
        }

        public string GetDeviceSn(bool isNotFoundThrowException = false)
        {
            object result = null;
            RegistryKey key = Registry.LocalMachine;
            RegistryKey software = key.OpenSubKey("software\\QCT\\PointOfSaleSoftware", false);
            if (software != null)
            {
                result = software.GetValue("DeviceSn");
                software.Close();
            }
            key.Close();
            if (result == null && isNotFoundThrowException)
            {
                throw new SettingException("设备唯一标识读取失败，请确认设备是否已经激活！");
            }
            return result?.ToString();
        }
        #endregion DeviceInformation 设备设置

    }
}
