using Qct.IRepository;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Qct.Infrastructure.Extensions;
using System;

namespace Qct.Repository
{
    public class SysDepartmentRepository : BaseEFRepository<SysDepartments>, ISysDepartmentRepository
    {
        readonly ISysUserRespository _userRepository = null;
        public SysDepartmentRepository(ISysUserRespository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// 获取组织机构列表数据
        /// </summary>
        /// <returns></returns>
        public List<SysDepartmentsModel> GetExtList()
        {
            return GetExtList(-1);
        }
        /// <summary>
        /// 根据Id获取组织机构列表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SysDepartmentsModel> GetExtList(int id)
        {
            
            var queryUser= _userRepository.GetReadOnlyEntities();
            var queryDept = GetReadOnlyEntities();
            var query = GetReadOnlyEntities();
            if (id > 0) query = query.Where(o => o.Id == id);
            var q = from x in query
                    join y in queryDept on new { x.CompanyId, DepId = x.PDepId } equals new { y.CompanyId, y.DepId } into tmp
                    from o in tmp.DefaultIfEmpty()
                    select new SysDepartmentsModel()
                    {
                        Id = x.Id,
                        CompanyId = x.CompanyId,
                        DepId = x.DepId,
                        DeputyUId = x.DeputyUId,
                        IndexPageUrl = x.IndexPageUrl,
                        ManagerUId = x.ManagerUId,
                        PDepId = x.PDepId,
                        SN = x.SN,
                        Status = x.Status,
                        Title = x.Title,
                        SortOrder = x.SortOrder,
                        Type = x.Type,
                        ChildsNum = queryUser.Count(i => i.BumenId == x.DepId),
                        DeputyUName = queryUser.Where(i => i.UID == x.DeputyUId).Select(i => i.FullName).FirstOrDefault(),
                        ManagerUName = queryUser.Where(i => i.UID == x.ManagerUId).Select(i => i.FullName).FirstOrDefault(),
                        PTitle=o!=null?o.Title:"",
                        PType=o!=null?o.Type:(short)0
                    };
            return q.ToList();
        }
        /// <summary>
        /// 根据depId获取组织机构列表数据
        /// </summary>
        /// <param name="depId">机构ID（全局唯一）</param>
        /// <returns></returns>
        public SysDepartments GetListByDepId(int depId)
        {
            return GetReadOnlyEntities().FirstOrDefault(o=>o.DepId==depId && o.Status);
        }
        /// <summary>
        /// 根据类型获得机构/部门列表
        /// </summary>
        /// <param name="type">（1:机构、2:部门、3:子部门）</param>
        /// <returns></returns>
        public List<SysDepartments> GetListByType(int type)
        {
            return GetReadOnlyEntities().Where(o=>o.Type==type && o.Status).ToList();
        }
        /// <summary>
        /// 通过pDepId获取部门列表
        /// </summary>
        /// <param name="pDepId"></param>
        /// <returns></returns>
        public List<SysDepartments> GetListByPDepId(int pDepId)
        {
            return GetReadOnlyEntities().Where(o => o.PDepId == pDepId && o.Status).ToList();
        }
        public OperateResult SaveDep(SysDepartments model)
        {
            if(!model.SN.IsNullOrEmpty() && IsExists(o=>o.Id!=model.Id && o.SN==model.SN))
            {
                return OperateResult.Fail("代码已存在，不可重复!");
            }
            if (model.Id == 0)
            {
                model.DepId = GetMaxValInt(o => o.DepId);
                CreateWithSaveChanges(model);
            }
            else
            {
                var obj = Get(model.Id);
                model.ToCopyProperty(obj,true, "CompanyId", "DepId");
                SaveChanges();
            }
            return OperateResult.Success();
        }
        public SysDepartmentsModel GetModel(int id, int pdepid)
        {
            var obj = Get(id);
            var model = new SysDepartmentsModel();
            obj.ToCopyProperty(model);
            if (model == null)
            {
                model = GetNewModel(pdepid);
            }
            return model;
        }
        /// <summary>
        /// 获取新Mode
        /// </summary>
        /// <param name="pobjid"></param>
        /// <returns></returns>
        private SysDepartmentsModel GetNewModel(int pobjid)
        {
            var pmodel = GetListByDepId(pobjid);
            var model = new SysDepartmentsModel();
            if (pmodel != null)
            {
                model.PDepId = pmodel.PDepId;
                model.Type = (short)(pmodel.Type + 1);
                model.PTitle = pmodel.Title;
            }
            else
            {
                model.Status = true;
                model.PDepId = 0;
                model.Type = 1;
            }
            return model;
        }

        public void ChangeStatus(int id)
        {
            var obj = Get(id);
            if (obj == null) return;
            obj.Status = !obj.Status;
            SaveChanges();
        }
    }
}
