using System;
namespace log4net.logging
{
   public interface ILogger
    {
        log4net.ILog Log { get; }
    }
}
