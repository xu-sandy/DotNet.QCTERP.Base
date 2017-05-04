using System;
using System.Collections.Generic;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Infrastructure.Data;
using Qct.Persistance.Data;
using System.Linq;
using Qct.Objects.ValueObjects;
using Qct.IRepository.Exceptions;

namespace Qct.Repository
{
    /// <summary>
    /// 商品仓储（服务器版本）
    /// </summary>
    public class ProductRepository : BaseEFRepository<ProductRecord>, IProductRepository
    {

        public IEnumerable<ProductRecord> FindProductByBarcode(string barcode)
        {
            var result = GetEntities().Where(o => o.CompanyId == CompanyId && o.State == ProductState.InTheShelf && o.Barcode == barcode || ("," + o.Barcodes + ",").Contains(barcode)).ToList();
            if (result == null && !result.Any())
            {
                throw new NotFoundProductException(string.Format("未能找到条码【{0}】对应的商品！", barcode));
            }
            return result;
        }

        public IEnumerable<ProductRecord> FindProductByBars(string[] barcodes)
        {
            return GetReadOnlyEntities().Where(o => barcodes.Contains(o.Barcode) || !(o.Barcodes == null || o.Barcodes == ""));
        }

        public ProductRecord FindProductByProductCode(string productCode)
        {
            var result = GetEntities().Where(o => o.CompanyId == CompanyId && o.State == ProductState.InTheShelf && o.ProductCode == productCode).FirstOrDefault();
            if (result == null)
            {
                throw new NotFoundProductException(string.Format("未能找到货号【{0}】对应的商品！", productCode));
            }
            return result;
        }

        public ProductRecord FindProductByProductCodeIngoreState(string productCode)
        {
            var result = GetEntities().Where(o => o.CompanyId == CompanyId && o.ProductCode == productCode).FirstOrDefault();
            if (result == null)
            {
                throw new NotFoundProductException(string.Format("未能找到货号【{0}】对应的商品！", productCode));
            }
            return result;
        }
        
    }
}
