using Qct.Infrastructure.Data.EnityFramework;
using System;

namespace Qct.Infrastructure.Data
{
    public interface IEntityFrameworkUnitOfWork : IUnitOfWork, IDisposable
    {
        CommonDbContext Context { get; }
    }
}
