using Qct.Objects.Entities;
using Qct.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Qct.Repository
{
    /// <summary>
    /// 权限-数据访问层
    /// </summary>
    public class SysLimitisRepository : BaseEFRepository<SysLimits>
    {
        /// <summary>
        /// 获得权限列表
        /// </summary>
        /// <returns></returns>
        public List<SysLimits> GetList()
        {
            return GetReadOnlyEntities().Where(o => o.Status != 0).ToList();
        }
        public bool ExistsTitle(SysLimits model)
        {
            return GetReadOnlyEntities().Any(o => o.LimitId != model.LimitId && o.PLimitId == model.PLimitId && o.Title == model.Title);
        }
    }
}
