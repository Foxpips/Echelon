using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Echelon.Core.Logging.Resources;
using log4net;
using log4net.Appender;
using log4net.Config;

namespace Echelon.Core.Logging.Loggers
{
    public class ClientLogger : IClientLogger
    {
        private const string Log4NetFile = "log4net.xml";
        private ILog Logger { get; }

        public ClientLogger()
        {
            Configure();
            Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        private static void Configure()
        {
            XmlConfigurator.Configure(
                new MemoryStream(Encoding.UTF8.GetBytes(ResourceLoader.GetResourceContent(Log4NetFile))));
            SetOutputPath();
        }

        private static void SetOutputPath()
        {
            var appender = LogManager.GetRepository().GetAppenders().First(x => x is RollingFileAppender);
            var fileAppender = (FileAppender) appender;
            fileAppender.File =
                $"{Path.GetDirectoryName(fileAppender.File)}\\{DateTime.Now.ToString("dd-MM-yyyy")}\\log.txt";
            fileAppender.ActivateOptions();
        }

        public void Info(object message)
        {
            Logger.Info(message);
        }

        public void Debug(object message)
        {
            Logger.Debug(message);
        }

        public void Error(object message, Exception exception = null)
        {
            Logger.Error(message, exception);
        }

        public void Fatal(object message, Exception exception = null)
        {
            Logger.Fatal(message);
        }

        public void Warn(object message, Exception exception = null)
        {
            Logger.Warn(message);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Logger.ErrorFormat(format, args);
        }

        public void Dispose()
        {
            Logger.Logger.Repository.Shutdown();
        }
    }
}