using Qct.IRepository;
using Qct.Settings;
using Qct.IRepository.Exceptions;
using System.IO;
using System.Xml.Linq;
using Qct.Repository.Pos.Common;

namespace Qct.Repository.Pos
{
    public class PosDeviceRepository : IPosDeviceRepository
    {
        public PosDeviceRepository()
        {

        }
        private readonly string ConfigFilePath = SettingsPathHelper.GetOrCreateSettingsPath();

        /// <summary>
        /// 门店配置文件名称
        /// </summary>
        private const string DeviceSettingFileName = "Device.Config";
        public POSDeviceInformation Get(bool isNotFoundThrowException = false)
        {
            var fileName = Path.Combine(ConfigFilePath, DeviceSettingFileName);
            try
            {
                if (File.Exists(fileName))
                {
                    var doc = XDocument.Load(fileName);
                    var root = doc.Element("DeviceSetting");
                    return new POSDeviceInformation()
                    {
                        CompanyId = int.Parse(root.Element("CompanyId").Value),
                        CompanyName = root.Element("CompanyName").Value,
                        CompanyShorterName = root.Element("CompanyShorterName").Value,
                        DeviceSn = root.Element("DeviceSn").Value,
                        MachineSn = root.Element("MachineSn").Value,
                        StoreId = root.Element("StoreId").Value,
                        StoreName = root.Element("StoreName").Value
                    };
                }
                else if (isNotFoundThrowException)
                {
                    throw new SettingException("未找到设备配置信息，请确认设备是否已经激活！");
                }
                return null;
            }
            catch
            {
                throw new SettingException("读取设备配置时出错，请检查配置是否正确！");
            }
        }

        public void Save(POSDeviceInformation deviceInfo)
        {

            var fileName = Path.Combine(ConfigFilePath, DeviceSettingFileName);

            XElement root = new XElement("DeviceSetting");
            root.Add(new XElement("CompanyId", deviceInfo.CompanyId));
            root.Add(new XElement("CompanyName", deviceInfo.CompanyName));
            root.Add(new XElement("CompanyShorterName", deviceInfo.CompanyShorterName));
            root.Add(new XElement("DeviceSn", deviceInfo.DeviceSn));
            root.Add(new XElement("MachineSn", deviceInfo.MachineSn));
            root.Add(new XElement("StoreId", deviceInfo.StoreId));
            root.Add(new XElement("StoreName", deviceInfo.StoreName));
            root.Save(fileName);
        }
    }
}
