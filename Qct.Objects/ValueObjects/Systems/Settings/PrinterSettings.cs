namespace Qct.Settings
{
    public class PrinterSettings
    {
        /// <summary>
        /// 销售小票打印机是否可用
        /// </summary>
        public bool SalePrinterEnable { get; set; }
        /// <summary>
        /// 销售小票打印机名称
        /// </summary>
        public string SalePrinterName { get; set; }
        /// <summary>
        /// 打印模板路径
        /// </summary>
        public string PrintTemplateFilePath { get; set; }
    }
}
