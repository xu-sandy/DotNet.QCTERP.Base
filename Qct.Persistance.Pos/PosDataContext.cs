using Qct.Infrastructure.Data.EnityFramework;
using Qct;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Qct.Objects.Entities;

namespace Qct.Persistance.Pos
{
    [DbConfigurationType(typeof(PosDataContextDbConfiguration))]
    public class PosDataContext : CommonDbContext
    {
        public PosDataContext() : this(false)
        {
        }
        private PosDataContext(bool disableProxy = false)
           : base(PosDataContextConnectionString.GetConnectionString())
        {
            if (disableProxy)
                Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ///移除EF映射默认给表名添加“s“或者“es”
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<IncreasingNumber> IncreasingNumbers { get; set; }
    }
}
