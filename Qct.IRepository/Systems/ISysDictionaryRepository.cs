using System.Collections.Generic;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Data;

namespace Qct.IRepository
{
    public interface ISysDictionaryRepository: IEFRepository<SysDataDictionary>
    {
        /// <summary>
        /// 取最大dic
        /// </summary>
        int GetMaxDicSn { get; }
        /// 数据字典状态变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperateResult ChangeStatus(int sn);
        SysDataDictionary GetItemByDicsn(int dicsn);
        SysDataDictionary GetItemByTitle(string title);
        /// <summary>
        /// 取分页网格数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        List<SysDictionaryModel> GetList(int pageIndex, int pageSize, string key);
        /// <summary>
        /// 根据id取字典扩展数据
        /// </summary>
        /// <param name="idOrDicSn"></param>
        /// <param name="isId"></param>
        /// <returns></returns>
        SysDictionaryModel GetExtModelById(int idOrDicSn, bool isId);
        /// <summary>
        /// 顺序移动
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        OperateResult MoveItem(int mode, int sn);
        /// <summary>
        /// 根据子项查询父项信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="psn"></param>
        /// <returns></returns>
        SysDictionaryModel GetExtModel(int id, int psn);
        List<SysDataDictionary> GetItemsByDicpsn(int dicpsn);
        OperateResult SaveModel(SysDataDictionary model);
    }
}