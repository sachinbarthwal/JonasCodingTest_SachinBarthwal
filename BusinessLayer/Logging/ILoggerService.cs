using System;

namespace BusinessLayer.Logging
{
    public interface ILoggerService
    {
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        void Error(string message);
        void Error(Exception ex, string message);
    }
}
