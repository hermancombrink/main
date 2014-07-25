using System;
namespace log4net.logging
{
   public interface ILogger
    {
        log4net.ILog Log { get; }

        void SafeException(Exception ex, string msg, string TAG, LogLevel level = LogLevel.FATAL);
        Exception GetSafeException(Exception ex, string msg, string TAG, LogLevel level = LogLevel.FATAL);
        void RaiseException(string msg, string TAG, LogLevel level = LogLevel.ERROR);
        Exception GetRaiseException(string msg, string TAG, LogLevel level = LogLevel.ERROR);
        void Debug(string msg, string TAG, LogLevel level = LogLevel.DEBUG);
        void Info(string msg, string TAG, LogLevel level = LogLevel.INFO);
        void Warn(string msg, string TAG, LogLevel level = LogLevel.WARN);
       
    }
}
