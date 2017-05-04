using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace Qct.Infrastructure.Printer.PrintTemplate
{
    public class DefaultModelProcessor : IModelProcessor
    {
        /// <summary>
        /// 获取模板绑定的数据
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual string GetBindingValue(string binding, object model)
        {
            return Regex.Replace(binding, "##.*?##", (o) =>
             {
                 var bindingText = o.Value.Substring(2, o.Value.Length - 4);
                 var formatText = string.Empty;
                 if (bindingText.Any(p => p == ':'))
                 {
                     var bindings = bindingText.Split(":".ToArray(), StringSplitOptions.None);
                     bindingText = bindings[0];
                     formatText = bindings[1];
                 }
                 return GetModelValue(model, bindingText, formatText);
             });
        }
        /// <summary>
        /// 获取数组、集合的枚举接口
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IEnumerable GetItems(string binding, object model)
        {
            return new object[0];
        }
        /// <summary>
        /// 获取绑定的模型数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bindingText"></param>
        /// <param name="formatText"></param>
        /// <returns></returns>
        public virtual string GetModelValue(object model, string bindingText, string formatText)
        {
            return bindingText;
        }
    }

}
