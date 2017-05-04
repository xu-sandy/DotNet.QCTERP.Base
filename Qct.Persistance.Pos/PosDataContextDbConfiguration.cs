using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServerCompact;

namespace Qct.Persistance.Pos
{
    public class PosDataContextDbConfiguration : DbConfiguration
    {
        public PosDataContextDbConfiguration()
        {
            SetProviderServices(SqlCeProviderServices.ProviderInvariantName, SqlCeProviderServices.Instance);
            SetDefaultConnectionFactory(new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0", "", PosDataContextConnectionString.GetConnectionString()));
        }

    }
}
