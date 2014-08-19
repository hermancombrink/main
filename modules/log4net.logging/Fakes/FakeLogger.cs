using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net.logging.Fakes
{
    public class FakeLogger : ILogger
    {
        public ILog Log
        {
            get { return new LogFake(); }
        }

        public void SafeException(Exception ex, string msg, string TAG, LogLevel level = LogLevel.FATAL)
        {
            return;
        }

        public Exception GetSafeException(Exception ex, string msg, string TAG, LogLevel level = LogLevel.FATAL)
        {
            throw new NotImplementedException();
        }

        public void RaiseException(string msg, string TAG, LogLevel level = LogLevel.ERROR)
        {
            throw new NotImplementedException();
        }

        public Exception GetRaiseException(string msg, string TAG, LogLevel level = LogLevel.ERROR)
        {
            throw new NotImplementedException();
        }

        public void Debug(string msg, string TAG, LogLevel level = LogLevel.DEBUG)
        {
            return;
        }

        public void Info(string msg, string TAG, LogLevel level = LogLevel.INFO)
        {
            return;
        }

        public void Warn(string msg, string TAG, LogLevel level = LogLevel.WARN)
        {
            return;
        }
    }
}
