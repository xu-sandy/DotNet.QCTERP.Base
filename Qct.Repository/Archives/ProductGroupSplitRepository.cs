using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Repository
{
    public class ProductGroupSplitRepository : EFRepositoryWithIntegerIdEntity<ProductGroupSplit>, IProductGroupSplitRepository
    {
        public ProductGroupSplitRepository(int companyId) : base(DataContextFactory.Create<DataContext>())
        {
            CompanyId = companyId;
        }
        private int CompanyId { get; set; }
    }
}
