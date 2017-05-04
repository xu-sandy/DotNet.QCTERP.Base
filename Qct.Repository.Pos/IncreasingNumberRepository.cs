using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.BaseRepositories;
using Qct.Persistance.Pos;
using Qct.Objects.Entities;

namespace Qct.Repository
{
    /// <summary>
    /// 流水号资源库
    /// </summary>
    public class IncreasingNumberRepository : EFRepositoryWithStringIdEntity<IncreasingNumber>
    {
        public IncreasingNumberRepository() : base(DataContextFactory.Create<PosDataContext>())
        { }
    }
}
