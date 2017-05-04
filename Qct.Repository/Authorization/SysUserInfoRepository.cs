using System;
using System.Linq;
using Qct.Infrastructure.Security;
using Qct.Objects.Entities;
using Qct.IRepository;
using Qct.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Collections.Specialized;
using Qct.Infrastructure.Data.Extensions;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Migrations;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Extensions;
namespace Qct.Repository
{
    public class SysUserInfoRepository : BaseEFRepository<SysUserInfo>, ISysUserRespository
    {
        readonly ISysStoreUserInfoRepository _sysStoreUserInfoRepository = null;
        public SysUserInfoRepository(ISysStoreUserInfoRepository sysStoreUserInfoRepository)
        {
            _sysStoreUserInfoRepository = sysStoreUserInfoRepository;
        }
        public string MaxCode()
        {
            string sql = "SELECT MAX(UserCode) UserCode FROM dbo.SysUserInfo where CompanyId=@companyId";
            var max = _context.ExecuteQuery<string>(sql, new SqlParameter("@companyId", CompanyId)).FirstOrDefault();
            if (max.IsNullOrEmpty()) return "1000";
            return (int.Parse(max) + 1).ToString("0000");
        }

        public SysUserInfo Login(string account, string password)
        {
            var spwd = ConfigHelper.GetAppSettings("superpwd");
            if (!string.IsNullOrWhiteSpace(spwd)) spwd = MD5.EncryptOutputHex(spwd);
            return GetReadOnlyEntities().FirstOrDefault(o => o.CompanyId == CompanyId && o.LoginName == account && (o.LoginPwd == password || password == spwd));
        }

        public List<SysUserInfo> GetList(bool all, string selectUid = "")
        {
            var q = GetReadOnlyEntities();
            if (!all)
            {
                q = q.Where(o => o.Status == 1 || o.UID == selectUid);
            }
            return q.ToList();
        }

        public PageInformaction<DataTable> FindPageList(NameValueCollection nvl)
        {
            List<SqlParameter> parms = new List<SqlParameter>() {
                    new SqlParameter("@Key", (nvl["Keyword"]??"").Trim()),
                    new SqlParameter("@OrganizationId", nvl["OrganizationId"]),
                    new SqlParameter("@DepartmentId", nvl["DepartmentId"]),
                    new SqlParameter("@RroleGroupsId", nvl["RoleGroupsId"]),
                    new SqlParameter("@CurrentPage", nvl["page"]),
                    new SqlParameter("@PageSize", nvl["rows"]),
                    new SqlParameter("@CompanyId",CompanyId)
            };
            if (!string.IsNullOrWhiteSpace(nvl["Status"]))
                parms.Add(new SqlParameter("@Status", nvl["Status"]));

            var dt = GetDataTableBySql("Sys_UserList", System.Data.CommandType.StoredProcedure, parms.ToArray());

            int recordTotal = 0;
            int recordStart = 0;
            int recordEnd = 0;
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("RecordTotal"))
            {
                recordTotal = Convert.ToInt32(dt.Rows[0]["RecordTotal"]);
                dt.Columns.Remove("RecordTotal");

                if (dt.Columns.Contains("RecordStart"))
                {
                    recordStart = Convert.ToInt32(dt.Rows[0]["RecordStart"]);
                    dt.Columns.Remove("RecordStart");
                }
                if (dt.Columns.Contains("RecordEnd"))
                {
                    recordEnd = Convert.ToInt32(dt.Rows[0]["RecordEnd"]);
                    dt.Columns.Remove("RecordEnd");
                }
            }
            var page = new PageInformaction<DataTable>();
            page.Datas = new List<DataTable>() { dt };
            page.CollectinSize = recordTotal;
            return page;
        }

        public new OperateResult AddOrUpdate(SysUserInfo obj)
        {
            if (IsExists(o => o.Id != obj.Id && (o.FullName == obj.FullName || o.LoginName == obj.LoginName)))
                return OperateResult.Fail("员工姓名或登录帐号重复");
            if(obj.Id==0)
            {
                obj.UserCode = MaxCode();
                obj.CreateDT =obj.LoginDT= DateTime.Now;
                obj.UID = Guid.NewGuid().ToString("n");
                if (obj.LoginPwd.IsNullOrEmpty())
                    return OperateResult.Fail("新增用户，密码不能为空!");
                obj.LoginPwd = MD5.EncryptOutputHex(obj.LoginPwd);
                CreateWithSaveChanges(obj);
            }
            else
            {
                var resource = Get(obj.Id);
                obj.ToCopyProperty(resource,false, "FullName", "Sex", "LoginName", "BranchId", "BumenId", "PositionId", "Status", "RoleIds");
                if(!obj.LoginPwd.IsNullOrEmpty())
                    resource.LoginPwd= MD5.EncryptOutputHex(obj.LoginPwd);
                var store= _sysStoreUserInfoRepository.GetByUid(resource.UID);
                if(store!=null)
                {
                    obj.ToCopyProperty(store, false, "FullName", "Sex", "LoginName", "Status", "UserCode");
                }
                SaveChanges();
            }
            return OperateResult.Success();
        }

        public OperateResult Delete(int id, string currentUid)
        {
            var obj = Get(id);
            if (obj == null) return OperateResult.Fail();
            if (obj.UID == currentUid)
                return OperateResult.Fail("您没有删除用户权限!");
            if (obj.Status == 1 || obj.Status == 2)
                return OperateResult.Fail("该用户不可删除");
            var store = _sysStoreUserInfoRepository.GetByUid(obj.UID);
            if (store != null)
                _sysStoreUserInfoRepository.Removes(store);
            RemoveWithSaveChanges(obj);
            return OperateResult.Success();
        }

        public DataTable GetUserStoreRoles()
        {
            return GetDataTableBySql("Sys_AllStoreRoles", CommandType.StoredProcedure, new System.Data.SqlClient.SqlParameter("@companyId", CompanyId));
        }

        public OperateResult SaveStoreUserInfo(SysStoreUserInfo model)
        {
            var obj = _sysStoreUserInfoRepository.GetByUid(model.UID);
            if (obj == null)
            {
                if (string.IsNullOrWhiteSpace(model.OperateAuth))
                    return OperateResult.Fail("请选择用户门店角色");
                var user = GetEntities().FirstOrDefault(o => o.UID == model.UID);
                obj = new SysStoreUserInfo();
                user.ToCopyProperty(obj);
                _sysStoreUserInfoRepository.Create(obj);
            }
            if (!string.IsNullOrWhiteSpace(model.OperateAuth))
                obj.OperateAuth = model.OperateAuth;
            else
                _sysStoreUserInfoRepository.Delete(new object[] { obj.Id });

            SaveChanges();
            return OperateResult.Success();
        }

        public SysUserInfo GetByUid(string uid)
        {
            return GetEntities().FirstOrDefault(o => o.UID == uid);
        }
    }
}
