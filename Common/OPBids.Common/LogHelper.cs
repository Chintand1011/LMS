using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace OPBids.Common
{
    public class LogHelper
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void LogInfo(string message)
        {
            logger.Info(message);
        }

        public static void LogError(Exception ex, string message)
        {
            logger.Error(ex, message);
        }

        public static void LogFatal(Exception ex, string message)
        {
            logger.Fatal(ex, message);
        }

        private static void Log(LogLevel logLevel, string strMessage, Exception ex = null)
        {
            LogManager.GetCurrentClassLogger().Log(logLevel, strMessage);
        }
    }
}
