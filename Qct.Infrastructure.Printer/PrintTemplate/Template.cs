using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace Qct.Infrastructure.Printer.PrintTemplate
{
    public class Template
    {
        public Template()
        {
            Items = new List<TemplateItem>();
        }
        /// <summary>
        /// 字体
        /// </summary>
        public string FontFamily { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public double FontSize { get; set; }
        /// <summary>
        /// 字体样式
        /// </summary>
        public string FontStyle { get; set; }
        /// <summary>
        /// 以笔画的粗细来引用字体的密度。
        /// </summary>
        public string FontWeight { get; set; }
        /// <summary>
        /// 获取或设置控件内的边距。
        /// </summary>
        public Thickness Padding { get; set; }
        /// <summary>
        /// 文档行高
        /// </summary>
        public double LineHeight { get; set; }
        /// <summary>
        /// 模板详细
        /// </summary>
        public List<TemplateItem> Items { get; set; }

    }

}
