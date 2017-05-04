using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Persistance.Data;

namespace Qct.Repository
{
    public class ProductChangePriceRepository : EFRepositoryWithIntegerIdEntity<ProductChangePrice>, IProductChangePriceRepository
    {
        public int CompanyId { get; set; }
        public ProductChangePriceRepository(int companyId) : base(DataContextFactory.Create<DataContext>())
        {
            CompanyId = companyId;
        }


    }
}
