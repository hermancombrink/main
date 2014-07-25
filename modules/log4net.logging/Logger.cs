using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace log4net.logging
{
    public enum LogLevel
    {
        ERROR, 
        WARN, 
        INFO, 
        DEBUG,
        FATAL
    }
    public class Logger : log4net.logging.ILogger
    {

        public Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private ILog _logger;
        public ILog Log
        {
            get
            {
                if (_logger == null)
                {
                    var logname = System.Reflection.Assembly.GetCallingAssembly().FullName;
                    log4net.Config.XmlConfigurator.Configure();
                    _logger = log4net.LogManager.GetLogger(logname);
                    _logger.Info("Spawning new logger");
                }
                return _logger;
            }
        }

        private void LogMe(string msg, string TAG, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.FATAL:
                    { this.Log.Fatal(msg); break; }
                case LogLevel.ERROR:
                    { this.Log.Error(msg); break; }
                case LogLevel.WARN:
                    { this.Log.Warn(msg); break; }
                case LogLevel.INFO:
                    { this.Log.Info(msg); break; }
                case LogLevel.DEBUG:
                    { this.Log.Debug(msg); break; }
                default:
                    break;
            }
        }

        public void RaiseException(string msg, string TAG, LogLevel level = LogLevel.ERROR)
        {
            LogMe(msg, TAG, level);
            throw new Exception(msg);
        }

        public void SafeException(Exception ex, string msg, string TAG, LogLevel level = LogLevel.FATAL)
        {
            LogMe(msg, TAG, level);
            throw new Exception(msg, ex);
        }

        public void Debug(string msg, string TAG, LogLevel level = LogLevel.DEBUG)
        {
            LogMe(msg, TAG, level);
        }

        public void Info(string msg, string TAG, LogLevel level = LogLevel.INFO)
        {
            LogMe(msg, TAG, level);
        }

        public void Warn(string msg, string TAG, LogLevel level = LogLevel.WARN)
        {
            LogMe(msg, TAG, level);
        }

        Exception ILogger.GetSafeException(Exception ex, string msg, string TAG, LogLevel level = LogLevel.FATAL)
        {
            LogMe(msg, TAG, level);
            return new Exception(msg, ex);
        }

        Exception ILogger.GetRaiseException(string msg, string TAG, LogLevel level = LogLevel.ERROR)
        {
            LogMe(msg, TAG, level);
            return new Exception(msg);
        }
    }
}
