using log4net.Appender;
using Qct.Infrastructure.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.Log
{
    public class Log4NetAdoNetAppender : AdoNetAppender
    {
        public new string ConnectionString
        {
            get { return ConnectionStringConfig.GetConnectionString(ConnectionStringName); }
            set { }
        }

        public bool Additional { get; set; }
    }
}
