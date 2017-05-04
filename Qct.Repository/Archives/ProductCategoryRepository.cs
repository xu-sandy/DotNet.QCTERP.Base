using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Persistance.Data;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Qct.Repository
{
    public class ProductCategoryRepository : BaseEFRepository<ProductCategory>, IProductCategoryRepository
    {
        
        public ProductCategory Get(int categorySN)
        {
            var result = GetEntities().FirstOrDefault(
                o =>
                    o.CategorySN == categorySN
                    && o.State == 1
                    && o.CompanyId == CompanyId);
            return result;
        }

        public ProductCategory GetIngoreState(int categorySN)
        {
            var result = GetEntities().FirstOrDefault(
                o =>
                    o.CategorySN == categorySN
                    && o.CompanyId == CompanyId);
            return result;
        }

        public IEnumerable<ProductCategory> GetProductCategorys(params int[] categorySNs)
        {
            IEnumerable<ProductCategory> result = null;
            if (categorySNs == null || !categorySNs.Any())
            {
                result = GetEntities().Where(o => o.State == 1 && o.CategoryPSN == 0 && o.CompanyId == CompanyId);
            }
            else
            {
                result = GetEntities().Where(o => o.State == 1 && categorySNs.Contains(o.CategorySN) && o.CompanyId == CompanyId);

            }
            return result;
        }

        public IEnumerable<ProductCategory> GetRootCategorys(bool all = false)
        {
            var query = GetReadOnlyEntities().Where(o=>o.CategoryPSN==0);
            if (!all)
                query = query.Where(o => o.State == 1);
            var list = query.ToList();
            list.ForEach(a =>
            {
                if (a.State == 0)
                    a.Title = "*" + a.Title;
            });
            return list.OrderByDescending(o => o.State).ThenBy(o => o.OrderNum);
        }
    }
}
