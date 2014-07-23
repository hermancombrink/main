using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace log4net.logging
{
    public static class Logger
    {
        private static ILog _logger;
        public static ILog Log
        {
            get {
                if (_logger == null)
                {
                    var logname = System.Reflection.Assembly.GetCallingAssembly().FullName;
                    log4net.Config.XmlConfigurator.Configure();
                    _logger = log4net.LogManager.GetLogger(logname);
                }
                return _logger;
            }
        }
    }
}
