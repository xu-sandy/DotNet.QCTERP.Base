using System.Collections;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Qct.Infrastructure.Data.EnityFramework
{
    public class CommonDbContext : DbContext
    {
        public CommonDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        /// <summary>
        /// 只读模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> ReadOnly<T>() where T : class
        {
            var result = ObjContext().CreateObjectSet<T>();
            result.MergeOption = MergeOption.NoTracking;
            return result;
        }
        /// <summary>
        /// 跟踪模式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Trackable<T>() where T : class
        {
            return ObjContext().CreateObjectSet<T>();
        }
        /// <summary>
        /// 使用特定实体的存储数据刷新缓存数据。
        /// </summary>
        /// <param name="collection"></param>
        public void Refresh(IEnumerable collection)
        {
            ObjContext().Refresh(RefreshMode.StoreWins, collection);
        }
        /// <summary>
        /// 使用特定实体的存储数据刷新缓存数据。
        /// </summary>
        /// <param name="item"></param>
        public void Refresh(object item)
        {
            ObjContext().Refresh(RefreshMode.StoreWins, item);
        }
        /// <summary>
        /// 从对象上下文移除对象。
        /// </summary>
        /// <param name="item"></param>
        public void Detach(object item)
        {
            ObjContext().Detach(item);
        }
        /// <summary>
        /// 属性加载
        /// </summary>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        public void LoadProperty(object item, string propertyName)
        {
            ObjContext().LoadProperty(item, propertyName);
        }
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }
        public DbRawSqlQuery<T> ExecuteQuery<T>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<T>(sql, parameters);
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            ObjContext().Connection.Close();
        }
        /// <summary>
        /// 获取ObjectContext对象
        /// </summary>
        /// <returns></returns>
        public ObjectContext ObjContext()
        {
            return ((IObjectContextAdapter)this).ObjectContext;
        }
        /// <summary>
        /// 接受在对象上下文中对对象所做的所有更改。
        /// </summary>
        public void AcceptAllChanges()
        {
            ObjContext().AcceptAllChanges();
        }
    }
}
