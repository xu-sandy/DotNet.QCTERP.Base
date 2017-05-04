using Qct.Infrastructure.Data.Configuration;
using Qct.Infrastructure.Data.EnityFramework;
using Qct.Objects.Entities;
using Qct.Objects.Entities.Systems;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Qct.Persistance.Data
{
    public class DataContext : CommonDbContext
    {
        public DataContext() : this(false)
        {
        }
        public DataContext(bool disableProxy = false)
           : base(ConnectionStringConfig.GetConnectionString())
        {
            if (disableProxy)
                Configuration.ProxyCreationEnabled = false;
        }

        //public DbSet<StoredValueCardPayRecord> StoredValueCardPayRecords { get; set; }
        //public DbSet<MembershipCard> MembershipCards { get; set; }
        //public DbSet<Members> Members { get; set; }
        //public DbSet<SysStoreUserInfo> SysStoreUserInfos { get; set; }
        //public DbSet<PayConfiguration> PayConfigurations { get; set; }
        //public DbSet<PayWay> PayWays { get; set; }
        //public DbSet<StorePaymentAuthorization> StorePaymentAuthorizations { get; set; }

        public DbSet<ProductRecord> ProductRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ///移除EF映射默认给表名添加“s“或者“es”
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Allocation
            #endregion Allocation

            #region Archives

            #region ProductCategory
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory").HasKey(o => new { o.CategorySN, o.CompanyId });
            modelBuilder.Entity<ProductCategory>().HasRequired(o => o.Parent).WithMany().HasForeignKey(o => new { o.CategoryPSN, o.CompanyId });
            #endregion ProductCategory

            #region ProductBrand
            modelBuilder.Entity<ProductBrand>().ToTable("ProductBrand").HasKey(o => new { o.BrandSN, o.CompanyId });
            #endregion ProductBrand

            #region ProductRecord
            modelBuilder.Entity<ProductRecord>().ToTable("ProductRecord");
            modelBuilder.Entity<ProductRecord>().HasRequired(o => o.SubUnit).WithMany().HasForeignKey(o => new { o.SubUnitId, o.CompanyId });
            modelBuilder.Entity<ProductRecord>().HasRequired(o => o.Category).WithMany().HasForeignKey(o => new { o.CategorySN, o.CompanyId });
            modelBuilder.Entity<ProductRecord>().HasRequired(o => o.Brand).WithMany().HasForeignKey(o => new { o.BrandSN, o.CompanyId });
            #endregion ProductRecord

            #region ProductGroupSplit
            var productGroupSplit = modelBuilder.Entity<ProductGroupSplit>().ToTable("ProductGroupSplit").HasKey(o => o.Id);
            productGroupSplit.HasMany(o => o.Items).WithRequired().HasForeignKey(o => o.GroupSplitId);
            var productGroupSplitItem = modelBuilder.Entity<ProductGroupSplitItem>().ToTable("ProductGroupSplitItem");
            productGroupSplitItem.HasRequired(o => o.Parent).WithMany().HasForeignKey(o => o.GroupSplitId);
            #endregion ProductGroupSplit

            #region ProductChangePrice
            var productChangePrice = modelBuilder.Entity<ProductChangePrice>().ToTable("ProductChangePrice").HasKey(o => o.ChangePriceSN);
            productChangePrice.HasMany(o => o.Items).WithRequired().HasForeignKey(o => o.ChangePriceSN);
            var productChangePriceItem = modelBuilder.Entity<ProductChangePriceItem>().ToTable("ProductChangePriceItem");
            productChangePriceItem.HasRequired(o => o.Parent).WithMany().HasForeignKey(o => o.ChangePriceSN);
            #endregion ProductChangePrice

            #endregion Archives

            #region Authorization
            modelBuilder.Entity<SysUserInfo>().HasKey(o => new { o.UID, o.CompanyId });
            modelBuilder.Entity<SysUserInfo>().Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<SysStoreUserInfo>();
            #endregion Authorization

            #region Inventory
            #region Inventory
            #endregion Inventory
            #endregion Inventory

            #region Member
            #endregion Member

            #region OrderSystem
            #endregion OrderSystem

            #region Purchase
            modelBuilder.Entity<IndentOrder>().HasKey(o => o.IndentOrderId).HasMany(o => o.IndentOrderItems).WithOptional().HasForeignKey(o => o.IndentOrderId);
            #endregion Purchase

            #region Sync
            #endregion Sync

            #region System
            modelBuilder.Entity<SysDataDictionary>().HasKey(o => new { o.DicSN, o.CompanyId });
            modelBuilder.Entity<SysDataDictionary>().Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<SysMenus>();
            modelBuilder.Entity<Notice>();
            modelBuilder.Entity<SysWebSetting>();
            modelBuilder.Entity<Warehouse>().HasKey(o => new { o.StoreId, o.CompanyId });
            modelBuilder.Entity<Warehouse>().Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<SysLog>();
            modelBuilder.Entity<SysCustomMenus>();
            modelBuilder.Entity<SysDepartments>();
            modelBuilder.Entity<SysLimits>();
            modelBuilder.Entity<SysRoles>();
            modelBuilder.Entity<SysStoreUserInfo>();
            modelBuilder.Entity<ImportSet>();
            modelBuilder.Entity<ApiLibrary>();
            modelBuilder.Entity<DeviceRegInfo>().HasRequired(o => o.Store).WithMany().HasForeignKey(o => new { o.StoreId, o.CompanyId });
            modelBuilder.Entity<DeviceRegInfo>().HasRequired(o => o.Auditor).WithMany().HasForeignKey(o => new { o.AuditorUID, o.CompanyId });
            #endregion System

            //#region Members 会员信息
            //modelBuilder.Entity<Members>().Property(t => t.UsableIntegral).HasPrecision(18, 4);
            //#endregion Members 会员信息

            #region Sales 销售
            modelBuilder.Entity<SaleOrders>().HasKey(o=>o.PaySN);
            modelBuilder.Entity<SaleDetail>();
            modelBuilder.Entity<ConsumptionPayment>();
            modelBuilder.Entity<WipeZero>();
            #endregion Members 会员信息


        }



    }
}
