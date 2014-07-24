using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace log4net.logging
{
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
    }
}
