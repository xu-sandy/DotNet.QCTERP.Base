using log4net;
using System;

namespace Qct.Infrastructure.Log
{
    public class LoggerWithLog4Net<T> : ILogger
        where T : BaseLogContent
    {
        ILog log;
        T additional;
        public LoggerWithLog4Net(string module, T additional)
        {
            this.additional = additional;
            this.additional.ModuleName = module;
            log = LogManager.GetLogger("pharos");
        }
        private void FormatMessage(string message)
        {
            additional.Summary = message;
        }
        private void FormatMessage(string message, byte level)
        {
            additional.Summary = message;
        }
        private void FormatMessage(string message, Exception ex)
        {
            additional.Summary = additional.FormatMessage(ex);
        }
        public void Debug(string message)
        {
            FormatMessage(message);
            log.Debug(additional);
        }

        public void Debug(string message, Exception exception)
        {
            FormatMessage(message, exception);
            log.Debug(additional);
        }

        public void Error(string message)
        {
            FormatMessage(message);
            log.Error(additional);
        }

        public void Error(string message, Exception exception)
        {
            FormatMessage(message, exception);
            log.Error(additional);
        }

        public void Fatal(string message)
        {
            FormatMessage(message);
            log.Fatal(additional);
        }

        public void Fatal(string message, Exception exception)
        {
            FormatMessage(message, exception);
            log.Fatal(additional);
        }

        public void Info(string message)
        {
            FormatMessage(message);
            log.Info(additional);
        }

        public void Info(string message, Exception exception)
        {
            FormatMessage(message, exception);
            log.Info(additional);
        }

        public void Info(string message, byte level)
        {
            FormatMessage(message, level);
            log.Info(additional);
        }

        public void WriteLogContent()
        {
            log.Info(additional);
        }
        public void Warn(string message)
        {
            FormatMessage(message);
            log.Warn(additional);
        }

        public void Warn(string message, Exception exception)
        {
            FormatMessage(message, exception);
            log.Warn(additional);
        }

        
    }
}
