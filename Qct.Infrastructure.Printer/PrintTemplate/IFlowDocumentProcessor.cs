using System.Windows.Documents;

namespace Qct.Infrastructure.Printer.PrintTemplate
{
    public interface IFlowDocumentProcessor
    {
        /// <summary>
        /// 创建打印文档
        /// </summary>
        /// <param name="modelProcessor">模型处理器</param>
        /// <param name="tpl">打印模板</param>
        /// <param name="model">数据模型</param>
        /// <returns>返回打印文档</returns>
        FlowDocument CreateDocument(IModelProcessor modelProcessor, Template tpl, object model);
    }
}
