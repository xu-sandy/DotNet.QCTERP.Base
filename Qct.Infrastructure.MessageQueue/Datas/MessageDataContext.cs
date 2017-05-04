using Qct.Infrastructure.Data.EnityFramework;
using Qct.Infrastructure.MessageClient.Implementations;
using System.Data.Entity;

namespace Qct.Infrastructure.MessageClient.Datas
{
    public class MessageDataContext : CommonDbContext
    {
        public MessageDataContext() : base("Data Source=|DataDirectory|Message.sdf")
        {
        }
        public DbSet<EventJsonWrapper> FailedEvents { get; set; }


    }
}
