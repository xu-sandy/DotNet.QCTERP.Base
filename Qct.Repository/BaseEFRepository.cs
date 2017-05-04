using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.EnityFramework;
using Qct.Persistance.Data;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Qct.Infrastructure.Data.EntityInterface;
using System.Data.Common;
using System.Linq.Expressions;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Qct.Objects.ValueObjects;
using Qct.IRepository;

namespace Qct.Repository
{
    public abstract class BaseEFRepository<T> : IEFRepository<T> where T : class, CompanyEntity, new()
    {
        protected readonly CommonDbContext _context;
        protected readonly IEntityFrameworkUnitOfWork _unitOfWork;
        private DbSet<T> _entities;

        public BaseEFRepository() : this(DataContextFactory.Create<DataContext>())
        {
        }
        public BaseEFRepository(IEntityFrameworkUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {
            _unitOfWork = unitOfWork;
        }
        public BaseEFRepository(CommonDbContext dataContext)
        {
            _context = dataContext;
        }
        public void Create(T item)
        {
            item.CompanyId = CompanyId;
            Entities.Add(item);
        }

        public virtual void Delete(object id, bool notFindThowException = false)
        {
            var entity = Get(id);
            if (entity != null && entity.CompanyId == CompanyId)
            {
                Entities.Remove(entity);
            }
            else if (notFindThowException)
            {
                throw new Qct.Infrastructure.Exceptions.DataException(string.Format("未找到id为[{0}]的项目！", id));
            }
        }
        /// <summary>
        /// 删除指定主键ID
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idName"></param>
        protected virtual void Deletes(object[] ids, string idName = "Id")
        {
            var type = typeof(T);
            var propery = type.GetProperty(idName);
            if (propery == null)
                throw new DataException("指定列未找到!");
            foreach (var id in ids)
            {
                var obj = new T();
                propery.SetValue(obj, id);
                var ent = _context.Entry(obj);
                ent.State = EntityState.Deleted;
            }
        }
        public virtual T GetByReadOnly(object id)
        {
            return Entities.Find(id);
        }
        public virtual T Get(object id)
        {
            return Entities.Find(id);
        }

        public IQueryable<T> GetEntities()
        {
            return _context.Trackable<T>().Where(o => o.CompanyId == CompanyId);
        }
        public IQueryable<T> GetReadOnlyEntities()
        {
            return _context.ReadOnly<T>().Where(o => o.CompanyId == CompanyId);
        }

        public void CreateWithSaveChanges(T item)
        {
            Create(item);
            SaveChanges();
        }

        public void DeleteWithSaveChanges(object id)
        {
            Delete(id);
            SaveChanges();
        }
        /// <summary>
        /// 删除指定主键ID
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idName"></param>
        public void DeletesWithSaveChanges(object[] ids, string idName = "Id")
        {
            Deletes(ids, idName);
            var result = true;
            do
            {
                try
                {
                    result = false;
                    SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entity = ex.Entries.Single();
                    //entity.Reload();//从数据库重新加载
                    var originals = entity.GetDatabaseValues();
                    if (originals != null)//为null则可能已被删除
                    {
                        entity.OriginalValues.SetValues(originals);
                    }
                    else
                    {
                        entity.State = EntityState.Detached;
                    }
                    result = true;
                }
            } while (result);
        }
        public void Removes(params T[] entities)
        {
            if (entities != null && !entities.Any(o => o == null))
                Entities.RemoveRange(entities);
        }

        public void RemoveWithSaveChanges(params T[] entities)
        {
            Removes(entities);
            SaveChanges();
        }

        public virtual dynamic AddOrUpdate(params T[] objs)
        {
            foreach (var obj in objs)
            {
                obj.CompanyId = CompanyId;
                Entities.AddOrUpdate(obj);
            }
            SaveChanges();
            return OperateResult.Success();
        }

        public void SaveChanges()
        {
            if (_unitOfWork == null)
            {
                _context.SaveChanges();
            }
            else
            {
                _unitOfWork.Commit();
            }
        }
        protected DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }
        private int? tempCompanyId;
        public ICompany _Company { get; set; }
        protected int CompanyId
        {
            get
            {
                if (tempCompanyId != null)
                {
                    return tempCompanyId.Value;
                }
                if (_Company != null)
                {
                    var cid = _Company.GetCompanyId();
                    if (cid > 0)
                    {
                        tempCompanyId = cid;
                        return cid;
                    }
                }
                throw new DataException("企业ID不能为空！");
            }
        }
        public void SetCompanyId(int companyId)
        {
            tempCompanyId = companyId;
        }
        protected bool IsExists(Expression<Func<T, bool>> whereLambda)
        {
            return GetReadOnlyEntities().Any(whereLambda);
        }
        protected int GetMaxValInt(Expression<Func<T, int?>> fieldLambda, int min = 1, int max = int.MaxValue, Expression<Func<T, bool>> whereLambda = null)
        {
            var query = GetReadOnlyEntities();
            if (whereLambda != null) query = query.Where(whereLambda);
            var maxValue = query.Max(fieldLambda);
            if (!maxValue.HasValue)
                maxValue = min;
            else
                maxValue = maxValue.Value + 1;
            if (maxValue > max)
                throw new ArgumentException("最大值超过" + max);
            return maxValue.GetValueOrDefault();
        }

        #region DataTable
        protected DataTable GetDataTableBySql(string sql, CommandType type, params DbParameter[] parms)
        {
            using (var conn = _context.Database.Connection)
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = type;
                if (parms != null && parms.Length > 0)
                {
                    cmd.Parameters.AddRange(parms);
                }
                var dr = cmd.ExecuteReader();
                DataTable table = new DataTable();
                if (dr.HasRows)
                {
                    table.Load(dr);
                }
                dr.Close();
                conn.Close();
                conn.Dispose();
                return table;
            }
        }
        protected DataTable GetDataTableBySql(string sql, params DbParameter[] parms)
        {
            return GetDataTableBySql(sql, CommandType.Text, parms);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _context.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~EFRepositoryWithGuidIdEntity() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
