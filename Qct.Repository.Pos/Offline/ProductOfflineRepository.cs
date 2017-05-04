using Qct.IRepository;
using System;
using System.Collections.Generic;
using Qct.Domain.CommonObject;
using Qct.Objects.Entities;
using System.Linq;
using System;

namespace Qct.Repository.Pos.Offline
{
    public class ProductOfflineRepository : IProductRepository
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
            throw new NotImplementedException();
        }

        public IEnumerable<ProductRecord> FindProductByBars(string[] barcodes)
        {
            throw new NotImplementedException();
        }

        public ProductRecord FindProductByProductCode(string productCode)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
