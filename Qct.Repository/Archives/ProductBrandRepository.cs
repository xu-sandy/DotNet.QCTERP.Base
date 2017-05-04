using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Persistance.Data;

namespace Qct.Repository
{
    public class ProductBrandRepository : EFRepositoryWithIntegerIdEntity<ProductBrand>, IProductBrandRepository
    {
        int CompanyId { get; set; }
        public ProductBrandRepository(int companyId) : base(DataContextFactory.Create<DataContext>())
        {
            CompanyId = companyId;
        }
    }
}
