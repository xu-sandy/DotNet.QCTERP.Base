namespace Qct.Persistance.Pos
{
    public static class PosDataContextConnectionString
    {
        internal static string GetConnectionString()
        {
            return @"Data Source=D:\PosDb.sdf;Max Database Size=4091;Password=pharos@2017@pos@db;Mode=Exclusive;";
        }
    }
}
