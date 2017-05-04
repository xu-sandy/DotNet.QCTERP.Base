using System.Linq;

namespace Qct.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEFRepository<T> : IRepository<T>
    {
        IQueryable<T> GetReadOnlyEntities();
        T GetByReadOnly(object id);
        void CreateWithSaveChanges(T item);
        void DeleteWithSaveChanges(object id);
        void DeletesWithSaveChanges(object[] ids, string idName = "Id");
        void SaveChanges();
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity">对象</param>
        void Removes(params T[] entities);
        /// <summary>
        /// 批量删除并保存
        /// </summary>
        /// <param name="entity">对象</param>
        void RemoveWithSaveChanges(params T[] entities);
        dynamic AddOrUpdate(params T[] objs);
    }
}
