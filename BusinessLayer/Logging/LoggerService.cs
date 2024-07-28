using System;
using NLog;

namespace BusinessLayer.Logging
{
    public class LoggerService : ILoggerService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            logger.Error(ex, message);
        }
    }
}
