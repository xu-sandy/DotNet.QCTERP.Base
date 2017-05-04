using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Data.Extensions;
using Qct.Infrastructure.Data;

namespace Qct.IRepository
{
    public interface IIndentOrderRepository : IEFRepository<IndentOrder>
    {
        PageInformaction<IndentOrder> FindPageList(NameValueCollection nvl);
        List<IndentOrderModel> GetNewOrder(int num, string currentStoreId);
        PageInformaction<IndentOrderItemModel> LoadDetailList(string orderId);
        PageInformaction LoadReportDetailList(string orderId);
        object ReportDetail(string type, string orderId);
    }
}
