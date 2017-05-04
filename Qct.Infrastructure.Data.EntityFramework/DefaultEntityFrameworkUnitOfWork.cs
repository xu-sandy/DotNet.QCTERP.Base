using Qct.Infrastructure.Data.EnityFramework;
using Qct.Infrastructure.Exceptions;
using System;
using System.Data.Entity;

namespace Qct.Infrastructure.Data
{
    public sealed class DefaultEntityFrameworkUnitOfWork : IEntityFrameworkUnitOfWork
    {
        private readonly DbContextTransaction transaction;
        private bool comitted;
        private bool rolledBack;
        private bool disposed;

        public CommonDbContext Context { get; private set; }

        public DefaultEntityFrameworkUnitOfWork(CommonDbContext context)
        {
            if (context == null)
                throw new DataException("DbContext 不能为Null!");
            Context = context;
            transaction = Context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                Context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }

            comitted = true;
        }

        public void Rollback()
        {
            transaction.Rollback();

            Context.Dispose();

            rolledBack = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (!comitted && !rolledBack)
                    {
                        Commit();
                    }

                    Context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
