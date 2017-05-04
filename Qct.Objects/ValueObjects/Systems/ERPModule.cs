using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects
{
    public enum ERPModule
    {
        [Description("POS客户端")]
        POSClient,
    }

    public static class ERPModuleExtensions
    {
        public static string GetModuleName(this ERPModule module)
        {
            var enumType = module.GetType();
            string name = Enum.GetName(enumType, module);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return string.Empty;
        }
    }
}
