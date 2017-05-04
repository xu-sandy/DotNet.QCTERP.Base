using log4net.Layout;

namespace Qct.Infrastructure.Log
{
    public class Log4NetPatternLayout : PatternLayout
    {
        public Log4NetPatternLayout()
        {
            AddConverter("property", typeof(Log4NetPatternLayoutConverter));
        }
    }
}
