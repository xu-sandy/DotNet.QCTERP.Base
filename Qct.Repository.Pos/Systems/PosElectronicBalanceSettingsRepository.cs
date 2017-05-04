using Qct.IRepository.Exceptions;
using Qct.IRepository;
using Qct.ElectronicBalance;
using Qct.Settings;
using Qct.Repository.Pos.Common;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Qct.Repository
{
    public class PosElectronicBalanceSettingsRepository : IPosElectronicBalanceSettingsRepository
    {
        private readonly string ConfigFilePath = SettingsPathHelper.GetOrCreateSettingsPath();
        private const string ElectronicBalanceSettingFileName = "ElectronicBalance.Config";

        public ElectronicBalanceSetting Get(bool isNotFoundReturnDefault = false)
        {
            try
            {
                var fileName = Path.Combine(ConfigFilePath, ElectronicBalanceSettingFileName);

                if (File.Exists(fileName))
                {
                    var doc = XDocument.Load(fileName);
                    var root = doc.Element("ElectronicBalanceSetting");

                    var patternSettingItemType = typeof(PatternSettingItem);
                    var settings = root.Element("PatternSettings").Attributes().ToDictionary(o => (PatternSettingItem)Enum.Parse(patternSettingItemType, o.Name.LocalName), o => o.Value);
                    var result = new ElectronicBalanceSetting()
                    {
                        Enable = bool.Parse(root.Attribute("Enable").Value),
                        WorkPattern = (ElectronicBalanceWorkPattern)Enum.Parse(typeof(ElectronicBalanceWorkPattern), root.Attribute("WorkPattern").Value),
                        PatternSettings = settings
                    };
                    return result;
                }
                if (isNotFoundReturnDefault)
                    return ElectronicBalanceSetting.Default;
                return null;
            }
            catch (Exception)
            {
                throw new SettingException("读取电子秤设置时出错，请检查配置是否正确！");
            }
        }

        public void Save(ElectronicBalanceSetting setting)
        {
            var fileName = Path.Combine(ConfigFilePath, ElectronicBalanceSettingFileName);

            XElement root = new XElement("ElectronicBalanceSetting");
            root.SetAttributeValue(XName.Get("Enable"), setting.Enable);
            root.SetAttributeValue(XName.Get("WorkPattern"), setting.WorkPattern);

            var patternSettingsElement = new XElement("PatternSettings");
            foreach (var item in setting.PatternSettings)
            {
                var element = new XElement(item.Key.ToString("f"), item.Value);
                patternSettingsElement.Add(element);
            }
            root.Add(patternSettingsElement);
            root.Save(fileName);
        }
    }
}
