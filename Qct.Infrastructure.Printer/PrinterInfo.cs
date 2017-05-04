using System.Collections.Generic;
using System.Linq;
using System.Printing;

namespace Qct.Infrastructure.Printer
{
    public static class PrinterInfo
    {
        /// <summary>
        /// 打印服务器对象
        /// </summary>
        private static LocalPrintServer _printServer;
        /// <summary>
        /// 获取打印服务器
        /// </summary>
        internal static LocalPrintServer PrintServer
        {
            get { return _printServer ?? (_printServer = new LocalPrintServer()); }
        }
        /// <summary>
        /// 打印机列表缓存
        /// </summary>
        private static PrintQueueCollection _printers;
        /// <summary>
        /// 获取打印机列表
        /// </summary>
        internal static PrintQueueCollection Printers
        {
            get
            {
                return _printers ?? (_printers = PrintServer.GetPrintQueues(new[]
                                                                            {
                                                                                EnumeratedPrintQueueTypes.Local,
                                                                                EnumeratedPrintQueueTypes.Connections
                                                                            }));
            }
        }
        /// <summary>
        /// 根据打印机名称查找打印机
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PrintQueue FindPrinterByName(string name)
        {
            return Printers.FirstOrDefault(x => x.HostingPrintServer.Name + "\\" + x.FullName == name);
        }
        /// <summary>
        /// 获取打印机列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetPrinterNames()
        {
            return Printers.Select(printer => printer.HostingPrintServer.Name + "\\" + printer.FullName).ToList();
        }
        /// <summary>
        /// 重置打印服务器对象
        /// </summary>
        public static void ResetCache()
        {
            _printServer = null;
        }
    }
}
