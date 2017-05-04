using Qct.IRepository;
using System.Collections.Generic;
using Qct.Repository.Pos.Common;
using Qct.IRepository.Exceptions;
using Qct.Objects.Entities;
using System;
using System.Linq;
using Qct.Infrastructure.Data;

namespace Qct.Repository.Pos.Online
{
    public class ProductOnlineRepository : IProductRepository
    {
        public dynamic AddOrUpdate(params ProductRecord[] objs)
        {
            throw new NotImplementedException();
        }

        public dynamic AddOrUpdate(ProductRecord obj)
        {
            throw new NotImplementedException();
        }

        public void Create(ProductRecord item)
        {
            throw new NotImplementedException();
        }

        public void CreateWithSaveChanges(ProductRecord item)
        {
            throw new NotImplementedException();
        }

        public void Delete(object[] ids, string idName = "Id")
        {
            throw new NotImplementedException();
        }

        public void Delete(object id, bool notFindThowException = false)
        {
            throw new NotImplementedException();
        }

        public void DeletesWithSaveChanges(object[] ids, string idName = "Id")
        {
            throw new NotImplementedException();
        }

        public void DeleteWithSaveChanges(object id)
        {
            throw new NotImplementedException();
        }

        public void DeleteWithSaveChanges(object[] ids, string Name = "Id")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductRecord> FindProductByBarcode(string barcode)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("barcode", barcode);
            var result = POSRestClient.Post<List<ProductRecord>>("Api/Product/FindProductByBarcode", parameters);
            if (result.Successed)
            {
                return result.Data;
            }
            else
            {
                throw new NotFoundProductException(result.Message);
            }
        }

        public IEnumerable<ProductRecord> FindProductByBars(string[] barcodes)
        {
            throw new NotImplementedException();
        }

        public ProductRecord FindProductByProductCode(string productCode)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("productCode", productCode);
            var result = POSRestClient.Post<ProductRecord>("Api/Product/FindProductByProductCode", parameters);
            if (result.Successed)
            {
                return result.Data;
            }
            else
            {
                throw new NotFoundProductException(result.Message);
            }
        }

        public ProductRecord FindProductByProductCodeIngoreState(string productCode)
        {
            throw new NotImplementedException();
        }

        public ProductRecord Get(object id)
        {
            throw new NotImplementedException();
        }

        public ProductRecord GetByReadOnly(object id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductRecord> GetEntities()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductRecord> GetReadOnlyEntities()
        {
            throw new NotImplementedException();
        }

        public void Removes(params ProductRecord[] entities)
        {
            throw new NotImplementedException();
        }

        public void RemoveWithSaveChanges(params ProductRecord[] entities)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
        }

 
    }
}
