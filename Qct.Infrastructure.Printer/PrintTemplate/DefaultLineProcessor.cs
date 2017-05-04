using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Qct.Infrastructure.Printer.PrintTemplate
{
    /// <summary>
    /// 行模板生成器
    /// </summary>
    public class DefaultLineProcessor : IFlowDocumentProcessor
    {
        /// <summary>
        /// 模板格式化为FlowDocument
        /// </summary>
        /// <param name="modelProcessor">数据模型处理器</param>
        /// <param name="tpl">模板</param>
        /// <param name="model">数据模型</param>
        /// <returns>FlowDocument文档</returns>
        public FlowDocument CreateDocument(IModelProcessor modelProcessor, Template tpl, object model)
        {
            if (tpl.Items == null || tpl.Items.Count == 0)
                return null;
            var section = new Section();
            foreach (var item in tpl.Items)
            {
                switch (item.ItemType)
                {
                    case TemplateItemType.Text:
                        {
                            var pagePanel = new Paragraph();

                            var content = CreateTextTemplateItem(item, modelProcessor, model);
                            if (content != null)
                                pagePanel.Inlines.Add(content);
                            section.Blocks.Add(pagePanel);

                        }
                        break;
                    case TemplateItemType.Table:
                        {
                            var table = CreateTableTemplateItem(item, modelProcessor, model);
                            section.Blocks.Add(table);
                        }
                        break;
                    case TemplateItemType.Line:
                        {
                            var line = CreateLineTemplateItem(item);
                            section.Blocks.Add(line);
                        }
                        break;
                    case TemplateItemType.Image:
                        {
                            var pagePanel = new Paragraph();

                            var img = CreateImageTemplateItem(item);
                            if (img != null)
                                pagePanel.Inlines.Add(img);
                            section.Blocks.Add(pagePanel);
                        }
                        break;
                }
            }


            FlowDocument doc = CreateDoc(tpl);
            doc.Blocks.Add(section);
            return doc;
        }
        /// <summary>
        /// 创建FlowDocument文档
        /// </summary>
        /// <param name="tpl">模板</param>
        /// <returns>返回FlowDocument文档</returns>
        private FlowDocument CreateDoc(Template tpl)
        {
            if (tpl.Items == null || tpl.Items.Count == 0)
                return null;
            FlowDocument doc = new FlowDocument();
            if (!string.IsNullOrWhiteSpace(tpl.FontFamily))

                doc.FontFamily = new Typeface(tpl.FontFamily).FontFamily;
            if (tpl.FontSize > 0)

                doc.FontSize = tpl.FontSize;
            doc.PagePadding = tpl.Padding;
            if (tpl.LineHeight > 0)
                doc.LineHeight = tpl.LineHeight;
            SetControlFontStyle(tpl.FontStyle, doc);
            SetControlFontWeight(tpl.FontWeight, doc);
            return doc;

        }
        /// <summary>
        /// 格式化图片模板
        /// </summary>
        /// <param name="tableTemplate"></param>
        /// <returns></returns>
        public Inline CreateImageTemplateItem(TemplateItem tableTemplate)
        {
            var img = new Image();
            img.Source = new BitmapImage(new Uri(tableTemplate.BindingData));
            var container = new InlineUIContainer(img);
            return container;
        }
        /// <summary>
        /// 格式化分割线模板
        /// </summary>
        /// <param name="tableTemplate"></param>
        /// <returns></returns>
        private Section CreateLineTemplateItem(TemplateItem tableTemplate)
        {
            var section = new Section();
            section.BorderBrush = Brushes.Black;
            section.BorderThickness = new Thickness(0, 0, 0, tableTemplate.Size);
            return section;
        }
        /// <summary>
        /// 格式化表格模板
        /// </summary>
        /// <param name="tableTemplate"></param>
        /// <param name="modelProcessor"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private Table CreateTableTemplateItem(TemplateItem tableTemplate, IModelProcessor modelProcessor, object model)
        {
            var table = new Table();
            table.Margin = tableTemplate.Margin;
            table.Padding = tableTemplate.Padding;
            if (!string.IsNullOrWhiteSpace(tableTemplate.FontFamily))
                table.FontFamily = new Typeface(tableTemplate.FontFamily).FontFamily;
            if (tableTemplate.Size > 0)
                table.FontSize = tableTemplate.Size;
            SetControlFontStyle(tableTemplate.FontStyle, table);
            SetControlFontWeight(tableTemplate.FontWeight, table);
            var items = modelProcessor.GetItems(tableTemplate.BindingData, model);
            var rowGroup = new TableRowGroup();
            var header = new TableRow();//create header
            foreach (var item in tableTemplate.Children)
            {
                var cell = new TableCell();

                if (item.TableHeader != null)
                {
                    var content = CreateTextTemplateItem(item.TableHeader, modelProcessor, model);
                    var textContainer = new Paragraph();
                    textContainer.Inlines.Add(content);
                    cell.Blocks.Add(textContainer);
                }
                if (item.InTableColumnSpan > 0)
                    cell.ColumnSpan = item.InTableColumnSpan;
                header.Cells.Add(cell);
            }
            rowGroup.Rows.Add(header);

            foreach (var context in items) //create Item
            {
                var row = new TableRow();
                foreach (var item in tableTemplate.Children)
                {
                    var content = CreateTextTemplateItem(item, modelProcessor, context);
                    var textContainer = new Paragraph();
                    textContainer.Inlines.Add(content);
                    var cell = new TableCell();
                    if (item.InTableColumnSpan > 0)
                        cell.ColumnSpan = item.InTableColumnSpan;
                    cell.Blocks.Add(textContainer);
                    row.Cells.Add(cell);
                }
                rowGroup.Rows.Add(row);
            }
            table.RowGroups.Add(rowGroup);
            return table;
        }
        /// <summary>
        /// 格式化文本模板
        /// </summary>
        /// <param name="itemTpl"></param>
        /// <param name="modelProcessor"></param>
        /// <param name="currentDataContext"></param>
        /// <returns></returns>
        private Inline CreateTextTemplateItem(TemplateItem itemTpl, IModelProcessor modelProcessor, object currentDataContext)
        {
            var content = modelProcessor.GetBindingValue(itemTpl.BindingData, currentDataContext);
            var text = new Run(content);
            var textPanel = new TextBlock(text);
            if (!string.IsNullOrWhiteSpace(itemTpl.FontFamily))
                textPanel.FontFamily = new Typeface(itemTpl.FontFamily).FontFamily;
            if (itemTpl.Size > 0)
                textPanel.FontSize = itemTpl.Size;
            textPanel.TextWrapping = TextWrapping.WrapWithOverflow;
            textPanel.Padding = itemTpl.Padding;
            textPanel.Margin = itemTpl.Margin;
            SetControlFontStyle(itemTpl.FontStyle, textPanel);
            SetControlFontWeight(itemTpl.FontWeight, textPanel);
            var container = new InlineUIContainer(textPanel);
            return container;


        }

        /// <summary>
        /// 设置字体样式
        /// </summary>
        /// <param name="fontStyle"></param>
        /// <param name="ctrl"></param>
        private void SetControlFontStyle(string fontStyle, dynamic ctrl)
        {
            switch (fontStyle)
            {
                case "Italic":
                    ctrl.FontStyle = FontStyles.Italic;
                    break;
                case "Normal":
                    ctrl.FontStyle = FontStyles.Normal;
                    break;
                case "Oblique":
                    ctrl.FontStyle = FontStyles.Oblique;
                    break;
            }
        }
        /// <summary>
        /// 设置字体宽度
        /// </summary>
        /// <param name="fontWeight"></param>
        /// <param name="ctrl"></param>
        private void SetControlFontWeight(string fontWeight, dynamic ctrl)
        {
            switch (fontWeight)
            {
                case "Black":
                    ctrl.FontWeight = FontWeights.Black;
                    break;
                case "Bold":
                    ctrl.FontWeight = FontWeights.Bold;
                    break;
                case "DemiBold":
                    ctrl.FontWeight = FontWeights.DemiBold;
                    break;
                case "ExtraBlack":
                    ctrl.FontWeight = FontWeights.ExtraBlack;
                    break;
                case "ExtraBold":
                    ctrl.FontWeight = FontWeights.ExtraBold;
                    break;
                case "ExtraLight":
                    ctrl.FontWeight = FontWeights.ExtraLight;
                    break;
                case "Heavy":
                    ctrl.FontWeight = FontWeights.Heavy;
                    break;
                case "Light":
                    ctrl.FontWeight = FontWeights.Light;
                    break;
                case "Medium":
                    ctrl.FontWeight = FontWeights.Medium;
                    break;
                case "Normal":
                    ctrl.FontWeight = FontWeights.Normal;
                    break;
                case "Regular":
                    ctrl.FontWeight = FontWeights.Regular;
                    break;
                case "SemiBold":
                    ctrl.FontWeight = FontWeights.SemiBold;
                    break;
                case "Thin":
                    ctrl.FontWeight = FontWeights.Thin;
                    break;
                case "UltraBlack":
                    ctrl.FontWeight = FontWeights.UltraBlack;
                    break;
                case "UltraBold":
                    ctrl.FontWeight = FontWeights.UltraBold;
                    break;
                case "UltraLight":
                    ctrl.FontWeight = FontWeights.UltraLight;
                    break;
            }
        }

    }
}
