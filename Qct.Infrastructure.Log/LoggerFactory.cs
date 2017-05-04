namespace Qct.Infrastructure.Log
{
    public static class LoggerFactory
    {
        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="module">记录日志的模块</param>
        /// <param name="additional">附加信息</param>
        /// <param name="type">日志类型</param>
        /// <returns>日志记录器</returns>
        public static ILogger Create<T>(string module, T additional, LoggerType type = LoggerType.Log4net)
            where T : BaseLogContent
        {
            ILogger logger = null;
            switch (type)
            {
                case LoggerType.Log4net:
                    logger = new LoggerWithLog4Net<T>(module, additional);
                    break;
            }
            return logger;
        }

        public static void CreateWithSave<T>(T additional, LoggerType type = LoggerType.Log4net)
            where T : BaseLogContent
        {
            ILogger logger = Create(additional.ModuleName, additional, type);
            logger.WriteLogContent();
        }
    }
}
