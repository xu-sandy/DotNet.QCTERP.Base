using System.Collections.Generic;
using System.Windows;

namespace Qct.Infrastructure.Printer.PrintTemplate
{
    public class TemplateItem
    {
        /// <summary>
        /// 绑定模板类型
        /// </summary>
        public TemplateItemType ItemType { get; set; }
        /// <summary>
        /// 绑定文本或者数据源或者文件路径
        /// </summary>
        public string BindingData { get; set; }
        /// <summary>
        /// 字体
        /// </summary>
        public string FontFamily { get; set; }
        /// <summary>
        /// 字体大小/横线高度
        /// </summary>
        public double Size { get; set; }
        /// <summary>
        /// 是否字体样式
        /// </summary>
        public string FontStyle { get; set; }
        /// <summary>
        /// 以笔画的粗细来引用字体的密度。
        /// </summary>
        public string FontWeight { get; set; }
        /// <summary>
        /// 获取或设置元素的外边距。
        /// </summary>
        public Thickness Margin { get; set; }
        /// <summary>
        /// 获取或设置控件内的边距。
        /// </summary>
        public Thickness Padding { get; set; }
        int _InTableColumnSpan;
        /// <summary>
        /// 在table中的宽度占比(默认继承header)
        /// </summary>
        public int InTableColumnSpan
        {
            get
            {
                if (_InTableColumnSpan == 0 && TableHeader != null)
                {
                    return TableHeader.InTableColumnSpan;
                }
                return _InTableColumnSpan;
            }
            set
            {
                _InTableColumnSpan = value;
            }
        }
        /// <summary>
        /// table的头部内容
        /// </summary>
        public TemplateItem TableHeader { get; set; }

        public List<TemplateItem> Children { get; set; }
    }
}
