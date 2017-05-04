using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Repository;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Qct.Repository
{
    public class SysCustomMenuRepository : BaseEFRepository<SysCustomMenus>, ISysCustomMenusRepository
    {
        /// <summary>
        /// 根据objId获得对应的菜单权限
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public SysCustomMenus GetbyObjid(int objId)
        {
            return GetEntities().Where(o => o.ObjId == objId).FirstOrDefault();
        }
    }
}
