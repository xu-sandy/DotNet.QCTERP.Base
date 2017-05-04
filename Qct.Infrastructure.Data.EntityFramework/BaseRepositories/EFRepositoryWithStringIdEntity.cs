using Qct.Infrastructure.Data.EnityFramework;
using Qct.Infrastructure.Data.EntityInterface;
using Qct.Infrastructure.Exceptions;
using System.Linq;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Qct.Infrastructure.Data.BaseRepositories
{
    public class EFRepositoryWithStringIdEntity<T> : IEFRepository<T>
        where T : class, StringIdEntity,new()
    {
        protected readonly CommonDbContext _context;
        protected readonly IEntityFrameworkUnitOfWork _unitOfWork;

        public EFRepositoryWithStringIdEntity(IEntityFrameworkUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {
            _unitOfWork = unitOfWork;
        }
        public EFRepositoryWithStringIdEntity(CommonDbContext dataContext)
        {
            _context = dataContext;
        }
        public void Create(T item)
        {
            _context.Set<T>().Add(item);
        }

        public void Delete(object id, bool notFindThowException = false)
        {
            var _Id = (string)id;
            var entities = _context.Set<T>();
            var entity = entities.FirstOrDefault(o => o.Id == _Id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
            else if (notFindThowException)
            {
                throw new DataException(string.Format("未找到id为[{0}]的项目！", _Id));
            }
        }
        public T GetByReadOnly(object id)
        {
            var _Id = (string)id;
            var entity = GetReadOnlyEntities().FirstOrDefault(o => o.Id == _Id);
            return entity;
        }
        public T Get(object id)
        {
            var _Id = (string)id;
            var entity = GetEntities().FirstOrDefault(o => o.Id == _Id);
            return entity;
        }
        public IQueryable<T> GetEntities()
        {
            return _context.Trackable<T>();
        }
        public IQueryable<T> GetReadOnlyEntities()
        {
            return _context.ReadOnly<T>();
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
        public void Removes(params T[] entities)
        {
            if (entities != null && entities.Any())
                _context.Set<T>().RemoveRange(entities);
        }

        public void RemoveWithSaveChanges(params T[] entities)
        {
            Removes(entities);
            SaveChanges();
        }
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

        public dynamic AddOrUpdate(params T[] obj)
        {
            _context.Set<T>().AddOrUpdate(obj);
            SaveChanges();
            return true;
        }

        public void DeletesWithSaveChanges(object[] ids, string idName = "Id")
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
