using Qct.IRepository;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Objects.ValueObjects.Inventory;
using System.Collections.Specialized;
using Qct.Infrastructure.Data.Extensions;
using Qct.Infrastructure.Extensions;
using Qct.Objects.ValueObjects;

namespace Qct.Repository
{
    public class WarehouseRepository : BaseEFRepository<Warehouse>, IWarehouseRepository
    {
        public int MaxSn()
        {
            string sql = "SELECT ISNULL(MAX(cast(StoreId as int)),0) from Warehouse where companyId = @companyId";
            var sn= _context.ExecuteQuery<int>(sql,new System.Data.SqlClient.SqlParameter("@companyId", CompanyId)).FirstOrDefault();
            return sn+ 1;
        }

        public System.Data.DataTable FindPageList(NameValueCollection nvl)
        {
            var state = nvl["State"].ToType<short?>();
            var parms = new List<System.Data.SqlClient.SqlParameter>() {new System.Data.SqlClient.SqlParameter("@companyId",CompanyId) };
            var sql = "SELECT *,dbo.GetCategoryTitles(a.CategorySN,a.CompanyId) CategoryTitle FROM dbo.Warehouse a where companyId=@companyId";
            if(state.HasValue)
            {
                sql += " and state=@state";
                parms.Add(new System.Data.SqlClient.SqlParameter("@state", state));
            }
            return GetDataTableBySql(sql, parms.ToArray());
        }

        public List<Warehouse> GetAdminList(bool isAll = false)
        {
            return GetReadOnlyEntities().Where(o => o.AdminState == 1).OrderBy(o => o.Title).ToList();
        }

        public List<Warehouse> GetAdminList(string sid)
        {
            return GetReadOnlyEntities().Where(o => o.AdminState == 1 && o.StoreId==sid).OrderBy(o => o.Title).ToList();
        }

        public List<Warehouse> GetList(bool isAll = false)
        {
            var query = GetReadOnlyEntities();
            if (isAll)
            {
                var all = query.ToList();
                all.ForEach(a => { if (a.State == 0) a.Title = "*" + a.Title; });
                return all.OrderByDescending(a => a.State).ThenBy(a => a.Title).ToList();
            }
            return query.Where(o => o.State == 1).OrderBy(o => o.Title).ToList();
        }
        public OperateResult AddOrUpdateResult(Warehouse obj)
        {
            if (IsExists(o => o.Id != obj.Id && o.Title == obj.Title))
                return OperateResult.Fail("门店名称重复!");
            try
            {
                if (obj.Id == 0)
                {
                    //todo,验证限制门店数
                    obj.State = 1;
                    obj.StoreId = MaxSn().ToString();
                    obj.CreateDT = DateTime.Now;
                    Create(obj);
                }
                else
                {
                    var resouce = Get(obj.Id);
                    obj.ToCopyProperty(resouce, false, "Title", "Address", "CategorySN", "AdminState");
                }
                SaveChanges();
            }
            catch(Exception ex)
            {
                
            }
            return OperateResult.Success();
        }

        public void SetState(string Ids, short state, short type)
        {
            var ids= Ids.ToTypeArray<int>();
            var list = GetEntities().Where(o => ids.Contains(o.Id)).ToList();
            list.ForEach(o => {
                if (type == 1)
                    o.State = state;
                else if (type == 2)
                    o.AdminState = state;
            });
            SaveChanges();
        }
    }
}
