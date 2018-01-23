using System;

namespace Echelon.Core.Logging.Loggers
{
    public interface IClientLogger
    {
        void Info(object message);
        void Debug(object message);
        void Error(object message, Exception exception = null);
        void Fatal(object message, Exception exception = null);
        void Warn(object message, Exception exception = null);
        void ErrorFormat(string format, params object[] args);
        void Dispose();
    }
}