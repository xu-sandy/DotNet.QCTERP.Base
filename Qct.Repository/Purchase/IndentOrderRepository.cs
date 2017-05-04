using Qct.IRepository;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Infrastructure.Data.Extensions;
using Qct.Objects.ValueObjects;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using Qct.Infrastructure.Extensions;
namespace Qct.Repository
{
    public class IndentOrderRepository : BaseEFRepository<IndentOrder>, IIndentOrderRepository
    {
        readonly IWarehouseRepository _warehouseRepository = null;
        readonly IProductRepository _productRepository = null;
        public IndentOrderRepository(IWarehouseRepository warehouseRepository, IProductRepository productRepository)
        {
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
        }

        public PageInformaction<IndentOrder> FindPageList(NameValueCollection nvl)
        {
            var state = nvl["State"];//状态
            var orderMan = nvl["OrderMan"];//订货人
            var startDate = nvl["StartDate"];//订货日期
            var endDate = nvl["EndDate"];
            var sup = nvl["sup"];//订单配送
            var supplierId = nvl["supplierId"];//供应商
            var orderNo = nvl["orderNo"];
            var expin = nvl["expin"];//排除已入库
            var searchField = nvl["searchField"];
            var searchText = nvl["searchText"];
            var currentStoreId = nvl["storeId"];
            var query = GetReadOnlyEntities().Include(o=>o.IndentOrderItems);
            if (!state.IsNullOrEmpty())
            {
                var st = state.Split(',').Select(o => short.Parse(o)).ToList();
                query = query.Where(d => st.Contains(d.State));
            }
            if (!orderMan.IsNullOrEmpty())
            {
                query = query.Where(d => d.CreateUID == orderMan);
            }
            if (!orderNo.IsNullOrEmpty())
            {
                query = query.Where(d => d.IndentOrderId.Contains(orderNo));
            }
            if (!startDate.IsNullOrEmpty())
            {
                var st1 = DateTime.Parse(startDate);
                query = query.Where(d => d.CreateDT >= st1);
            }
            if (!endDate.IsNullOrEmpty())
            {
                var st2 = DateTime.Parse(endDate).AddDays(1);
                query = query.Where(d => d.CreateDT < st2);
            }
            if (sup == "1")
                query = query.Where(o => o.State > 0);
            if (!expin.IsNullOrEmpty())
            {
                //query = query.Where(i => !InboundGoodsBLL.CurrentRepository.Entities.Any(o => o.IndentOrderId == i.IndentOrderId));
            }
            if (!currentStoreId.IsNullOrEmpty())
                query = query.Where(o => o.StoreId == currentStoreId);
            if (!supplierId.IsNullOrEmpty())
                query = query.Where(o => o.SupplierID == supplierId);

            if (!searchText.IsNullOrEmpty())
            {
                if (searchField == "barcode")
                    query = query.Where(o => o.IndentOrderItems.Any(i => i.ProductCode.Contains(searchText)));
                else if (searchField == "IndentOrderId")
                    query = query.Where(o => o.IndentOrderId.Contains(searchText));
            }
            
            return query.GetPageWithInformaction();
        }

        public List<IndentOrderModel> GetNewOrder(int num, string currentStoreId)
        {
            var query = GetReadOnlyEntities().Include(o => o.IndentOrderItems);
            if (!string.IsNullOrWhiteSpace(currentStoreId))
                query = query.Where(o => o.StoreId == currentStoreId);
            var queryStore = _warehouseRepository.GetReadOnlyEntities();
            var q = query.Select(x => new
            {
                x,
                items=x.IndentOrderItems,
                StoreTitle = queryStore.Where(i => i.StoreId == x.StoreId).Select(i => i.Title).FirstOrDefault()
            });
            var ls = q.OrderByDescending(o => o.x.Id).Take(num).ToList();
            var list = new List<IndentOrderModel>();
            foreach (var obj in ls)
            {
                var m = new IndentOrderModel();
                obj.x.ToCopyProperty(m);
                m.IndentOrderItems = obj.items;
                m.StoreTitle = obj.StoreTitle;
                m.OrderGiftnum = m.IndentNums.ToAutoString() + "/" + obj.items.Where(o => o.Nature == 1).Sum(o => o.IndentNum);
                list.Add(m);
            }
            return list;
        }

        public PageInformaction<IndentOrderItemModel> LoadDetailList(string orderId)
        {
            var queryOrder = GetReadOnlyEntities().Where(o=>o.IndentOrderId==orderId);
            var queryProduct = _productRepository.GetReadOnlyEntities().Include(o => o.SubUnit);
            var queryOrderItem = _context.Set<IndentOrderList>();
            var q = from x in queryOrder
                    join y in queryOrderItem on x.IndentOrderId equals y.IndentOrderId
                    join z in queryProduct on new { x.CompanyId, y.ProductCode } equals new { z.CompanyId, z.ProductCode }
                    select new
                    {
                        y,
                        SubUnit=z.SubUnit==null?"":z.SubUnit.Title,
                        z.Title,
                        z.Barcode,
                        z.CategorySN,
                        z.Expiry
                    };
            var list = q.ToList();
            var ls = new List<IndentOrderItemModel>();
            decimal total = 0, nums = 0;
            foreach (var item in list.Where(o=>o.y.Nature==0))
            {
                var itemModel = new IndentOrderItemModel();
                item.y.ToCopyProperty(itemModel);
                item.ToCopyProperty(itemModel);
                itemModel.Detail =string.Join("<br/>", list.Where(i => i.y.Nature == 1 && i.y.UnderProductCode == item.y.ProductCode).Select(i => i.Barcode + " " + i.Title + i.y.IndentNum.ToAutoString() + "件"));
                itemModel.Gift = string.Join(",", list.Where(i => i.y.Nature == 1 && i.y.UnderProductCode == item.y.ProductCode).Select(i => i.Barcode + "~" + i.y.IndentNum.ToAutoString()));
                total += item.y.Subtotal;
                nums += item.y.IndentNum;
            }
            var page = new PageInformaction<IndentOrderItemModel>();
            page.Datas = ls;
            page.Footers = new List<object>()
            {
                new {Subtotal=total,IndentNum=nums,Price="合计:"}
            };
            return page;
        }

        public PageInformaction LoadReportDetailList(string orderId)
        {
            throw new NotImplementedException();
        }

        public object ReportDetail(string type, string orderId)
        {
            throw new NotImplementedException();
        }

    }
}
