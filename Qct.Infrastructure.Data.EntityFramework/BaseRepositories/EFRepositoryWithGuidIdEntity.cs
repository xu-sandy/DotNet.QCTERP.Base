using Qct.Infrastructure.Data.EnityFramework;
using Qct.Infrastructure.Data.EntityInterface;
using Qct.Infrastructure.Exceptions;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Qct.Infrastructure.Data.BaseRepositories
{
    public class EFRepositoryWithGuidIdEntity<T> : IEFRepository<T>
        where T : class, GuidIdEntity, new()
    {
        protected readonly CommonDbContext _context;
        protected readonly IEntityFrameworkUnitOfWork _unitOfWork;

        public EFRepositoryWithGuidIdEntity(IEntityFrameworkUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {
            _unitOfWork = unitOfWork;
        }
        public EFRepositoryWithGuidIdEntity(CommonDbContext dataContext)
        {
            _context = dataContext;
        }
        public void Create(T item)
        {
            _context.Set<T>().Add(item);
        }

        public void Delete(object id, bool notFindThowException = false)
        {
            var _Id = (Guid)id;
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
            var _Id = (Guid)id;
            var entity = GetReadOnlyEntities().FirstOrDefault(o => o.Id == _Id);
            return entity;
        }
        public T Get(object id)
        {
            var _Id = (Guid)id;
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
    }
}
