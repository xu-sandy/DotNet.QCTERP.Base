using Qct.Domain.CommonObject;
using Qct.IRepository;
using Qct.IRepository;
using Qct.IServices;
using Qct.Objects.Entities;
using Qct.Services.Exceptions;
using Qct.Repository.Pos.Offline;
using Qct.Repository.Pos.Online;
using Qct.Repository.Pos;
using Qct.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qct.Services
{
    /// <summary>
    /// 购物车服务
    /// </summary>
    public class ShoppingcartService : IShoppingcartService
    {
        /// <summary>
        /// 输入信息解析为商品
        /// </summary>
        /// <param name="text">输入文本</param>
        /// <returns>商品列表</returns>
        public IEnumerable<ProductRecord> InputProductAnalysis(string text, bool isNotFoundThrowException = false)
        {
            //异步多线程处理条码解析
            var productTask = Task.Factory.StartNew(() => { return BarcodeAnalysis(text); });
            var weightProductTask = Task.Factory.StartNew(() => { return WeightBarcodeAnalysis(text); });
            Task.WaitAll(productTask, weightProductTask);
            List<ProductRecord> products = new List<ProductRecord>();
            if (weightProductTask.Exception == null && weightProductTask.IsCompleted && weightProductTask.Result != null && weightProductTask.Result.Any())
            {
                products.AddRange(weightProductTask.Result);
            }
            if (productTask.IsCompleted && productTask.Exception == null && productTask.Result != null && productTask.Result.Any())
            {
                products.AddRange(productTask.Result);
            }
            if (isNotFoundThrowException && !products.Any())
            {
                throw new NotFoundProductException(string.Format("未能找到指定条码【{0}】的商品！", text));
            }
            return products;

        }
        /// <summary>
        /// 输入信息解析为已标识动作
        /// </summary>
        /// <param name="text">输入信息</param>
        /// <param name="shoppingcartId">购物车编号</param>
        /// <param name="selectItemId">选项编号</param>
        /// <returns>解析是否成功</returns>
        public bool InputFlagActionAnalysis(string text, Guid shoppingcartId, Guid selectItemId)
        {
            if (text.Length >= 1 && shoppingcartId != Guid.Empty && selectItemId != Guid.Empty)
            {
                var flagCode = text.Substring(0, 1).ToUpper();
                switch (flagCode)
                {
                    case "+":
                        {
                            var num = ParseEditProductNumber(text);
                            ShoppingcartRepository shoppingcartRepository = new ShoppingcartRepository();
                            var shoppingcart = shoppingcartRepository.Get(shoppingcartId);
                            var shoppingcartItem = shoppingcart.Items.FirstOrDefault(o => o.Id == selectItemId);
                            shoppingcart.EditProductNumber(shoppingcartItem, shoppingcartItem.Number.UnitNumber + num);
                        }
                        return true;
                    case "-":
                        {
                            var num = ParseEditProductNumber(text);
                            ShoppingcartRepository shoppingcartRepository = new ShoppingcartRepository();
                            var shoppingcart = shoppingcartRepository.Get(shoppingcartId);
                            var shoppingcartItem = shoppingcart.Items.FirstOrDefault(o => o.Id == selectItemId);
                            shoppingcart.EditProductNumber(shoppingcartItem, shoppingcartItem.Number.UnitNumber - num);
                        }
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 解析输入商品数量
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private decimal ParseEditProductNumber(string text)
        {
            decimal num = 0m;
            if (text.Length > 1)
            {
                var numText = text.Substring(1);
                if (decimal.TryParse(numText, out num))
                {
                    return num;
                }
                else
                {
                    throw new BarcodeParseException("您输入的商品数量有误，请重新输入！");
                }
            }
            else if (text.Length == 1)
            {
                num = 1;
            }
            return num;
        }
        /// <summary>
        /// 普通商品条码解析
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isNotFoundThrowException">当未找到商品时，是否抛出异常</param>
        /// <returns>商品列表</returns>
        public IEnumerable<ProductRecord> BarcodeAnalysis(string barcode, bool isNotFoundThrowException = false)
        {
            IEnumerable<ProductRecord> products = new ProductRecord[0];

            IPosSystemSettingsRepository systemSettingsRepository = new PosSystemSettingsRepository();
            var settings = systemSettingsRepository.Get();
            IProductRepository productRepository = null;
            if (settings.EnableRemoteService)
            {
                productRepository = new ProductOnlineRepository();
            }
            else
            {
                productRepository = new ProductOfflineRepository();
            }

            products = productRepository.FindProductByBarcode(barcode);
            if (isNotFoundThrowException && (products == null || products.Count() == 0))
            {
                throw new NotFoundProductException(string.Format("未能找到指定条码【{0}】的商品！", barcode));
            }
            return products;

        }
        /// <summary>
        /// 称重条码解析
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isNotFoundThrowException">当未找到商品时，是否抛出异常</param>
        /// <returns>商品列表</returns>
        public IEnumerable<ProductRecord> WeightBarcodeAnalysis(string barcode, bool isNotFoundThrowException = false)
        {
            IEnumerable<ProductRecord> products = new ProductRecord[0];
            IPosElectronicBalanceSettingsRepository posElectronicBalanceSettingsRepository = new PosElectronicBalanceSettingsRepository();
            var electronicBalanceSetting = posElectronicBalanceSettingsRepository.Get(true);
            if (electronicBalanceSetting.Enable && electronicBalanceSetting.MaybeWeightProduct(barcode))//处理称重商品
            {
                var productBarcode = string.Empty;
                var productCode = string.Empty;

                IPosSystemSettingsRepository systemSettingsRepository = new PosSystemSettingsRepository();
                var settings = systemSettingsRepository.Get();
                IProductRepository productRepository = null;
                if (settings.EnableRemoteService)
                {
                    productRepository = new ProductOnlineRepository();
                }
                else
                {
                    productRepository = new ProductOfflineRepository();
                }

                if (electronicBalanceSetting.IsProductCode(barcode, out productCode))
                {
                    var product = productRepository.FindProductByProductCode(productCode);
                    products = new ProductRecord[] { product };
                }
                else if (electronicBalanceSetting.IsProductBarcode(barcode, out productBarcode))
                {
                    products = productRepository.FindProductByBarcode(productBarcode);
                }
            }
            if (isNotFoundThrowException && (products == null || products.Count() == 0))
            {
                throw new NotFoundProductException(string.Format("未能找到指定条码【{0}】的商品！", barcode));
            }
            return products;
        }




    }
}
