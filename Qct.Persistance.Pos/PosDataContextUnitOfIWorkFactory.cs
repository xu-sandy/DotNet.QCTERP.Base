using Qct.Infrastructure.Data;

namespace Qct.Persistance.Pos
{
    public static class PosDataContextUnitOfIWorkFactory
    {
        public static IEntityFrameworkUnitOfWork Create()
        {
            return new DefaultEntityFrameworkUnitOfWork(new PosDataContext());
        }
    }
}
