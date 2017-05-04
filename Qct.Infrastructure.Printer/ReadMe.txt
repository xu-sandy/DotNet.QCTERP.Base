模块设计开发者：余雄文
开发时间：2017-03-10
模块功能：打印模板化与数据模型映射

1、本类库是专门针对打印模板化设计的，模板高度灵活，支持数据模型映射。
2、目前，已实现行模板解析器，在一行内只允许出现一个控件（Line,Text,Table,Image）,如果无法满足业务需求，请重新实现IFlowDocumentProcessor
3、数据映射允许在一个文本控件中绑定多个数据，如“商品：##Barcode##  ##Product##”
4、当使用数据映射时需要提供模型处理器的实现，你可以直接实现IModelProcessor或者根据需要重载DefaultModelProcessor中的GetItems、GetModelValue、GetBindingValue方法
5、生成的文档使用FlowDocumentPrinter进行打印
6、PrinterInfo提供了获取打印队列的相关封装
