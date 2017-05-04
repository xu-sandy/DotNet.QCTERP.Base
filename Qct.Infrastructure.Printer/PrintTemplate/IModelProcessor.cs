using System.Collections;

namespace Qct.Infrastructure.Printer.PrintTemplate
{
    public interface IModelProcessor
    {
        /// <summary>
        /// 获取模型绑定数据
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        string GetBindingValue(string binding, object model);
        /// <summary>
        /// 获取数组、集合的枚举接口
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IEnumerable GetItems(string binding, object model);

    }
}
