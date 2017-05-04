using Qct.Infrastructure.Data.EnityFramework;
using System.Runtime.Remoting.Messaging;

namespace Qct.Infrastructure.Data
{
    public class DataContextFactory
    {
        public static TDataContext Create<TDataContext>(string key = "EntityFrameworkDbContext") where TDataContext : CommonDbContext, new()
        {
            TDataContext _dbContext = CallContext.GetData(key) as TDataContext;

            if (_dbContext == null)
            {
                _dbContext = new TDataContext();
                CallContext.SetData(key, _dbContext);
            }
            return _dbContext;
        }
    }
}
