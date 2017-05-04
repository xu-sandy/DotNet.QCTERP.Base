using System;
using System.Linq;

namespace Qct.Infrastructure.Data
{


    public interface IRepository<T>
    {
        IQueryable<T> GetEntities();

        void Create(T item);
        void Delete(object id, bool notFindThowException = false);
        T Get(object id);
    }
}
