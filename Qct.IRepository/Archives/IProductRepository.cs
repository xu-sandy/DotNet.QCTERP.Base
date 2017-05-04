using Qct.Infrastructure.Data;
using Qct.Objects.Entities;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Qct.IRepository
{
    /// <summary>
    /// 商品仓储服务
    /// 影响实现对象：门店POS，后台接口，后台管理端
    /// 门店POS：POS客户端基于网络的仓储实现、POS客户端基于离线数据库的实现
    /// 后台管理端/后台接口公用一份实现，基于线上数据库
    /// </summary>
    public interface IProductRepository :  IEFRepository<ProductRecord>
    {
        /// <summary>
        /// 通过商品条码查找商品【注意：一个商品条码可对应多个商品。实现通过商品条码查找商品时，如果未能找到商品需要抛出NotFoundProductException】
        /// </summary>
        /// <param name="barcode">需要查询的条码</param>
        /// <returns>商品信息列表</returns>
        IEnumerable<ProductRecord> FindProductByBarcode(string barcode);
        /// <summary>
        /// 通过商品货号查找商品【注意：一个商品货号可对应一个商品。实现通过商品货号查找商品时，如果未能找到商品需要抛出NotFoundProductException】
        /// </summary>
        /// <param name="productCode">需要查询的货号</param>
        /// <returns>商品信息</returns>
        ProductRecord FindProductByProductCode(string productCode);

        /// <summary>
        /// 通过商品货号查找商品，并忽略商品状态【注意：一个商品货号只可对应一个商品。实现通过商品货号查找商品时，如果未能找到商品需要抛出NotFoundProductException】【用于历史数据查询】
        /// </summary>
        /// <param name="productCode">需要查询的货号</param>
        /// <returns>商品信息</returns>
        ProductRecord FindProductByProductCodeIngoreState(string productCode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcodes"></param>
        /// <returns></returns>
        IEnumerable<ProductRecord> FindProductByBars(string[] barcodes);
    }
}
