using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Config;
using System.IO;

namespace ToDoWeb
{
    /// <summary>
    /// logging events
    /// </summary>
    public class LogFactory
    {
        static ILog Log;
        static LogFactory()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));
        }
        public static void InitializeLogServiceFactory(ILog log)
        {
            Log = log;
        }
        /// <summary>
        /// Get logger
        /// </summary>
        /// <returns></returns>
        public static ILog GetLogService()
        {
            return Log;
        }
    }
}
