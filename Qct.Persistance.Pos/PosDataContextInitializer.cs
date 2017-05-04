using System.Data.Entity;

namespace Qct.Persistance.Pos
{
    public class PosDataContextInitializer : IDatabaseInitializer<PosDataContext>
    {
        public void InitializeDatabase(PosDataContext context)
        {
            if (!context.Database.Exists())
            {
                context.Database.Create();
            }
        }
    }
}
