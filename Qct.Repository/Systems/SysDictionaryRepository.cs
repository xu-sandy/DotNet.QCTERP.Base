using Qct.IRepository;
using Qct.Objects.Entities;
using System.Linq;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Qct.Repository
{
    public class SysDictionaryRepository : BaseEFRepository<SysDataDictionary>, ISysDictionaryRepository
    {

        public int GetMaxDicSn
        {
            get
            {
                return GetEntities().Max(o => (int?)o.SortOrder).GetValueOrDefault() + 1;
            }
        }

        public OperateResult ChangeStatus(int sn)
        {
            var model = GetEntities().FirstOrDefault(o => o.DicSN == sn);
            model.Status = !model.Status;
            try
            {
                SaveChanges();
            }
            catch (System.Exception e)
            {

                throw e;
            }
            return OperateResult.Success("操作保存成功！");

        }
        public OperateResult SaveModel(SysDataDictionary model)
        {
            var tempModel = GetEntities().FirstOrDefault(o => o.DicSN != model.DicSN && o.DicPSN == model.DicPSN && o.Title == model.Title);
            if (tempModel != null)
            {
                return OperateResult.Fail("数据字典名称不能重复！");
            }
            tempModel = GetEntities().FirstOrDefault(o => o.Id == model.Id);
            if (tempModel != null)
            {
                SaveChanges();
                return OperateResult.Success("数据保存成功！");
            }
            else
            {
                var maxObjId = GetEntities().Max(o => o.DicSN);
                model.DicSN = maxObjId + 1;

                var childrenList = GetEntities().Where(o => o.DicPSN == model.DicPSN);
                if (childrenList != null)
                {
                    model.SortOrder = childrenList.Max(o => (int?)o.SortOrder).GetValueOrDefault() + 1;
                }

                try
                {
                    CreateWithSaveChanges(model);
                }
                catch (System.Exception)
                {
                    return OperateResult.Fail("操作失败！");
                }
                return OperateResult.Success("数据保存成功！");
            }
        }
        public SysDictionaryModel GetExtModelById(int idOrDicSn, bool isId)
        {
            var queryData = (from s1 in GetEntities()
                             join s2 in GetEntities()
                             on s1.DicSN equals s2.DicPSN
                             where isId == true ? s1.Id == idOrDicSn : s1.DicSN == idOrDicSn
                             select new SysDictionaryModel
                             {
                                 Id = s1.Id,
                                 CompanyId = s1.CompanyId,
                                 DicSN = s1.DicSN,
                                 DicPSN = s1.DicPSN,
                                 SortOrder = s1.SortOrder,
                                 Title = s1.Title,
                                 Status = s1.Status,
                                 PTitle = s2.Title
                             }).FirstOrDefault();
            return queryData;
        }

        public SysDictionaryModel GetExtModel(int id, int psn)
        {
            var model = this.GetExtModelById(id, true);
            if (model == null)
            {
                model = new SysDictionaryModel();
                var pModel = GetItemByDicsn(psn);//此处根据psn查sn
                model.DicPSN = psn;
                model.PTitle = pModel.Title;
            }
            return model;
        }
        public List<SysDictionaryModel> GetList(int pageIndex, int pageSize, string key)
        {
            return _context.ExecuteQuery<SysDictionaryModel>("exec Sys_DataDicList @p0,@p1,@p2,@p3", key, pageIndex, pageSize, CompanyId).ToList();
        }

        public OperateResult MoveItem(int mode, int sn)
        {
            var op = OperateResult.Fail("顺序移动失败！");
            var obj = GetItemByDicsn(sn);
            var list = GetItemsByDicpsn(obj.DicPSN).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        SysDataDictionary next = null;
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                next = list[i + 1]; break;
                            }
                        }
                        if (next != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = next.SortOrder;
                            next.SortOrder = sort;
                            SaveChanges();
                        }
                    }
                    break;
                default:
                    var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        SysDataDictionary prev = null;
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                prev = list[i - 1]; break;
                            }
                        }
                        if (prev != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = prev.SortOrder;
                            prev.SortOrder = sort;
                            SaveChanges();
                        }
                    }
                    break;
            }
            return op;
        }

        public List<SysDataDictionary> GetItemsByDicpsn(int dicpsn)
        {
            return GetEntities().Where(o => o.DicPSN == dicpsn).OrderBy(o => o.SortOrder).ToList();
        }

        public SysDataDictionary GetItemByDicsn(int dicsn)
        {
            return GetEntities().FirstOrDefault(o => o.DicSN == dicsn);
        }

        public SysDataDictionary GetItemByTitle(string title)
        {
            return GetEntities().FirstOrDefault(o => o.Title == title);
        }
    }
}
