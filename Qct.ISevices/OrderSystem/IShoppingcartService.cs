using Qct.Domain.CommonObject;
using Qct.Objects.Entities;
using System.Collections.Generic;

namespace Qct.IServices
{
    /// <summary>
    /// 购物车服务
    /// </summary>
    public interface IShoppingcartService
    {
        /// <summary>
        /// 条码解析（包含称重条码解析）
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isNotFoundThrowException">当未找到商品时，是否抛出异常</param>
        /// <returns>商品列表</returns>
        IEnumerable<ProductRecord> BarcodeAnalysis(string barcode, bool isNotFoundThrowException = false);
        /// <summary>
        /// 称重条码解析
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isNotFoundThrowException">当未找到商品时，是否抛出异常</param>
        /// <returns>商品列表</returns>
        IEnumerable<ProductRecord> WeightBarcodeAnalysis(string barcode, bool isNotFoundThrowException = false);

    }
}
