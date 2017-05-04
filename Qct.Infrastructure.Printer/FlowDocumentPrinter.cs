using System;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Qct.Infrastructure.Printer
{
    public class FlowDocumentPrinter
    {
        private PrintQueue currentQueue;
        /// <summary>
        /// 传入打印队列
        /// </summary>
        /// <param name="printQueue"></param>
        public FlowDocumentPrinter(PrintQueue printQueue)
        {
            if (printQueue == null)
                throw new ArgumentNullException("打印队列不能为空！");
            currentQueue = printQueue;
        }
        /// <summary>
        /// 打印FlowDocument
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="docTitle"></param>
        public void Print(FlowDocument doc, string docTitle = "Pharos POS Printer Document")
        {
            if (doc == null)
                return;
            var printDialog = new PrintDialog { PrintQueue = currentQueue };
            doc.PageWidth = printDialog.PrintableAreaWidth;
            doc.MaxPageHeight = printDialog.PrintableAreaHeight;
            printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, docTitle);
        }
    }
}
